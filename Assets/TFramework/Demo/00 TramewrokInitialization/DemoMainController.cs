using UnityEngine;
using System.Collections;
using TFramework.System;

/// <summary>
/// 
/// 测试主模块的初始化 
/// 
/// 程序的唯一入口是MainController
/// 
/// </summary>

public class DemoMainController : MainControl
{

    //这个是项目的主入口，所有的主要模块在这里进行初始化准备。
    //一般来说，主要模块都可以使用单例模式，框架中有单例基类可以继承。
    //项目是否开始进行下一步，由GameManager统一调度，整个的切换，由Process进行管理，可以切换场镜。
    //模块之间尽量保证低耦合度，如果有耦合，则主要关注模块初始化的顺序，防止出错。

    protected override void Awake()
    {
        base.Awake();

        DemoGameManager.Instance.Initialization(this.gameObject, true);
    }

}
