using UnityEngine;
using System.Collections;
using Game.PlayerControlSpace;
using TFramework.Base;

/// <summary>
///
///角色管理器
///
/// 1 生成玩家角色
/// 2 生成其它同局玩家
/// 
/// </summary>

namespace Game
{
    public class PlayerManager : TMonoSingleton<PlayerManager>
    {

        private PlayerManager() { }

        /// <summary>
        /// 主玩家 -- 当前玩家自己
        /// </summary>
        protected PlayerControl _mainPlayer;

        //初始化
        public override void Initialization(GameObject obj, bool beChild)
        {
            base.Initialization(obj, beChild);

            InitMainPlayer();
        }

        /// <summary>
        /// 初始化玩家角色
        /// </summary>
        protected void InitMainPlayer()
        {
            GameObject obj = ResourcesManager.Instance.GetIniPrefabResourceByName("Player");
            _mainPlayer = obj.AddComponent<PlayerControl>();
            obj.name = "PlayerControl";
            _mainPlayer.Initialization(null, false);
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
