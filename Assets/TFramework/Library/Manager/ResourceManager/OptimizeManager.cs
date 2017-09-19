using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TFramework.Base;

/// <summary>
/// 
/// 游戏优化控制
/// 
/// 依赖于资源管理器
/// 从资源管理器中获取相应需要优化的对象
/// 
/// </summary>

public class OptimizeManager : MonoBehaviour
{
    #region Variable

    //优化DrawCall对象
    protected GameObject m_optimizeDrawCallObject = null;
    //DrawCall对象的结构 里面存放各种需要优化的对象词典
    public struct DrawCallObjectStruct
    {
        public IDictionary<string, Material> materialDictionary; //材质球缓存
        public IDictionary<string, Sprite> spriteIDictionary; //精灵贴图缓存
        public IDictionary<string, Sprite[]> spriteArrayIDictionary; //精灵贴图数组缓存
    }
    //DrawCall对象的结构对象
    protected DrawCallObjectStruct m_drawCallObjectStruct;

    #endregion

    #region Protected Function

    //初始化
    public void Initialization()
    {
        IniDrawCallObjectStruct();
    }

    //初始化 DrawCall对象的结构
    protected void IniDrawCallObjectStruct()
    {
        m_drawCallObjectStruct = new DrawCallObjectStruct();

        m_drawCallObjectStruct.materialDictionary = new Dictionary<string, Material>();
        m_drawCallObjectStruct.spriteIDictionary = new Dictionary<string, Sprite>();
        m_drawCallObjectStruct.spriteArrayIDictionary = new Dictionary<string, Sprite[]>();
    }

    #endregion

    #region Public Function

    //提供 材质球缓存词典 动态优化 
    //获取的时候 如果有 则直接给予 没有 则先缓存 再给予
    /// <summary>
    /// 获取优化的材质球
    /// </summary>
    /// <param name="name">获取的资源名</param>
    /// <returns>Material材质球</returns>
    public Material GetOptimizeMaterial(string name)
    {
        Material m;
        m_drawCallObjectStruct.materialDictionary.TryGetValue(name, out m);

        //如果没有 则先缓存
        if (m == null)
        {
            m = ResourcesManager.Instance.GetMaterialResourceByName(name);

            m_drawCallObjectStruct.materialDictionary.Add(name, m);

            m = m_drawCallObjectStruct.materialDictionary[name];

        }

        return m;
    }
    //解除 材质球缓存词典 动态优化 
    /// <summary>
    /// 解除 材质球缓存词典 动态优化 
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeMaterial(string name)
    {
        Material m;
        m_drawCallObjectStruct.materialDictionary.TryGetValue(name, out m);

        if (m != null)
        {
            m_drawCallObjectStruct.materialDictionary.Remove(name);

            m.mainTexture = null;
            Destroy(m);
        }
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
        Sprite s;
        m_drawCallObjectStruct.spriteIDictionary.TryGetValue(name, out s);

        //如果没有 则先缓存
        if (s == null)
        {
            s = ResourcesManager.Instance.GetSpriteResourceByName(name);

            m_drawCallObjectStruct.spriteIDictionary.Add(name, s);

            s = m_drawCallObjectStruct.spriteIDictionary[name];

        }

        return s;
    }

    //删除 精灵贴图缓存词典 动态优化
    /// <summary>
    /// 删除 精灵贴图缓存词典 动态优化
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeSprite(string name)
    {
        Sprite s;
        m_drawCallObjectStruct.spriteIDictionary.TryGetValue(name, out s);

        if (s != null)
        {
            m_drawCallObjectStruct.spriteIDictionary.Remove(name);

            Resources.UnloadAsset(s);
            s = null;
        }
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
        Sprite[] sArray = null;
        m_drawCallObjectStruct.spriteArrayIDictionary.TryGetValue(name, out sArray);

        //如果没有 则先缓存
        if (sArray == null)
        {
            sArray = ResourcesManager.Instance.GetSpriteArrayResourceByName(name);

            m_drawCallObjectStruct.spriteArrayIDictionary.Add(name, sArray);

            sArray = m_drawCallObjectStruct.spriteArrayIDictionary[name];

        }

        return sArray;
    }

    // 删除 精灵贴图数组缓存词典 动态优化
    /// <summary>
    /// 删除 精灵贴图数组缓存词典 动态优化 
    /// </summary>
    /// <param name="name"></param>
    public void RemoveOptimizeSpriteArray(string name)
    {
        Sprite[] sArray = null;
        m_drawCallObjectStruct.spriteArrayIDictionary.TryGetValue(name, out sArray);

        if (sArray != null)
        {
            m_drawCallObjectStruct.spriteArrayIDictionary.Remove(name);

            sArray = null;
        }
    }

    //根据提供的Texture名称 帮助生成材质球缓存
    /// <summary>
    /// 根据提供的Texture名称 帮助生成材质球缓存
    /// </summary>
    /// <param name="name">获取材质球的贴图名</param>
    /// <returns>获取的材质球</returns>
    public Material GetCreateMaterialByName(string name)
    {
        Material m = null;

        m_drawCallObjectStruct.materialDictionary.TryGetValue("M" + name, out m);

        //如果没有 则先缓存
        if (m == null)
        {
            m = ResourcesManager.Instance.GetCopyMaterialResourceByName("TextureDemo_T.mat");

            m.mainTexture = ResourcesManager.Instance.GetTextureResourceByName(name);

            m_drawCallObjectStruct.materialDictionary.Add("M" + name, m);
        }

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
        Material[] mArray = null;

        Sprite[] sArray = null;

        sArray = GetOptimizeSpriteArray(name);

        mArray = new Material[sArray.Length];

        Texture2D tex;
        for (int i = 0; i < mArray.Length; i++)
        {
            m_drawCallObjectStruct.materialDictionary.TryGetValue("MA" + name + i, out mArray[i]);

            if (mArray[i] == null)
            {
                mArray[i] = ResourcesManager.Instance.GetCopyMaterialResourceByName("TextureDemo_T.mat");

                tex = new Texture2D((int)sArray[i].rect.width, (int)sArray[i].rect.height, sArray[i].texture.format, false);
                tex.SetPixels(sArray[i].texture.GetPixels((int)sArray[i].rect.xMin, (int)sArray[i].rect.yMin, (int)sArray[i].rect.width, (int)sArray[i].rect.height));
                tex.Apply();

                mArray[i].mainTexture = tex;

                m_drawCallObjectStruct.materialDictionary.Add("MA" + name + i, mArray[i]);
            }

        }

        return mArray;
    }

    //清理所有缓存


    #endregion
}
