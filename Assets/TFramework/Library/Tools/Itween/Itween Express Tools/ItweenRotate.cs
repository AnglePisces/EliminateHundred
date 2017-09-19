using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// iTween旋转物体的类
/// 
/// </summary>

public class ItweenRotate : ItweenBase
{
    //旋转所需的时间
    public float m_rotateEndTime = 0.0f;

    //旋转的初始位置
    public Vector3 m_rotateStartAngles = new Vector3(0.0f, 0.0f, 0.0f);

    //旋转到指定坐标
    public Vector3 m_rotateToAngles = new Vector3(0.0f, 0.0f, 0.0f);

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

        m_itweenObjectTransform.eulerAngles = m_rotateStartAngles;

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
        iTween.RotateTo(m_itweenObject, iTween.Hash("rotation", m_rotateToAngles, "time", m_rotateEndTime, "easytype", m_playEaseType.ToString(), "looptype", m_playLoopType.ToString(), "delay", m_loopDelayTime));
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
