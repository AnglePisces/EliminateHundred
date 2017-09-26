using UnityEngine;
using System.Collections;
using Game.MapControlSpace;
using TFramework.Base;

/// <summary>
///
/// 地图管理器
/// 
/// 1 初始化地图
/// 2 地图根据配置来生成
///
/// </summary>

namespace Game
{
    public class GameMapControl : TMonoSingleton<GameMapControl>
    {

        private GameMapControl() { }

        /// <summary>
        /// 地图配置文件控制
        /// </summary>
        protected MapXMLManager _mapXmlManager;
        /// <summary>
        /// 地图控制
        /// </summary>
        protected MapControl _mapControl;

        //初始化
        public override void Initialization(GameObject obj, bool beChild)
        {
            base.Initialization(obj, beChild);

            InitMap();
        }

        /// <summary>
        /// 初始化地图
        /// 根据配置文件 生成对应的地图
        /// </summary>
        protected void InitMap()
        {

            //目前写假的 一个整体的地图直接实例化
            GameObject obj = ResourcesManager.Instance.GetIniPrefabResourceByName("Map");
            _mapControl = obj.AddComponent<MapControl>();
            obj.name = "MapControl";
            _mapControl.Initialization(null, false);
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
