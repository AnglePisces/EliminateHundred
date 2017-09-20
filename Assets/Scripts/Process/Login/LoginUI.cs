using EHEvent;
using EventHandleModel;
using System;
using TFramework.Base;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// 登录界面
/// 
/// </summary>

public class LoginUI : TUIMonoBehaviour
{
    //初始界面
    protected Transform _firstUI;
    //帐号登录按钮
    protected Button _zhLogin;
    //帐号注册按钮
    protected Button _zhRegister;
    //微信登录按钮
    protected Button _wxLogin;

    //登录界面
    protected Transform _loginUI;
    //帐号框
    protected InputField _loginID;
    //密码框
    protected InputField _loginPwd;
    //登录按钮
    protected Button _doLogin;
    //返回按钮
    protected Button _loginBack;

    public override void Initialization()
    {
        FindChild();

        AddListener();
    }

    protected override void FindChild()
    {

        _firstUI = transform.Find("firstUI").transform;
        _zhLogin = _firstUI.transform.Find("zhLogin").GetComponent<Button>();
        _zhLogin.onClick.AddListener(OpenLoginUI);
        _zhRegister = _firstUI.transform.Find("zhRegister").GetComponent<Button>();
        _wxLogin = _firstUI.transform.Find("wxLogin").GetComponent<Button>();

        _loginUI = transform.Find("login").transform;
        _loginID = _loginUI.transform.Find("loginID").GetComponent<InputField>();
        _loginPwd = _loginUI.transform.Find("loginPwd").GetComponent<InputField>();
        _doLogin = _loginUI.transform.Find("doLogin").GetComponent<Button>();
        _doLogin.onClick.AddListener(DoIDLogin);
        _loginBack = _loginUI.transform.Find("loginBack").GetComponent<Button>();
        _loginBack.onClick.AddListener(BackFirstUI);
    }

    //挂事件监听
    protected void AddListener()
    {
        EventCenter.Instance.AddEventListener((int)EHGameProcessEventID.Process_Login_Event, IDLoginResult);
    }

    //卸载监听
    protected void RemoveListener()
    {
        EventCenter.Instance.RemoveEventListener((int)EHGameProcessEventID.Process_Login_Event, IDLoginResult);
    }

    //打开登录界面
    protected void OpenLoginUI()
    {
        _firstUI.gameObject.SetActive(false);
        _loginID.text = string.Empty;
        _loginPwd.text = string.Empty;
        _loginUI.gameObject.SetActive(true);
    }

    //返回到初始界面
    protected void BackFirstUI()
    {
        _loginUI.gameObject.SetActive(false);
        _firstUI.gameObject.SetActive(true);
    }

    //开始帐号登录
    protected void DoIDLogin()
    {
        string id = _loginID.text;
        string pwd = _loginPwd.text;

        if (!CheckIDLogin(id, pwd))
        {
            return;
        }

        IDLogin(id, pwd);

    }

    //帐号字符串登录验证
    protected bool CheckIDLogin(string id, string pwd)
    {
        return true;
    }

    //帐号登录
    protected void IDLogin(string id, string pwd)
    {
        //经过服务器验证登录进行登录

        //登录成功
        if (true)
        {
            EHLoginEvent evt = new EHLoginEvent();
            evt._state = true;
            evt._log = "登录成功";
            EventCenter.Instance.TriggerEvent(evt);
        }

    }

    //帐号登录回调
    protected void IDLoginResult(IEvent evt)
    {
        EHLoginEvent e = evt as EHLoginEvent;

        if (e._state)
        {
            //登录成功
            EHGameManager.Instance.IntoMainUI();
        }
        else
        {
            //登录失败
        }

    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

        RemoveListener();
    }
}
