using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

/// <summary>
///
///角色位移控制
///
/// </summary>

namespace PlayerControl
{
    public class PlayerMoveControl : TMonoBehaviour
    {
        //角色根节点 -- 移动移动的是根节点
        protected GameObject _rootBody;
        //碰撞体
        protected Collider2D _collider2D;
        //位移速度
        public float _speed = 0.4f;
        //当前位置
        protected Vector2 _dest = Vector2.zero;

        public override void Initialization(GameObject parentOBJ, bool beChild)
        {
            base.Initialization(parentOBJ, false);
            _rootBody = parentOBJ;
            
        }

        /// <summary>
        /// 校验是否碰撞到了东西 是否可向前继续前进
        /// 从前方一个单位的前近距离向自身对象发射射线
        /// 射线碰撞结果是自己的时候 则说明没东西
        /// 如果不是自己 则说明前面将碰到其它物体 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        protected bool ValidCollider(Vector2 dir)
        {
            Vector2 pos = transform.position;
            RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
            //目前简单检测 只要碰到物体就返回false 之后需要修改成同是玩家 返回true
            return (hit.collider == _collider2D);
        }

        void FixedUpdate()
        {

        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}