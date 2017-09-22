using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

/// <summary>
///
///角色位移控制
///
/// </summary>

namespace Game.PlayerControlSpace
{
    public class PlayerMoveControl : TMonoBehaviour
    {
        /// <summary>
        /// 角色是否可以移动
        /// </summary>
        protected bool _isCanMove = false;
        /// <summary>
        /// 角色是否可以移动 -- 存储器
        /// </summary>
        public bool ISCANMOVE
        {
            get { return _isCanMove; }
            set { _isCanMove = value; }
        }

        /// <summary>
        /// 角色身体点 -- 移动的是根节点而不是身体
        /// </summary>
        protected GameObject _pBody;
        /// <summary>
        /// 身体动画
        /// </summary>
        protected Animator _bodyAnimator;
        /// <summary>
        /// 碰撞体 -- 根节点上
        /// </summary>
        protected Collider2D _collider2D;
        /// <summary>
        /// 刚体 -- 根节点上
        /// </summary>
        protected Rigidbody2D _rigidBnRigidbody2D;
        /// <summary>
        /// 位移速度
        /// </summary>
        public float _speed = 0.4f;
        /// <summary>
        /// 目的地位置
        /// </summary>
        protected Vector2 _dest = Vector2.zero;

        public override void Initialization(GameObject obj, bool beChild)
        {
            base.Initialization(obj, false);
            _pBody = obj;
            _bodyAnimator = _pBody.GetComponent<Animator>();
            _collider2D = gameObject.GetComponent<Collider2D>();
            _rigidBnRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _dest = transform.position;
        }

        void FixedUpdate()
        {
            //游戏没有开始 跳出update
            if (!GameSceneControl.Instance._isStart)
            {
                return;
            }

            //如果不能移动 跳出update检测
            if (!_isCanMove)
            {
                return;
            }

            //平滑位移
            _rigidBnRigidbody2D.velocity = Vector3.zero;
            Vector2 p = Vector2.MoveTowards(transform.position, _dest, _speed);
            _rigidBnRigidbody2D.MovePosition(p);

            //按键移动控制 -- 目前只做按键的 之后扩展UI层按钮移动
            //如果到了目的地 才开始响应按键事件
            //位移的模式 -- 如果有对应方向位移 事件发生并且没有碰到阻止移动的物体
            //则向对应方向移动一个单位
            if (Input.GetKey(KeyCode.UpArrow))
            {
                _dest = (Vector2)transform.position + Vector2.up;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                _dest = (Vector2)transform.position + Vector2.right;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                _dest = (Vector2)transform.position - Vector2.up;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _dest = (Vector2)transform.position - Vector2.right;
            }

            //设置当前动画
            Vector2 dir = _dest - (Vector2)transform.position;
            _bodyAnimator.SetFloat("DirX", dir.x);
            _bodyAnimator.SetFloat("DirY", dir.y);
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}