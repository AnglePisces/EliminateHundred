using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class AssetBundleLoader
{
    private Dictionary<string, AssetBundleInfo> m_LocalAssetBundleInfos;//name,info 名称与路径、版本号信息
    private List<string> m_XMLPathList;
    private List<string> m_AudioClipPathList;
    private List<string> m_TexturePathList;
    private List<string> m_SpritePathList;
    private List<string> m_PrefabPathList;
    private List<string> m_MaterialPathList;
    private List<string> m_AnimatorPathList;
    private List<string> m_JsonPathList;

    /// <summary>
    /// 初始化函数
    /// </summary>
    public void init()
    {
        //读取本地资源包信息
        string fileName = Application.persistentDataPath + "/" + ConstString.sAssetBundleInfoXML;
        AssetBundleXMLUtils.loadLocalInfo(ref m_LocalAssetBundleInfos, fileName);
        //资源包名称归类
        InitPathInfo();
    }
    /// <summary>
    /// 初始化各类型资源包路径
    /// </summary>
    private void InitPathInfo()
    {
        m_XMLPathList = new List<string>();
        m_AudioClipPathList = new List<string>();
        m_TexturePathList = new List<string>();
        m_SpritePathList = new List<string>();
        m_PrefabPathList = new List<string>();
        m_MaterialPathList = new List<string>();
        m_AnimatorPathList = new List<string>();
        m_JsonPathList = new List<string>();
        List<string> abInfoKeysList = new List<string>(m_LocalAssetBundleInfos.Keys);
        for (int i = 0; i < abInfoKeysList.Count; ++i)
        {
            string key = abInfoKeysList[i];
            if (key.Contains(ConstString.sManifest))
            {
                continue;
            }
            else if (key.Contains(ConstString.sAnimator))
            {
                m_AnimatorPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sAudioClip))
            {
                m_AudioClipPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sJson))
            {
                m_JsonPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sMaterial))
            {
                m_MaterialPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sPrefab))
            {
                m_PrefabPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sSprite))
            {
                m_SpritePathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sTexture))
            {
                m_TexturePathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sTexture))
            {
                m_TexturePathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
            else if (key.Contains(ConstString.sXMLDocument))
            {
                m_XMLPathList.Add(m_LocalAssetBundleInfos[key].sPath);
            }
        }
    }
    public XmlDocument GetXMLDocumentByName(string xmlName)
    {
        XmlDocument xml = null;

        for (int i = 0; i < m_XMLPathList.Count; ++i)
        {
            TextAsset textAsset = loadAssetBundle(m_XMLPathList[i], xmlName) as TextAsset;

            if (textAsset != null)
            {
                xml = new XmlDocument();
                xml.LoadXml(textAsset.text);
                break;
            }
        }

        return xml;
    }

    /// <summary>
    /// 获取音频
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>AudioClip音频</returns>
    public AudioClip GetAudioClipResourceByName(string resName)
    {
        AudioClip audio = null;
        for (int i = 0; i < m_AudioClipPathList.Count; ++i)
        {
            audio = loadAssetBundle(m_AudioClipPathList[i], resName) as AudioClip;
            if (audio != null)
            {
                break;
            }
        }

        return audio;
    }

    /// <summary>
    /// 获取贴图
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Texture贴图</returns>
    public Texture GetTextureResourceByName(string resName)
    {
        Texture texture = null;
        for (int i = 0; i < m_TexturePathList.Count; ++i)
        {
            texture = loadAssetBundle(m_TexturePathList[i], resName) as Texture;
            if (texture != null)
            {
                break;
            }
        }

        return texture;
    }

    /// <summary>
    /// 获取精灵贴图
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Sprite贴图</returns>
    public Sprite GetSpriteResourceByName(string resName)
    {
        Sprite sprite = null;
        for (int i = 0; i < m_TexturePathList.Count; ++i)
        {

            string assetBundlePath = m_TexturePathList[i];
            //判断文件是否存在
            if (!File.Exists(assetBundlePath))
            {
                return null;
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (null == assetBundle)
            {
                return null;
            }
            sprite = assetBundle.LoadAsset<Sprite>(resName);
            assetBundle.Unload(false);
            if (sprite != null)
            {
                break;
            }
        }
        return sprite;
    }


    /// <summary>
    /// 获取精灵贴图数组
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Sprite贴图数组</returns>
    public Sprite[] GetSpriteArrayResourceByName(string resName)
    {
        Sprite[] spriteArray = null;
        for (int i = 0; i < m_TexturePathList.Count; ++i)
        {

            string assetBundlePath = m_TexturePathList[i];
            //判断文件是否存在
            if (!File.Exists(assetBundlePath))
            {
                return null;
            }
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (null == assetBundle)
            {
                return null;
            }
            spriteArray = assetBundle.LoadAssetWithSubAssets<Sprite>(resName);
            assetBundle.Unload(false);
            if (spriteArray != null)
            {
                break;
            }
        }
        return spriteArray;
    }

    /// <summary>
    /// 获取预制
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>GameObject预制 未实例化</returns>
    public GameObject GetPrefabResourceByName(string resName)
    {
        GameObject obj = null;
        for (int i = 0; i < m_PrefabPathList.Count; ++i)
        {
            obj = loadAssetBundle(m_PrefabPathList[i], resName) as GameObject;
            if (obj != null)
            {
                break;
            }
        }
        return obj;
    }

    /// <summary>
    /// 获取实例化对应预制后的对象
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>GameObject 实例化后的对象</returns>
    //public GameObject GetIniPrefabResourceByName(string resName)
    //{
    //    GameObject obj = (GameObject)GetPrefabResourceByName(resName);

    //    obj = (GameObject)Instantiate(obj);

    //    return obj;
    //}

    /// <summary>
    /// 获取材质
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Material 材质球</returns>
    public Material GetMaterialResourceByName(string resName)
    {
        Material material = null;
        for (int i = 0; i < m_MaterialPathList.Count; ++i)
        {
            material = loadAssetBundle(m_MaterialPathList[i], resName) as Material;
            if (material != null)
            {
                break;
            }
        }

        return material;
    }

    /// <summary>
    /// 获取Json文本 Resources本地读取
    /// </summary>
    /// <param name="jsonName">json文件名</param>
    /// <returns></returns>
    public TextAsset GetJsonTextByName(string jsonName)
    {

        TextAsset ta = null;
        for (int i = 0; i < m_JsonPathList.Count; ++i)
        {
            ta = loadAssetBundle(m_JsonPathList[i], jsonName) as TextAsset;
            if (ta != null)
            {
                break;
            }
        }

        return ta;
    }

    /// <summary>
    /// 读取assetBundle资源
    /// </summary>
    /// <param name="assetBundlePath">assetBundle文件本地路径</param>
    /// <param name="objectName">资源文件名称</param>
    /// <returns>返回统一的Object类型</returns>
    private Object loadAssetBundle(string assetBundlePath, string objectName)
    {
        //判断文件是否存在
        if (!File.Exists(assetBundlePath))
        {
            return null;
        }
        AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        if (null == assetBundle)
        {
            return null;
        }
        Object ob = assetBundle.LoadAsset(objectName);
        assetBundle.Unload(false);
        return ob;
    }

}
