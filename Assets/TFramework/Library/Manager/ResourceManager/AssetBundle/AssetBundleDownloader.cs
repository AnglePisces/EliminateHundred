using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleInfo
{
	public string sPath;
	public string sVersion;
}

public class AssetBundleDownloader
{

	private Dictionary<string, AssetBundleInfo> m_AssetBundleInfos;//name,info网络获取信息
	private Dictionary<string, AssetBundleInfo> m_LocalAssetBundleInfos;//name,info本地信息
	private Dictionary<string, AssetBundleInfo> m_DownloadAssetBundleInfos;//name,info需要下载信息
	private bool m_IsDone = false;//下载是否完成标记
	private string m_Error;//下载错误信息,为空时下载正常
	private float m_Progress = 0;//下载进度
    private int m_AmountOfAB = 0;
    private int m_NumOfDowloadedAB = 0;
    private int m_DownloadedBytes = 0;
    private int m_Size = 0;
    public delegate void FinishDownload();
    public FinishDownload FinishDownloadDelegate = null;
    public bool isDone
    {
        get
        {
            return m_IsDone;
        }
    }

    public string error
    {
        get
        {
            return m_Error;
        }
    }
    
    public float progress
    {
        get
        {
            return m_Progress;
        }
    }
    //资源包数量
    public int amountOfAB
    {
        get
        {
            return m_AmountOfAB;
        }
    }
    //已下载资源包数量
    public int numOfDowloadedAB
    {
        get
        {
            return m_NumOfDowloadedAB;
        }
    }
    //当前下载包已下载大小
    public int downloadedBytes
    {
        get
        {
            return m_DownloadedBytes;
        }
    }
    //当前下载包总大小
    public int size
    {
        get
        {
            return m_Size;
        }
    }
    public AssetBundleDownloader(string assetBundleInfo,string serverPath)
	{
		if (string.IsNullOrEmpty(assetBundleInfo))
		{
			m_IsDone = true;
			m_Error = null;
          	m_Progress = 1;
			return;
		}
		//解析字符串
		parseInfo (assetBundleInfo, serverPath);
		//读取本地资源包信息
        string fileName = Application.persistentDataPath + "/" + ConstString.sAssetBundleInfoXML;
        AssetBundleXMLUtils.loadLocalInfo(ref m_LocalAssetBundleInfos, fileName);
		//过滤出需要更新的资源包
		selectAssetBudlesForUpdate();
	}

    /// <summary>
    /// 下载资源包函数
    /// </summary>
    /// <param name="finishDownload"></param>
    /// <returns></returns>
	public IEnumerator StartDownload()
	{
        Debug.Log("In Start DownLoad~~~~~~~~~~~~~~||");
        m_AmountOfAB = m_DownloadAssetBundleInfos.Count;
		if (m_DownloadAssetBundleInfos == null || m_DownloadAssetBundleInfos.Count == 0)
		{
			m_IsDone = true;
			m_Error = null;
			m_Progress = 1;
			yield return null;
		}
        Debug.Log("Show DownloadAsset Count:" + m_DownloadAssetBundleInfos.Count);
        foreach (KeyValuePair<string, AssetBundleInfo> assetBundle in m_DownloadAssetBundleInfos)
		{
			AssetBundleInfo strcAssetBundleInfo = assetBundle.Value;
			WWW w = new WWW(strcAssetBundleInfo.sPath);
            m_Size = w.bytesDownloaded;
            m_DownloadedBytes += w.bytesDownloaded;
            Debug.Log("Show strcAssetBundleInfo.sPath: " + strcAssetBundleInfo.sPath);
            yield return w;

            if (w.error != null)
            {
                if (string.IsNullOrEmpty(m_Error))
                {
                    m_Error = assetBundle.Key + ":" + w.error + ";";
                }
                else
                {
                    m_Error += assetBundle.Key + ":" + w.error + ";";
                }

                Debug.Log("m_Error: "+ m_Error+"@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            }
            else if (w.isDone)
            {
                byte[] model = w.bytes;
                int length = model.Length;
                //写入文件到本地  
                string sAssetBundlePath = Application.persistentDataPath;
                CreateModelFile(sAssetBundlePath, assetBundle.Key, model, length);
                m_NumOfDowloadedAB++;
                m_Progress = (float)m_NumOfDowloadedAB / m_AmountOfAB;
                Debug.Log("Create File Over~~~~~~~~~~~~~~");
                //写入本地记录
                if (m_LocalAssetBundleInfos.ContainsKey(assetBundle.Key))
                {
                    m_LocalAssetBundleInfos[assetBundle.Key].sPath = strcAssetBundleInfo.sPath;
                    m_LocalAssetBundleInfos[assetBundle.Key].sVersion = strcAssetBundleInfo.sVersion;
                }
                else
                {
                    m_LocalAssetBundleInfos.Add(assetBundle.Key, assetBundle.Value);
                }
                //保存信息文件到本地
                if (string.IsNullOrEmpty(m_Error) || !m_Error.Contains(assetBundle.Key))
                { 
                    string fileName = Application.persistentDataPath + "/" + ConstString.sAssetBundleInfoXML;
                    AssetBundleXMLUtils.saveLocalInfo(m_LocalAssetBundleInfos, fileName);
                }   
            }
		}

        Debug.Log("DownLoad Done~~~~~~~~~~~");
        m_IsDone = true;
        if (FinishDownloadDelegate != null)
        {
            FinishDownloadDelegate();
            FinishDownloadDelegate = null;
        }
	}
	private void CreateModelFile(string path, string name, byte[] info, int length)  
	{  
		//文件流信息  
		//StreamWriter sw;  
		Stream sw;  
		FileInfo t = new FileInfo(path + "/" + name);  
		if (!t.Exists)  
		{  
			//如果此文件不存在则创建  
			sw = t.Create();  
		}  
		else  
		{  
			//如果此文件存在则打开  
			sw = t.Create();  
			return;  
		}  
		//以行的形式写入信息  
		//sw.WriteLine(info);  
		sw.Write(info, 0, length);  
		//关闭流  
		sw.Close();  
		//销毁流  
		sw.Dispose();  
	}  
    /// <summary>
    /// 解析网络返回的资源包信息
    /// </summary>
    /// <param name="assetBundleInfo">网络资源包信息（格式：包名1,路径1,版本号1;包名2,路径2,版本号2）</param>
	private void parseInfo(string assetBundleInfo, string serverPath)
	{
		m_AssetBundleInfos = new Dictionary<string, AssetBundleInfo> ();
        string[] assetBundleInfos = assetBundleInfo.Split(';');
		for (int i = 0; i < assetBundleInfos.Length; ++i) 
		{
            if(string.IsNullOrEmpty(assetBundleInfos[i]))
            {
                continue;
            }
			AssetBundleInfo assetBundle = new AssetBundleInfo();
			string[] singleAssetBundle = assetBundleInfos[i].Split(',');
			assetBundle.sPath = serverPath + singleAssetBundle[0];
			assetBundle.sVersion = singleAssetBundle[1];
			m_AssetBundleInfos.Add(singleAssetBundle[0], assetBundle);
		}
	}
    
    /// <summary>
    /// 对比版本号,选出需要更新的资源包
    /// </summary>
	private void selectAssetBudlesForUpdate()
	{
		m_DownloadAssetBundleInfos = new Dictionary<string, AssetBundleInfo> ();
		foreach (KeyValuePair<string, AssetBundleInfo> assetBundle in m_AssetBundleInfos) 
		{
			string sKey = assetBundle.Key;
			AssetBundleInfo oValue = assetBundle.Value;
			if(!m_LocalAssetBundleInfos.ContainsKey(sKey) 
			   || m_LocalAssetBundleInfos[sKey].sVersion != assetBundle.Value.sVersion)
			{
				m_DownloadAssetBundleInfos.Add(sKey, oValue);
			}
		}
	}

}
