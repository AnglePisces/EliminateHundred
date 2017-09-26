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
        //地图初始化控制
        protected MapInitControl _mapInitControl;
        public MapInitControl MAPINITCONTROL { get { return _mapInitControl; } }

        //地图总对象 所有的游戏对象都在这个总对象下
        protected GameObject _mapTotal;

        public override void Initialization(GameObject obj, bool beChild)
        {
            base.Initialization(obj, beChild);

            //初始化地图控制
            _mapTotal = new GameObject();
            _mapTotal.transform.position = Vector3.zero;
            _mapTotal.name = "MapTotal";
            _mapInitControl = _mapTotal.AddComponent<MapInitControl>();
            _mapInitControl.Initialization(null, false);
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}
