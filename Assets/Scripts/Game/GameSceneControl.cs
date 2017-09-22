using UnityEngine;
using System.Collections;
using TFramework.Base;

/// <summary>
///
///游戏场景控制
/// 
/// 1 一局游戏的总入口
/// 2 初始化一局游戏所需要的各种单例、数据、对象
/// 
/// </summary>

namespace Game
{
    public class GameSceneControl : TMonoSingleton<GameSceneControl>
    {

        private GameSceneControl()
        {
        }

        /// <summary>
        /// 游戏是否开始 目前用布尔
        /// 后面会有多种游戏状态 可能会换成枚举
        /// </summary>
        public bool _isStart;

        //初始化
        public override void Initialization(GameObject obj, bool beChild)
        {
            _isStart = false;
            base.Initialization(obj, beChild);

            //创建地图管理器单例
            GameMapControl.Instance.Initialization(this.gameObject, true);
            //创建角色管理器单例
            PlayerManager.Instance.Initialization(this.gameObject, true);

            _isStart = true;
        }



        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}