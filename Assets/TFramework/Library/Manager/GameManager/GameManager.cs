using UnityEngine;
using System.Collections;
using TFramework.Base;

/// <summary>
///
/// 游戏管理器
/// 
/// 1 统一调度游戏的进程 
/// 2 所有模块中最后一个初始化的模块 初始化完成，开始运作项目
///
/// </summary>
public class GameManager<T> : TMonoSingleton<T> where T : GameManager<T>
{

	public GameManager() { }

	//初始化
    public override void Initialization(GameObject parentOBJ)
    {
        base.Initialization(parentOBJ);
    }

    //继承重写销毁函数 里面有基类销毁管理
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

}