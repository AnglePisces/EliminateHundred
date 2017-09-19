using UnityEngine;
using System.Collections;
using System.Xml;
using TFramework.Base;

/// <summary>
/// 
/// 资源总控制
/// 
/// </summary>

public class ResourcesManager : TMonoSingleton<ResourcesManager>
{
    public ResourcesManager() { }

    //XML配置资源管理 -- 本地
    protected XMLResourcesController m_xmlResourcesController = null;
    //网络资源 -- AssetBundle
    protected AssetBundleLoader m_netResourceController = null;
    //获取外部资源 -- 一般为PC端

    //资源性能优化
    protected OptimizeManager m_optimizeManager = null;

    //初始化
    public override void Initialization(GameObject parentOBJ)
    {
        base.Initialization(parentOBJ);

        IniEveryResourcesController();
    }

    //初始化各资源管理模块
    protected void IniEveryResourcesController()
    {

        m_netResourceController = new AssetBundleLoader();
        m_netResourceController.init();

        m_xmlResourcesController = this.gameObject.AddComponent<XMLResourcesController>();
        m_xmlResourcesController.Initialization();

        m_optimizeManager = this.gameObject.AddComponent<OptimizeManager>();
        m_optimizeManager.Initialization();

    }

    //资源模块重新初始化
    public void ReIni()
    {
        ClearnIni();
        IniEveryResourcesController();
    }

    //资源模块清理
    protected void ClearnIni()
    {
        if (m_netResourceController != null)
        {
            m_netResourceController = null;
        }

        if (m_xmlResourcesController != null)
        {
            Destroy(m_xmlResourcesController);
        }
    }

    #region Public Util Function

    /// <summary>
    /// 启动网络资源更新
    /// 
    /// 步骤:
    /// 1 将打包工具打包好的资源放在服务器端 需要更新的内容是一个配置文件服务器端读取
    /// 2 客户端向服务器端请求要更新的内容和更新服务器的地址
    /// 3 调用此函数进行更新
    /// 
    /// </summary>
    /// <param name="rString">需要更新的内容 -- 由打包工具生成 然后放到服务器端</param>
    /// <param name="sString">服务器资源更新地址 -- 也是由服务器端下发</param>
    public void StartUpdateAsset(string rString, string sString)
    {
        AssetBundleDownloader abDownloader = new AssetBundleDownloader(rString, sString);
        StartCoroutine(abDownloader.StartDownload());
    }

    #endregion

    #region Public Get Function

    /// <summary>
    /// 获取指定名称的XML
    /// </summary>
    /// <param name="xmlName">XML文档名称</param>
    /// <returns>XmlDocument</returns>
    public XmlDocument GetXMLDocumentByName(string xmlName)
    {
        XmlDocument xml = null;

        //网络获取
        xml = m_netResourceController.GetXMLDocumentByName(xmlName);

        if (xml != null)
        {
            return xml;
        }

        //网本地获取
        xml = m_xmlResourcesController.GetXMLDocumentByName(xmlName);

        return xml;
    }

    /// <summary>
    /// 获取指定路经名称的XML
    /// </summary>
    /// <param name="xmlPath">XML文档路经</param>
    /// <returns>XmlDocument</returns>
    public XmlDocument GetXMLDocumentByPath(string xmlPath)
    {
        string xmlName = xmlPath.Substring(xmlPath.LastIndexOf("/") + 1, xmlPath.Length - (xmlPath.LastIndexOf("/") + 1));

        XmlDocument xml = null;

        xml = m_netResourceController.GetXMLDocumentByName(xmlName);

        if (xml != null)
        {
            return xml;
        }

        xml = m_xmlResourcesController.GetXMLDocumentByPath(xmlPath);

        return xml;
    }

    /// <summary>
    /// 获取Json文本 Resources本地读取
    /// </summary>
    /// <param name="jsonName">json文件名</param>
    /// <returns>TextAsset</returns>
    public TextAsset GetJsonTextByName(string jsonName)
    {

        TextAsset ta = null;

        ta = m_netResourceController.GetJsonTextByName(jsonName);

        if (ta != null)
        {
            return ta;
        }

        ta = m_xmlResourcesController.GetJsonTextByName(jsonName);

        return ta;
    }

    /// <summary>
    /// 获取音频
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>AudioClip音频</returns>
    public AudioClip GetAudioClipResourceByName(string resName)
    {
        AudioClip audio = null;

        audio = m_netResourceController.GetAudioClipResourceByName(resName);

        if (audio != null)
        {
            return audio;
        }

        audio = m_xmlResourcesController.GetAudioClipResourceByName(resName);

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

        texture = m_netResourceController.GetTextureResourceByName(resName);

        if (texture != null)
        {
            return texture;
        }

        texture = m_xmlResourcesController.GetTextureResourceByName(resName);

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

        sprite = m_netResourceController.GetSpriteResourceByName(resName);

        if (sprite != null)
        {
            return sprite;
        }

        sprite = m_xmlResourcesController.GetSpriteResourceByName(resName);

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

        spriteArray = m_netResourceController.GetSpriteArrayResourceByName(resName);

        if (spriteArray != null)
        {
            return spriteArray;
        }

        spriteArray = m_xmlResourcesController.GetSpriteArrayResourceByName(resName);

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

        obj = m_netResourceController.GetPrefabResourceByName(resName);

        if (obj != null)
        {
            return obj;
        }

        obj = m_xmlResourcesController.GetPrefabResourceByName(resName);

        return obj;
    }
    /// <summary>
    /// 获取实例化对应预制后的对象
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>GameObject 实例化后的对象</returns>
    public GameObject GetIniPrefabResourceByName(string resName)
    {
        GameObject obj = null;

        obj = m_netResourceController.GetPrefabResourceByName(resName);

        if (obj != null)
        {
            obj = (GameObject)Instantiate(obj);

            return obj;
        }

        obj = m_xmlResourcesController.GetIniPrefabResourceByName(resName);

        return obj;
    }
    /// <summary>
    /// 获取材质
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>Material 材质球</returns>
    public Material GetMaterialResourceByName(string resName)
    {
        Material material = null;

        material = m_netResourceController.GetMaterialResourceByName(resName);

        if (material != null)
        {
            return material;
        }

        material = m_xmlResourcesController.GetMaterialResourceByName(resName);

        return material;
    }

    /// <summary>
    /// 获取克隆材质
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>克隆材质球</returns>
    public Material GetCopyMaterialResourceByName(string resName)
    {
        Material material = null;

        material = m_netResourceController.GetMaterialResourceByName(resName);

        if (material != null)
        {
            Material nMaterial = new Material(material);

            return nMaterial;
        }

        material = m_xmlResourcesController.GetCopyMaterialResourceByName(resName);

        return material;
    }

    /// <summary>
    /// 获取动画
    /// </summary>
    /// <param name="resName">资源名称</param>
    /// <returns>动画控制器</returns>
    public RuntimeAnimatorController GetAnimatorResourceByName(string resName)
    {
        RuntimeAnimatorController obj = m_xmlResourcesController.GetAnimatorResourceByName(resName);

        return obj;
    }

    #endregion

    #region Public GetOptimize Function

    //提供 材质球缓存词典 动态优化 
    //获取的时候 如果有 则直接给予 没有 则先缓存 再给予
    /// <summary>
    /// 获取优化的材质球
    /// </summary>
    /// <param name="name">获取的资源名</param>
    /// <returns>Material材质球</returns>
    public Material GetOptimizeMaterial(string name)
    {
        Material m = m_optimizeManager.GetOptimizeMaterial(name);

        return m;
    }
    //解除 材质球缓存词典 动态优化 
    /// <summary>
    /// 解除 材质球缓存词典 动态优化 
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeMaterial(string name)
    {
        m_optimizeManager.RemoveOptimizeMaterial(name);
    }

    //提供 精灵贴图缓存词典 动态优化 
    //获取的时候 如果有 则直接给予 没有 则先缓存 再给予
    /// <summary>
    /// 获取优化的精灵贴图
    /// </summary>
    /// <param name="name">获取的资源名</param>
    /// <returns>Sprite贴图</returns>
    public Sprite GetOptimizeSprite(string name)
    {
        Sprite s = m_optimizeManager.GetOptimizeSprite(name);

        return s;
    }

    //删除 精灵贴图缓存词典 动态优化
    /// <summary>
    /// 删除 精灵贴图缓存词典 动态优化
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeSprite(string name)
    {
        m_optimizeManager.RemoveOptimizeSprite(name);
    }

    //提供 精灵贴图数组缓存词典 动态优化 
    //获取的时候 如果有 则直接给予 没有 则先缓存 再给予
    /// <summary>
    /// 获取优化的精灵贴图数组
    /// </summary>
    /// <param name="name">获取的资源名</param>
    /// <returns>Sprite贴图数组</returns>
    public Sprite[] GetOptimizeSpriteArray(string name)
    {
        Sprite[] sArray = m_optimizeManager.GetOptimizeSpriteArray(name);

        return sArray;
    }

    // 删除 精灵贴图数组缓存词典 动态优化
    /// <summary>
    /// 删除 精灵贴图数组缓存词典 动态优化 
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeSpriteArray(string name)
    {
        m_optimizeManager.RemoveOptimizeSpriteArray(name);
    }

    //根据提供的Texture名称 帮助生成材质球缓存
    /// <summary>
    /// 根据提供的Texture名称 帮助生成材质球缓存
    /// </summary>
    /// <param name="name">获取材质球的贴图名</param>
    /// <returns>获取的材质球</returns>
    public Material GetCreateMaterialByName(string name)
    {
        Material m = m_optimizeManager.GetCreateMaterialByName(name);

        return m;

    }

    //根据提供的Sprite数组名称 获取生成材质球数组
    /// <summary>
    /// 根据提供的Sprite数组名称 获取生成材质球数组
    /// </summary>
    /// <param name="name">精灵图集名称</param>
    /// <returns>材质球数组</returns>
    public Material[] GetCreateMaterialArrayBySpriteName(string name)
    {
        Material[] mArray = m_optimizeManager.GetCreateMaterialArrayBySpriteName(name);

        return mArray;
    }

    #endregion
}
