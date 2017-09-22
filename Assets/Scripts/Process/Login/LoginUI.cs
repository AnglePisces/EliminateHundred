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
    #region 初始界面
    //初始界面
    protected Transform _firstUI;
    //帐号登录按钮
    protected Button _zhLogin;
    //帐号注册按钮
    protected Button _zhRegister;
    //微信登录按钮
    protected Button _wxLogin;
    #endregion
    #region 登录界面
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
    #endregion
    #region 注册界面
    //注册界面
    protected Transform _registerUI;
    //帐号框
    protected InputField _registerID;
    //密码框
    protected InputField _registerPwd;
    //注册按钮
    protected Button _doRegister;
    //返回按钮
    protected Button _registerBack;
    #endregion
    public override void Initialization()
    {
        FindChild();

        AddListener();

        BackFirstUI();
    }

    public override void Initialization(GameObject parentOBJ)
    {
        
    }

    protected override void FindChild()
    {

        _firstUI = transform.Find("firstUI").transform;
        _zhLogin = _firstUI.transform.Find("zhLogin").GetComponent<Button>();
        _zhLogin.onClick.AddListener(ShowLoginUI);
        _zhRegister = _firstUI.transform.Find("zhRegister").GetComponent<Button>();
        _zhRegister.onClick.AddListener(ShowRegisterUI);
        _wxLogin = _firstUI.transform.Find("wxLogin").GetComponent<Button>();

        _loginUI = transform.Find("login").transform;
        _loginID = _loginUI.transform.Find("loginID").GetComponent<InputField>();
        _loginPwd = _loginUI.transform.Find("loginPwd").GetComponent<InputField>();
        _doLogin = _loginUI.transform.Find("doLogin").GetComponent<Button>();
        _doLogin.onClick.AddListener(DoIDLogin);
        _loginBack = _loginUI.transform.Find("loginBack").GetComponent<Button>();
        _loginBack.onClick.AddListener(BackFirstUI);

        _registerUI = transform.Find("register").transform;
        _registerID = _registerUI.transform.Find("registerID").GetComponent<InputField>();
        _registerPwd = _registerUI.transform.Find("registerPwd").GetComponent<InputField>();
        _doRegister = _registerUI.transform.Find("doRegister").GetComponent<Button>();
        _doRegister.onClick.AddListener(DoRegister);
        _registerBack = _registerUI.transform.Find("registerBack").GetComponent<Button>();
        _registerBack.onClick.AddListener(BackFirstUI);
    }

    //挂事件监听
    protected void AddListener()
    {
        EventCenter.Instance.AddEventListener((int)EHGameProcessEventID.Process_Login_Event, IDLoginResult);
        EventCenter.Instance.AddEventListener((int)EHGameProcessEventID.Process_Register_Event, RegisterResult);
    }

    //卸载监听
    protected void RemoveListener()
    {
        EventCenter.Instance.RemoveEventListener((int)EHGameProcessEventID.Process_Login_Event, IDLoginResult);
        EventCenter.Instance.RemoveEventListener((int)EHGameProcessEventID.Process_Register_Event, RegisterResult);
    }

    //返回到初始界面
    protected void BackFirstUI()
    {
        _loginUI.gameObject.SetActive(false);
        _registerUI.gameObject.SetActive(false);
        _firstUI.gameObject.SetActive(true);
    }

    #region 帐号登录

    //打开登录界面
    protected void ShowLoginUI()
    {
        _firstUI.gameObject.SetActive(false);
        _loginID.text = string.Empty;
        _loginPwd.text = string.Empty;
        _loginUI.gameObject.SetActive(true);
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

    #endregion

    #region 帐号注册

    //显示注册界面
    protected void ShowRegisterUI()
    {
        _firstUI.gameObject.SetActive(false);
        _registerID.text = string.Empty;
        _registerPwd.text = string.Empty;
        _registerUI.gameObject.SetActive(true);
    }

    //开始注册
    protected void DoRegister()
    {

        string id = _registerID.text;
        string pwd = _registerPwd.text;

        if (!CheckRegister(id, pwd))
        {
            return;
        }

        Register(id, pwd);

    }

    //注册字符串检查
    protected bool CheckRegister(string id, string pwd)
    {
        return true;
    }

    //注册
    protected void Register(string id, string pwd)
    {
        //注册成功后 从回调信息里 把帐号和密码取出进行登录
        if (true)
        {
            EHRegisterEvent evt = new EHRegisterEvent();
            evt._state = true;
            evt._log = "注册成功";
            evt._id = "";
            evt._pwd = "";
            EventCenter.Instance.TriggerEvent(evt);
        }
    }

    //注册回调
    protected void RegisterResult(IEvent evt)
    {
        EHRegisterEvent e = evt as EHRegisterEvent;

        if (e._state)
        {
            //注册成功 直接用注册成功的帐号进行登录
            IDLogin(e._id, e._pwd);
        }
        else
        {
            //注册失败
        }
    }

    #endregion

    protected override void OnDestroy()
    {
        base.OnDestroy();

        RemoveListener();
    }
}
