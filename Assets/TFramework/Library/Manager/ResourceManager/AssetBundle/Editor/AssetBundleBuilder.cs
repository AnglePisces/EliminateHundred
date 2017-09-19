using UnityEngine;
using UnityEditor;

using System.Xml;
using System.Security.Cryptography;

using System.IO;
using System.Collections;
using System.Collections.Generic;

    public class AssetBundleBuilder : Editor  
    {
        [MenuItem("AssetBundleTools/CreateAssetBundle/CreateAssetBundleForWindows")]
        static void OnCreateAssetBundleWindows()  
        {
            BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundle/StandaloneWindows64", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
            //刷新 
            AssetDatabase.Refresh();  
            Debug.Log("AssetBundle打包完毕");  
        }

        [MenuItem("AssetBundleTools/CreateAssetBundle/CreateAssetBundleForiOS")]
        static void OnCreateAssetBundleIOS()
        {
            BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundle/iOS", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.iOS);

            //刷新 
            AssetDatabase.Refresh();
            Debug.Log("AssetBundle打包完毕");
        }

        [MenuItem("AssetBundleTools/CreateAssetBundle/CreateAssetBundleForAndroid")]
        static void OnCreateAssetBundleAndroid()
        {
            BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundle/Android", BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.Android);

            //刷新 
            AssetDatabase.Refresh();
            Debug.Log("AssetBundle打包完毕");
        }
    }


public class CreateMD5List
{
    public static void Execute(UnityEditor.BuildTarget target)
    {
        string platform = target.ToString();
        Execute(platform);
        AssetDatabase.Refresh();
    }

    public static void Execute(string platform)
    {
        Dictionary<string, string> DicFileMD5 = new Dictionary<string, string>();
        MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();

        string dir = System.IO.Path.Combine(Application.dataPath, "AssetBundle/" + platform);
        foreach (string filePath in Directory.GetFiles(dir))
        {
            if (filePath.Contains(".meta") || filePath.Contains("VersionMD5") || filePath.Contains(".xml"))
                continue;

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(file);
            string strMD5 = System.BitConverter.ToString(hash);
            file.Close();

            string key = filePath.Substring(dir.Length + 1, filePath.Length - dir.Length - 1);

            if (DicFileMD5.ContainsKey(key) == false)
                DicFileMD5.Add(key, strMD5);
            else
                Debug.LogWarning("<Two File has the same name> name = " + filePath);
        }

        string savePath = System.IO.Path.Combine(Application.dataPath, "AssetBundle/") + platform + "/VersionNum";
        if (Directory.Exists(savePath) == false)
            Directory.CreateDirectory(savePath);

        // 删除前一版的old数据
        if (File.Exists(savePath + "/VersionMD5-old.xml"))
        {
            System.IO.File.Delete(savePath + "/VersionMD5-old.xml");
        }

        // 如果之前的版本存在，则将其名字改为VersionMD5-old.xml
        if (File.Exists(savePath + "/VersionMD5.xml"))
        {
            System.IO.File.Move(savePath + "/VersionMD5.xml", savePath + "/VersionMD5-old.xml");
        }

        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("Files");
        XmlDoc.AppendChild(XmlRoot);
        foreach (KeyValuePair<string, string> pair in DicFileMD5)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("File");
            XmlRoot.AppendChild(xmlElem);

            xmlElem.SetAttribute("FileName", pair.Key);
            xmlElem.SetAttribute("MD5", pair.Value);
        }

        // 读取旧版本的MD5
        Dictionary<string, string> dicOldMD5 = ReadMD5File(savePath + "/VersionMD5-old.xml");
        // VersionMD5-old中有，而VersionMD5中没有的信息，手动添加到VersionMD5
        foreach (KeyValuePair<string, string> pair in dicOldMD5)
        {
            if (DicFileMD5.ContainsKey(pair.Key) == false)
                DicFileMD5.Add(pair.Key, pair.Value);
        }

        XmlDoc.Save(savePath + "/VersionMD5.xml");
        XmlDoc = null;
    }

    static Dictionary<string, string> ReadMD5File(string fileName)
    {
        Dictionary<string, string> DicMD5 = new Dictionary<string, string>();

        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicMD5;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;

        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;

            string file = (node as XmlElement).GetAttribute("FileName");
            string md5 = (node as XmlElement).GetAttribute("MD5");

            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, md5);
            }
        }

        XmlRoot = null;
        XmlDoc = null;

        return DicMD5;
    }
    [MenuItem("AssetBundleTools/CreateMD5/CreateMD5ForWindows")]
    public static void CreateMD5ForWindows()
    {
        Execute(BuildTarget.StandaloneWindows64);
    }
    [MenuItem("AssetBundleTools/CreateMD5/CreateMD5ForIOS")]
    public static void CreateMD5ForIOS()
    {
        Execute(BuildTarget.iOS);
    }

    [MenuItem("AssetBundleTools/CreateMD5/CreateMD5ForAndroid")]
    public static void CreateMD5ForAndroid()
    {
        Execute(BuildTarget.Android);
    }

}
public class CampareMD5ToGenerateVersionNum
{
    public static void Execute(UnityEditor.BuildTarget target)
    {
        string platform = target.ToString();
        Execute(platform);
        AssetDatabase.Refresh();
    }

    // 对比对应版本目录下的VersionMD5和VersionMD5-old，得到最新的版本号文件VersionNum.xml
    public static void Execute(string platform)
    {
        // 读取新旧MD5列表
        string newVersionMD5 = System.IO.Path.Combine(Application.dataPath, "AssetBundle/" + platform + "/VersionNum/VersionMD5.xml");
        string oldVersionMD5 = System.IO.Path.Combine(Application.dataPath, "AssetBundle/" + platform + "/VersionNum/VersionMD5-old.xml");

        Dictionary<string, string> dicNewMD5Info = ReadMD5File(newVersionMD5);
        Dictionary<string, string> dicOldMD5Info = ReadMD5File(oldVersionMD5);

        // 读取版本号记录文件VersinNum.xml
        string oldVersionNum = System.IO.Path.Combine(Application.dataPath, "AssetBundle/" + platform + "/VersionNum/VersionNum.xml");
        Dictionary<string, int> dicVersionNumInfo = ReadVersionNumFile(oldVersionNum);

        // 对比新旧MD5信息，并更新版本号，即对比dicNewMD5Info&&dicOldMD5Info来更新dicVersionNumInfo
        foreach (KeyValuePair<string, string> newPair in dicNewMD5Info)
        {
            // 旧版本中有
            if (dicOldMD5Info.ContainsKey(newPair.Key))
            {
                // MD5一样，则不变
                // MD5不一样，则+1
                // 容错：如果新旧MD5都有，但是还没有版本号记录的，则直接添加新纪录，并且将版本号设为1
                if (dicVersionNumInfo.ContainsKey(newPair.Key) == false)
                {
                    dicVersionNumInfo.Add(newPair.Key, 1);
                }
                else if (newPair.Value != dicOldMD5Info[newPair.Key])
                {
                    int num = dicVersionNumInfo[newPair.Key];
                    dicVersionNumInfo[newPair.Key] = num + 1;
                }
            }
            else // 旧版本中没有，则添加新纪录，并=1
            {
                dicVersionNumInfo.Add(newPair.Key, 1);
            }
        }
        // 不可能出现旧版本中有，而新版本中没有的情况，原因见生成MD5List的处理逻辑

        // 存储最新的VersionNum.xml
        SaveVersionNumFile(dicVersionNumInfo, oldVersionNum ,System.IO.Path.Combine(Application.dataPath, "AssetBundle/" + platform));
    }

    static Dictionary<string, string> ReadMD5File(string fileName)
    {
        Dictionary<string, string> DicMD5 = new Dictionary<string, string>();

        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicMD5;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;

        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;

            string file = (node as XmlElement).GetAttribute("FileName");
            string md5 = (node as XmlElement).GetAttribute("MD5");

            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, md5);
            }
        }

        XmlRoot = null;
        XmlDoc = null;

        return DicMD5;
    }

    static Dictionary<string, int> ReadVersionNumFile(string fileName)
    {
        Dictionary<string, int> DicVersionNum = new Dictionary<string, int>();

        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicVersionNum;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;

        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;

            string file = (node as XmlElement).GetAttribute("FileName");
            int num = XmlConvert.ToInt32((node as XmlElement).GetAttribute("Num"));

            if (DicVersionNum.ContainsKey(file) == false)
            {
                DicVersionNum.Add(file, num);
            }
        }

        XmlRoot = null;
        XmlDoc = null;

        return DicVersionNum;
    }

    static void SaveVersionNumFile(Dictionary<string, int> data, string savePath, string resourcePath)
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("VersionNum");
        long resourceSize = FileSize(resourcePath);
        XmlRoot.SetAttribute("ABSize", resourceSize.ToString());
        XmlDoc.AppendChild(XmlRoot);

        foreach (KeyValuePair<string, int> pair in data)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("File");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute("FileName", pair.Key);
            xmlElem.SetAttribute("Num", XmlConvert.ToString(pair.Value));
        }

        XmlDoc.Save(savePath);
        XmlRoot = null;
        XmlDoc = null;
    }

    private static long FileSize(string filePath)
    {
        long temp = 0;

            string[] str1 = Directory.GetFileSystemEntries(filePath);
            foreach (string s1 in str1)
            {
                if (s1.Contains(".meta") || File.Exists(s1) == false)
                {
                    continue;
                }
                temp += new FileInfo(s1).Length;
            }

        return temp;
    }

    [MenuItem("AssetBundleTools/CreateVersion/CreateVersionForWindows")]
    public static void CreateMD5ForWindows()
    {
        Execute(BuildTarget.StandaloneWindows64);
    }
    [MenuItem("AssetBundleTools/CreateVersion/CreateVersionForIOS")]
    public static void CreateMD5ForIOS()
    {
        Execute(BuildTarget.iOS);
    }

    [MenuItem("AssetBundleTools/CreateVersion/CreateVersionForAndroid")]
    public static void CreateMD5ForAndroid()
    {
        Execute(BuildTarget.Android);
    }
}  
