using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TFramework.System
{
    /// <summary>
    /// 
    /// 程序的主入口 负责各基础模块的启动
    /// 
    /// </summary>
    public class MainControl : MonoBehaviour
    {

        //程序入口
        protected virtual void Awake()
        {
            //核心不能销毁
            DontDestroyOnLoad(this.gameObject);

            //线程模块
            Loom.Init();
            //日志模块
            TLogger.Instance.ToString();

            TLogger.Log("基础模块开始初始化.", "MainControl", "Awake - IniMainModules");
            IniMainModules();
            TLogger.Log("基础模块初始化完成.", "MainControl", "Awake - IniMainModules");

            TLogger.Log("边缘模块开始初始化.", "MainControl", "Awake - IniMarginalModules");
            IniMarginalModules();
            TLogger.Log("边缘模块初始化完成.", "MainControl", "Awake - IniMarginalModules");

            TLogger.Log("游戏初始化完成.", "MainControl", "Awake");
        }

        //初始化主要模块
        /// <summary>
        /// 初始化主要模块
        /// </summary>
        protected virtual void IniMainModules()
        {
            //游戏退出模块
            QuitGameControl.Instance.Initialization(this.gameObject, true);
            //资源管理器
            ResourcesManager.Instance.Initialization(this.gameObject, true);
            //音频播放管理
            AudioManager.Instance.Initialization(this.gameObject, true);
        }

        //初始化边缘模块
        /// <summary>
        /// 初始化边缘模块
        /// </summary>
        protected virtual void IniMarginalModules()
        {

        }
    }

}


