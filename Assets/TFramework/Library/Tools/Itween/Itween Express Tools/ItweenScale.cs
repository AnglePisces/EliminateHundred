using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// iTween缩放物体的类
/// 
/// </summary>

public class ItweenScale : ItweenBase
{
    //移动所需的时间
    public float m_scaleEndTime = 0.0f;

    //移动的初始位置
    public Vector3 m_scaleStartScale = new Vector3(0.0f, 0.0f, 0.0f);

    //移动到指定坐标
    public Vector3 m_scaleToScale = new Vector3(0.0f, 0.0f, 0.0f);

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Initialization()
    {
        base.Initialization();

        m_itweenObjectTransform.localScale = m_scaleStartScale;

        if (m_autoPlayItweenFlag)
        {
            Invoke("PlayItween", m_delayTime);
        }

        if (m_stopItweenFlag)
        {
            Invoke("StopItween", m_stopItweenIntervalTime);
        }

        if (m_autoHideItweenFlag)
        {
            Invoke("HideItweenObject", m_autoHideItweenIntervalTime);
        }
    }

    //播放itween
    public override void PlayItween()
    {
        base.PlayItween();
        iTween.ScaleTo(m_itweenObject, iTween.Hash("scale", m_scaleToScale, "time", m_scaleEndTime, "easytype", m_playEaseType.ToString(), "looptype", m_playLoopType.ToString(), "delay", m_loopDelayTime));
    }

    //停止itween
    public override void StopItween()
    {
        base.StopItween();
    }

    //隐藏Itween对象
    protected override void HideItweenObject()
    {
        base.HideItweenObject();
    }
}
