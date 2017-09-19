using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// iTween�ƶ��������
/// 
/// </summary>

public class ItweenMove : ItweenBase
{
    //�ƶ������ʱ��
    public float m_moveEndTime = 0.0f;

    //�ֲ���ǩ
    public bool m_local = false;

    //�ƶ��ĳ�ʼλ��
    public Vector3 m_moveStartPosition = new Vector3(0.0f, 0.0f, 0.0f);

    //�ƶ���ָ������
    public Vector3 m_moveToPosition = new Vector3(0.0f, 0.0f, 0.0f);

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ItweenOverCallBackDelegateFunction = null;
    }

    public override void Initialization()
    {
        base.Initialization();

        m_itweenObjectTransform = m_itweenObject.transform;
        //m_itweenObjectTransform.position = m_moveStartPosition;

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

    //����itween
    public override void PlayItween()
    {
        base.PlayItween();
        iTween.MoveTo(m_itweenObject, iTween.Hash("position", m_moveToPosition, "islocal", m_local, "time", m_moveEndTime, "easetype", m_playEaseType.ToString(), "looptype", m_playLoopType.ToString(), "delay", m_loopDelayTime,"oncomplete", "ItweenOverCallBack"));
    }

    //�����ص�����
    public delegate void ItweenOverCallBackDelegate();
    public ItweenOverCallBackDelegate ItweenOverCallBackDelegateFunction;

    protected void ItweenOverCallBack()
    {
        if(ItweenOverCallBackDelegateFunction != null)
        {
            ItweenOverCallBackDelegateFunction();
        }
    }

    //ֹͣitween
    public override void StopItween()
    {
        base.StopItween();
    }

    //����Itween����
    protected override void HideItweenObject()
    {
        base.HideItweenObject();
    }

}
