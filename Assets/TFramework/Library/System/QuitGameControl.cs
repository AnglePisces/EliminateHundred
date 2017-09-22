using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

/// <summary>
/// 
/// 退出游戏管理
/// 
/// </summary>

public class QuitGameControl : TMonoSingleton<QuitGameControl>
{

    /// <summary>
    /// 是否进入退出流程
    /// </summary>
    static public bool IsQuitGame = false;

    public override void Initialization()
    {

    }

    //初始化
    public override void Initialization(GameObject parentOBJ)
    {
        base.Initialization(parentOBJ);
    }

    void OnApplicationQuit()
    {
        TLogger.Quit();
        Loom.AbortRunningThreads();
    }

}
