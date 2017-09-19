using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Itween 控制类
/// 
/// </summary>

public class ItweenBaseController : MonoBehaviour
{
    [HideInInspector]
    //各各itween组件
    public ItweenAlpha m_itweenAlphaComponent = null;
    public ItweenMove m_itweenMoveComponent = null;
    public ItweenScale m_itweenScaleComponent = null;
    public ItweenRotate m_itweenRotateComponent = null;

    //初始化
    public virtual void Initialization()
    {
        m_itweenAlphaComponent = this.gameObject.GetComponent<ItweenAlpha>();
        m_itweenMoveComponent = this.gameObject.GetComponent<ItweenMove>();
        m_itweenScaleComponent = this.gameObject.GetComponent<ItweenScale>();
        m_itweenRotateComponent = this.gameObject.GetComponent<ItweenRotate>();

        if (m_itweenAlphaComponent != null)
        {
            m_itweenAlphaComponent.Initialization();
        }

        if (m_itweenMoveComponent != null)
        {
            m_itweenMoveComponent.Initialization();
        }

        if (m_itweenScaleComponent != null)
        {
            m_itweenScaleComponent.Initialization();
        }

        if (m_itweenRotateComponent != null)
        {
            m_itweenRotateComponent.Initialization();
        }
    }

    //重新播放
    public virtual void RePlay()
    {
        if (m_itweenAlphaComponent != null)
        {
            m_itweenAlphaComponent.StopItween();
            m_itweenAlphaComponent.PlayItween();
        }

        if (m_itweenMoveComponent != null)
        {
            m_itweenMoveComponent.StopItween();
            m_itweenMoveComponent.PlayItween();
        }

        if (m_itweenScaleComponent != null)
        {
            m_itweenScaleComponent.StopItween();
            m_itweenScaleComponent.PlayItween();
        }

        if (m_itweenRotateComponent != null)
        {
            m_itweenRotateComponent.StopItween();
            m_itweenRotateComponent.PlayItween();
        }
    }

    //停止播放
    public virtual void StopPlay()
    {
        if (m_itweenAlphaComponent != null)
        {
            m_itweenAlphaComponent.StopItween();
        }

        if (m_itweenMoveComponent != null)
        {
            m_itweenMoveComponent.StopItween();
        }

        if (m_itweenScaleComponent != null)
        {
            m_itweenScaleComponent.StopItween();
        }

        if (m_itweenRotateComponent != null)
        {
            m_itweenRotateComponent.StopItween();
        }
    }
}
