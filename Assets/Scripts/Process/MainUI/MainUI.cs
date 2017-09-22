using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

/// <summary>
///
/// 主界面UI
///
/// </summary>
public class MainUI : TUIMonoBehaviour
{

    private MainUI() { }

    public override void Initialization(GameObject obj, bool beChild)
    {
        //目前主界面不做任何事情 直接开始游戏
        EHGameManager.Instance.StartGame();
    }

    protected override void FindChild()
    {

    }

}