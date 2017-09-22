using UnityEngine;
using System.Collections;
using TFramework.Base;

/// <summary>
///
///地图控制器
/// 
/// 1 对当前地图进行控制
///
/// </summary>

namespace Game.MapControlSpace
{
    public class MapControl : TMonoBehaviour
    {

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
