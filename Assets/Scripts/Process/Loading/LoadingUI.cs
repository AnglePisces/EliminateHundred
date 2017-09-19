using TFramework.Base;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : TMonoBehaviour
{
    //进度条
    protected Slider _loadSlider;
    //当前的加载进度
    protected float _loadProgress;


    public void Initialization()
    {
        FindChild();

        //开始加载 临时UI层效果
        InvokeRepeating("StartLoading", 0.3f, 0.1f);
    }

    protected void FindChild()
    {
        this._loadSlider = this.gameObject.transform.FindChild("loadSlider").GetComponent<Slider>();
    }

    //添加事件消息
    protected void AddEvent()
    {
       
    }

    protected void StartLoading()
    {
        _loadProgress += Random.Range(0f, 0.1f);
        if (_loadProgress >=1)
        {
            _loadProgress = 1;
            CancelInvoke("StartLoading");
        }
        SetLoadSlider(_loadProgress);
    }

    //设置进度条的值
    protected void SetLoadSlider(float v)
    {
        this._loadSlider.value = v;
    }


}
