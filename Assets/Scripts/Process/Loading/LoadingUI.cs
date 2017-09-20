using EHEvent;
using EventHandleModel;
using TFramework.Base;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// 加载界面
/// 
/// </summary>

public class LoadingUI : TUIMonoBehaviour
{
    //进度条
    protected Slider _loadSlider;
    //当前的加载进度
    protected float _loadProgress;

    public override void Initialization()
    {
        FindChild();

        //开始加载 临时UI层效果
        InvokeRepeating("StartLoading", 0.3f, 0.1f);
    }

    protected override void FindChild()
    {
        _loadSlider = transform.Find("loadSlider").GetComponent<Slider>();
    }

    //开始加载
    protected void StartLoading()
    {
        _loadProgress += Random.Range(0f, 0.1f);
        if (_loadProgress >= 1)
        {
            _loadProgress = 1;
            FinishLoading();
        }
        SetLoadSlider(_loadProgress);
    }

    //设置进度条的值
    protected void SetLoadSlider(float v)
    {
        this._loadSlider.value = v;
    }

    //加载完成
    protected void FinishLoading()
    {
        CancelInvoke("StartLoading");

        EHLoadingEvent evt = new EHLoadingEvent();
        evt._state = true;
        EventCenter.Instance.TriggerEvent(evt);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _loadSlider = null;

    }
}
