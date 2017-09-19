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
    //帐号登录按钮
    protected Button _zhLogin;
    //帐号注册按钮
    protected Button _zhRegister;
    //微信登录按钮
    protected Button _wxLogin;


    public override void Initialization()
    {
        FindChild();
    }

    protected override void FindChild()
    {
        _zhLogin = transform.Find("zhLogin").GetComponent<Button>();

        _zhRegister = transform.Find("zhRegister").GetComponent<Button>();

        _wxLogin = transform.Find("wxLogin").GetComponent<Button>();

    }



}
