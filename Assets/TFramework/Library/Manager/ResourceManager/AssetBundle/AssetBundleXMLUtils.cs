using UnityEngine;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public static class AssetBundleXMLUtils{
    private const string c_sFileName = "FileName";
    private const string c_sVersion = "Version";
    private const string c_sPath = "Path";
    /// <summary>
    /// 读取本地资源包信息
    /// </summary>
    public static void loadLocalInfo(ref Dictionary<string, AssetBundleInfo> localAssetBundleInfos, string fileName)
    {
        localAssetBundleInfos = new Dictionary<string, AssetBundleInfo>();
        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
        {
            return;
        }
        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;

        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;
            string sFile = (node as XmlElement).GetAttribute(c_sFileName);
            string sVersion = (node as XmlElement).GetAttribute(c_sVersion);
            string sPath = (node as XmlElement).GetAttribute(c_sPath);

            if (!localAssetBundleInfos.ContainsKey(sFile))
            {
                AssetBundleInfo oAssetBundleInfo = new AssetBundleInfo();
                oAssetBundleInfo.sPath = sPath;
                oAssetBundleInfo.sVersion = sVersion;
                localAssetBundleInfos.Add(sFile, oAssetBundleInfo);
            }
        }

        XmlRoot = null;
        XmlDoc = null;

    }
    /// <summary>
    /// 保存本地资源包信息
    /// </summary>
    /// <param name="localAssetBundleInfos"></param>
    public static void saveLocalInfo(Dictionary<string, AssetBundleInfo> localAssetBundleInfos, string fileName)
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("FileInfo");
        XmlDoc.AppendChild(XmlRoot);

        foreach (KeyValuePair<string, AssetBundleInfo> pair in localAssetBundleInfos)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("File");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute(c_sFileName, pair.Key);
            xmlElem.SetAttribute(c_sVersion, pair.Value.sVersion);
            xmlElem.SetAttribute(c_sPath, Application.persistentDataPath + "/" + pair.Key);
        }
        XmlDoc.Save(fileName);
        XmlRoot = null;
        XmlDoc = null;
    }
}
