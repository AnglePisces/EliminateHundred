using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Itween 基类
/// 
/// </summary>

public class ItweenBase : MonoBehaviour
{
    /// <summary>
    /// 变化自身的标签
    /// </summary>
    public bool m_itweenSelfFlag = false;

    /// <summary>
    /// 要变化的对象
    /// </summary>
    public GameObject m_itweenObject = null;
    protected Transform m_itweenObjectTransform = null;

    /// <summary>
    /// 自动播放的标签
    /// </summary>
    public bool m_autoPlayItweenFlag = false;

    /// <summary>
    /// 移动的循环方式选项
    /// </summary>
    public iTween.LoopType m_playLoopType = iTween.LoopType.none;

    /// <summary>
    /// 播放曲线
    /// </summary>
    public iTween.EaseType m_playEaseType = iTween.EaseType.linear;

    /// <summary>
    /// 播放等待时间
    /// </summary>
    public float m_delayTime = 0.0f;

    /// <summary>
    /// 循环间隔时间
    /// </summary>
    public float m_loopDelayTime = 0.0f;

    /// <summary>
    /// 延迟停止Itween的标签
    /// </summary>
    public bool m_stopItweenFlag = false;
    /// <summary>
    /// 延迟停止Itween的时间
    /// </summary>
    public float m_stopItweenIntervalTime = 0.0f;

    /// <summary>
    /// 自动隐藏变化目标的标签
    /// </summary>
    public bool m_autoHideItweenFlag = false;
    /// <summary>
    /// 自动隐藏变化目标的时间
    /// </summary>
    public float m_autoHideItweenIntervalTime = 0.0f;

    /// <summary>
    /// 激活回调
    /// </summary>
    protected virtual void OnEnable()
    {

    }
    /// <summary>
    /// 隐藏回调
    /// </summary>
    protected virtual void OnDisable()
    {
        StopItween();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public virtual void Initialization()
    {
        if (m_itweenSelfFlag)
        {
            m_itweenObject = this.gameObject;
        }
        m_itweenObjectTransform = m_itweenObject.transform;
    }

    /// <summary>
    /// 播放itween
    /// </summary>
    public virtual void PlayItween()
    {
        
    }

    /// <summary>
    /// 停止itween
    /// </summary>
    public virtual void StopItween()
    {
        iTween.Stop(m_itweenObject);
    }

    /// <summary>
    /// 隐藏Itween对象
    /// </summary>
    protected virtual void HideItweenObject()
    {
        m_itweenObject.SetActive(false);
    }
}
