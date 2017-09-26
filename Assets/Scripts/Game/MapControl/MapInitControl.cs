using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TFramework.Base;


/// <summary>
///
/// 地图初始化控制
/// 
/// 1、图片与场景距离的映射关系
/// 2、场景地图根据配置进行生成
///
/// </summary>
namespace Game.MapControlSpace
{
    public class MapInitControl : TMonoBehaviour
    {
        #region 图和坐标的1单位映射关系 例如 图片每达到一个单位宽度 则x轴加1

        /// <summary>
        /// 图片单位宽度
        /// </summary>
        public int uintWidth = 16;

        /// <summary>
        /// 图片单位长度
        /// </summary>
        public int uintHeight = 16;

        #endregion

        /// <summary>
        /// 空间坐标界限 -- 这个就是地图的总范围
        /// </summary>
        public Vector2 _xyAnnotation;

        /// <summary>
        /// 地图配置文件控制
        /// </summary>
        protected MapXMLManager _mapXmlManager;

        public override void Initialization(GameObject obj, bool beChild)
        {
            base.Initialization(obj, beChild);
        }

        /// <summary>
        /// 初始化地图
        /// </summary>
        /// <param name="xmlManager">新的地图信息</param>
        public void InitMap(MapXMLManager xmlManager)
        {
            _mapXmlManager = xmlManager;

            if (_mapXmlManager != null)
            {
                StartInitMap();
            }
            else
            {
                Debug.LogError("MapInitControl.InitMap() _mapXmlManager内容是空的!");
                TLogger.Log("_mapXmlManager内容是空的!", "MapInitControl", "InitMap");
            }
        }

        /// <summary>
        /// 开始初始化地图
        /// </summary>
        protected void StartInitMap()
        {

        }

        /// <summary>
        /// 初始化墙壁
        /// </summary>
        protected void InitWall()
        {

        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

    }
}