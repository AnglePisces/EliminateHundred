using System;
using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using TFramework;

/// <summary>
/// 
/// 总的XML资源控制类
/// 
/// 1 XML结构
/// 2 XML加载
/// 3 XML 公共方法
/// 
/// </summary>

public class XMLResourcesController : MonoBehaviour
{
    #region Variables

    /*
     *
     * Xml结构关系
     * 每个XML结构(XmlStruct)中 存放XML的分类节点结构(XmlNodeStruct)
     * 每个XML分类节点结构(XmlNodeStruct)中 存放对应节点下的XML资源结构(XmlResourceStruct)
     * 
     */
    //XML结构
    protected struct XmlStruct
    {
        public string xmlName; //xml名
        public string xmlPackageName; //所在资源包名称
        public string xmlPath; //资源包下的路经
        public float xmlVersion; //xml版本
        public XmlNodeStruct xmlNodeStruct; //XML节点结构链表 存放节点
    }
    //Xml节点结构
    protected struct XmlNodeStruct
    {
        public string xmlNodeName; //节点名称
        public XmlNode xmlNode; //节点
        public XmlResourceStruct[] xmlResourceStructArray; //xml资源结构数组 存放该xml节点中的所有内容
    }
    //XML节点资源结构
    protected struct XmlResourceStruct
    {
        public string resName; //资源名
        public string resPackageName; //所在资源包名称
        public string resPath; //资源包下的路经
        public float resVersion; //资源版本
    }

    //根XML的路经
    protected string m_xmlFolderRootURL = "XML/MainResourcesInventory";
    //根XML
    protected XmlDocument m_mainXML = null;
    //根XML的节点结构链表
    protected List<XmlNodeStruct> m_mainXMLNodeStructList = null;

    /* 
     * 
     * 资源XML结构 配置表名
     * 音频 图片 预制 材质 动画控制器
     * 
     */
    protected XmlStruct m_audioXML;
    protected string m_audioXMLName = "AudioClipResourcesInventory.xml";
    protected XmlStruct m_textureXML;
    protected string m_textureXMLName = "TextureResourcesInventory.xml";
    protected XmlStruct m_prefabXML;
    protected string m_prefabXMLName = "PrefabsResourcesInventory.xml";
    protected XmlStruct m_materialXML;
    protected string m_materialXMLName = "MaterialResourcesInventory.xml";
    protected XmlStruct m_animatorXML;
    protected string m_animatorXMLName = "AnimatorResourcesInventory.xml";
    protected XmlStruct m_xmlXML;
    protected string m_xmlXMLName = "XmlResourcesInventory.xml";
    protected XmlStruct m_jsonXML;
    protected string m_jsonXMLName = "JsonResourcesInventory.xml";
    #endregion

    #region InitializationXML

    //初始化
    public void Initialization()
    {
        LoadMainXML();

        if (m_mainXML != null)
        {
            LoadResourceXML();
        }

    }

    //加载指定XML
    protected XmlDocument LoadXml(string url)
    {
        //Debug.Log("加载--" + url + "||路经的XML");
        TextAsset textAsset = Resources.Load(url) as TextAsset;
        if (textAsset == null)
        {
            Debug.LogError("加载XML文件出错: " + url);

            return null;
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(new StringReader(textAsset.text));
        return doc;
    }

    //加载主XML
    protected void LoadMainXML()
    {
        m_mainXML = LoadXml(m_xmlFolderRootURL);
        m_mainXMLNodeStructList = new List<XmlNodeStruct>();

        if (m_mainXML == null)
        {
            Debug.LogError("XML本地资源不存在 中断XML本地资源初始化");
            return;
        }

        LoadMainXMLResources();
    }

    //加载主XML中的资源
    protected void LoadMainXMLResources()
    {
        LoadMainXMLNode();
    }

    //加载主XML中的节点
    protected void LoadMainXMLNode()
    {
        XmlNodeStruct nodeStruct = new XmlNodeStruct();
        XmlNode resourceNode = m_mainXML.SelectSingleNode(XMLStaticString._resources);
        nodeStruct.xmlNodeName = XMLStaticString._resourceFile;
        //加载XML的节点
        nodeStruct.xmlNode = resourceNode.SelectSingleNode(nodeStruct.xmlNodeName);
        //加载"ResourceFile"节点下的所有资源
        nodeStruct = LoadResourcesByXMLNode(nodeStruct);
        m_mainXMLNodeStructList.Add(nodeStruct);
    }

    //从主XML中获取资源XML的结构
    protected XmlResourceStruct GetResourceXMLStructFromMainXML(string nodeName, string structName)
    {
        XmlResourceStruct resourceStruct = new XmlResourceStruct();
        XmlNodeStruct nodeStruct = new XmlNodeStruct();

        if (m_mainXMLNodeStructList != null)
        {
            for (int i = 0; i < m_mainXMLNodeStructList.Count; i++)
            {
                if (nodeName.Equals(m_mainXMLNodeStructList[i].xmlNodeName))
                {
                    nodeStruct = m_mainXMLNodeStructList[i];
                    break;
                }
            }

            if (nodeStruct.xmlResourceStructArray != null)
            {
                for (int i = 0; i < nodeStruct.xmlResourceStructArray.Length; i++)
                {
                    if (structName.Equals(nodeStruct.xmlResourceStructArray[i].resName))
                    {
                        resourceStruct = nodeStruct.xmlResourceStructArray[i];
                        return resourceStruct;
                    }
                }
            }
        }

        return resourceStruct;
    }

    //加载资源XML
    protected void LoadResourceXML()
    {
        m_audioXML = LoadResourceXMLByXmlStruct(m_audioXML, m_audioXMLName);
        m_textureXML = LoadResourceXMLByXmlStruct(m_textureXML, m_textureXMLName);
        m_prefabXML = LoadResourceXMLByXmlStruct(m_prefabXML, m_prefabXMLName);
        m_materialXML = LoadResourceXMLByXmlStruct(m_materialXML, m_materialXMLName);
        m_animatorXML = LoadResourceXMLByXmlStruct(m_animatorXML, m_animatorXMLName);
        m_xmlXML = LoadResourceXMLByXmlStruct(m_xmlXML, m_xmlXMLName);
        m_jsonXML = LoadResourceXMLByXmlStruct(m_jsonXML, m_jsonXMLName);
    }

    //加载指定资源XML结构
    protected XmlStruct LoadResourceXMLByXmlStruct(XmlStruct xmlStruct, string structName)
    {
        xmlStruct = new XmlStruct();

        XmlResourceStruct resourceStruct = GetResourceXMLStructFromMainXML(XMLStaticString._resourceFile, structName);
        xmlStruct.xmlName = resourceStruct.resName;
        xmlStruct.xmlPackageName = resourceStruct.resPackageName;
        xmlStruct.xmlPath = resourceStruct.resPath;
        xmlStruct.xmlVersion = resourceStruct.resVersion;

        if (string.IsNullOrEmpty(xmlStruct.xmlPath))
        {
            return xmlStruct;
        }

        XmlDocument xml = LoadXml(xmlStruct.xmlPackageName + "/" + xmlStruct.xmlPath);

        XmlNodeStruct nodeStruct = new XmlNodeStruct();
        XmlNode resourceNode = xml.SelectSingleNode(XMLStaticString._resources);
        nodeStruct.xmlNodeName = XMLStaticString._resourceFile;
        //加载XML的节点
        nodeStruct.xmlNode = resourceNode.SelectSingleNode(nodeStruct.xmlNodeName);
        //加载"ResourceFile"节点下的所有资源
        nodeStruct = LoadResourcesByXMLNode(nodeStruct);
        xmlStruct.xmlNodeStruct = nodeStruct;

        return xmlStruct;
    }

    //加载指定节点下的所有资源
    protected XmlNodeStruct LoadResourcesByXMLNode(XmlNodeStruct nodeStruct)
    {
        //如果没有默认结构的节点 则退出加载 返回空对象
        if (nodeStruct.xmlNode == null)
        {
            return new XmlNodeStruct();
        }

        //获取所有子节点
        XmlNodeList nodeList = nodeStruct.xmlNode.SelectNodes(XMLStaticString._item);
        //初始化节点资源
        nodeStruct.xmlResourceStructArray = new XmlResourceStruct[nodeList.Count];
        //每一个节点的元素
        XmlElement node = null;
        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeStruct.xmlResourceStructArray[i] = new XmlResourceStruct();
            //把当前节点转换成元素 然后从元素中取值
            node = (XmlElement)nodeList[i];

            nodeStruct.xmlResourceStructArray[i].resName = node.GetAttribute(XMLStaticString._name);
            nodeStruct.xmlResourceStructArray[i].resPackageName = node.GetAttribute(XMLStaticString._packagename);
            nodeStruct.xmlResourceStructArray[i].resPath = node.GetAttribute(XMLStaticString._path);
            nodeStruct.xmlResourceStructArray[i].resVersion = float.Parse(node.GetAttribute(XMLStaticString._version));
        }

        return nodeStruct;
    }

    //从指定XML中获取指定的XML节点
    protected XmlResourceStruct GetNodeStructByXMLStruct(XmlStruct xml, string resParentNodeName, string resName)
    {
        XmlNodeStruct parentNode = new XmlNodeStruct();
        XmlResourceStruct resStruct = new XmlResourceStruct();

        parentNode = xml.xmlNodeStruct;

        for (int i = 0; i < parentNode.xmlResourceStructArray.Length; i++)
        {
            if (resName.Equals(parentNode.xmlResourceStructArray[i].resName))
            {
                resStruct = parentNode.xmlResourceStructArray[i];
                break;
            }
        }
        return resStruct;
    }

    #endregion

    #region PublicFunction

    /// <summary>
    /// 获取指定名称的XML -- 配置在表中的Xml
    /// </summary>
    /// <param name="xmlName">XML文档名称</param>
    /// <returns>XmlDocument</returns>
    public XmlDocument GetXMLDocumentByName(string xmlName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_xmlXML, "ResourceFile", xmlName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("Xml资源::" + xmlName + "获取失败");

            return null;
        }

        XmlDocument xml = LoadXml(resStruct.resPath);

        return xml;
    }

    /// <summary>
    /// 获取指定名称的XML -- 不需要配置的内部Xml
    /// </summary>
    /// <param name="xmlPath">XML文档路经</param>
    /// <returns></returns>
    public XmlDocument GetXMLDocumentByPath(string xmlPath)
    {
        XmlDocument xml = null;

        xml = LoadXml(xmlPath);

        return xml;
    }

    /// <summary>
    /// 获取音频
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>AudioClip音频</returns>
    public AudioClip GetAudioClipResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_audioXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("音频资源::" + resName + "获取失败");

            return null;
        }

        AudioClip audio = (AudioClip)Resources.Load(resStruct.resPath);

        return audio;
    }
    /// <summary>
    /// 获取贴图
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Texture贴图</returns>
    public Texture GetTextureResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_textureXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("Texture贴图资源::" + resName + "获取失败");

            return null;
        }

        Texture texture = (Texture)Resources.Load(resStruct.resPath);

        return texture;
    }
    /// <summary>
    /// 获取精灵贴图
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Sprite贴图</returns>
    public Sprite GetSpriteResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_textureXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("Sprite贴图资源::" + resName + "获取失败");

            return null;
        }

        Sprite sprite = Resources.Load<Sprite>(resStruct.resPath);

        return sprite;
    }
    /// <summary>
    /// 获取精灵贴图数组
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Sprite贴图数组</returns>
    public Sprite[] GetSpriteArrayResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_textureXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("Sprite贴图数组资源::" + resName + "获取失败");

            return null;
        }

        Sprite[] spriteArray = Resources.LoadAll<Sprite>(resStruct.resPath);

        return spriteArray;
    }
    /// <summary>
    /// 获取预制
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>GameObject预制 未实例化</returns>
    public GameObject GetPrefabResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_prefabXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("预制资源::" + resName + "获取失败");

            return null;
        }
        //Debug.Log(resStruct.resPath);
        GameObject obj = (GameObject)Resources.Load(resStruct.resPath);

        return obj;
    }
    /// <summary>
    /// 获取实例化对应预制后的对象
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>GameObject 实例化后的对象</returns>
    public GameObject GetIniPrefabResourceByName(string resName)
    {
        GameObject obj = (GameObject)GetPrefabResourceByName(resName);

        obj = (GameObject)Instantiate(obj);

        return obj;
    }
    /// <summary>
    /// 获取材质
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Material 材质球</returns>
    public Material GetMaterialResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_materialXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("材质资源::" + resName + "获取失败");

            return null;
        }
        Material material = (Material)Resources.Load(resStruct.resPath);

        return material;
    }

    /// <summary>
    /// 获取克隆材质
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>克隆材质球</returns>
    public Material GetCopyMaterialResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_materialXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("材质资源::" + resName + "获取失败");

            return null;
        }
        Material material = new Material((Material)Resources.Load(resStruct.resPath));

        return material;
    }

    /// <summary>
    /// 获取动画
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>动画控制器</returns>
    public RuntimeAnimatorController GetAnimatorResourceByName(string resName)
    {
        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_animatorXML, "ResourceFile", resName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("动画资源::" + resName + "获取失败");

            return null;
        }

        RuntimeAnimatorController obj = Resources.Load<RuntimeAnimatorController>("New Animator Controller");

        return obj;
    }

    /// <summary>
    /// 获取Json文本 Resources本地读取
    /// </summary>
    /// <param name="jsonName">json文件名</param>
    /// <returns></returns>
    public TextAsset GetJsonTextByName(string jsonName)
    {

        XmlResourceStruct resStruct = GetNodeStructByXMLStruct(m_jsonXML, "ResourceFile", jsonName);

        if (resStruct.resPath == null)
        {
            Debug.LogError("Json资源::" + jsonName + "获取失败");

            return null;
        }

        TextAsset myJson = (TextAsset)Resources.Load(resStruct.resPath);
        return myJson;
    }
    #endregion
}

