using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// Itween控制透明度的类
/// 
/// </summary>

public class ItweenAlpha : ItweenBase
{
    //要变化的材质球对象
    private Renderer m_itweenObjectRenderer = null;

    //初始颜色
    public Color m_colorStartValue = new Color();
    //材质颜色参数的变量
    public string m_colorRendervalue = "";

    //透明度
    public float m_alphaEndValue = 0.0f;

    //透明变化所需时间
    public float m_alphaEndTime = 0.0f;

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

        m_itweenObjectRenderer = m_itweenObject.gameObject.GetComponent<Renderer>();
        m_itweenObjectRenderer.material.SetColor(m_colorRendervalue, m_colorStartValue);

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
        iTween.FadeTo(m_itweenObject, iTween.Hash("alpha", m_alphaEndValue, "time", m_alphaEndTime, "easytype", m_playEaseType.ToString(), "looptype", m_playLoopType.ToString(), "delay", m_loopDelayTime));
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
