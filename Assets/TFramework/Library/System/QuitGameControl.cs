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

    //初始化
    public override void Initialization(GameObject parentOBJ, bool beChild)
    {
        base.Initialization(parentOBJ, beChild);
    }

    void OnApplicationQuit()
    {
        TLogger.Quit();
        Loom.AbortRunningThreads();
    }

}
