using UnityEngine;
using System.Collections;
using TFramework.Base;

/// <summary>
///
///角色总控制
///
/// </summary>

namespace Game.PlayerControlSpace
{
    public class PlayerControl : TMonoBehaviour
    {
        /// <summary>
        /// 角色是否初始化完成
        /// </summary>
        protected bool _isInited = false;
        /// <summary>
        /// 角色的身体
        /// </summary>
        protected GameObject _pBody;
        /// <summary>
        /// 角色位移控制
        /// </summary>
        protected PlayerMoveControl _moveControl;

        public override void Initialization(GameObject obj, bool beChild)
        {
            _isInited = false;

            base.Initialization(obj, false);

            InitPosition();

            FindChild();
            _moveControl = this.gameObject.AddComponent<PlayerMoveControl>();
            _moveControl.Initialization(_pBody, false);

            _isInited = true;

            //开始控制角色 临时的
            StartControlPlayer();
        }

        protected void FindChild()
        {
            _pBody = transform.Find("pBody").gameObject;
        }

        /// <summary>
        /// 初始化坐标
        /// 可以有多个出生点 玩家选择后 出生在对应出生点
        /// </summary>
        protected void InitPosition()
        {
            //这里先写假的 直接出生在一个定点
            transform.position = new Vector2(0, -2);
        }

        /// <summary>
        /// 开始控制角色
        /// 
        /// 临时这么写
        /// </summary>
        protected void StartControlPlayer()
        {
            _moveControl.ISCANMOVE = true;
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}