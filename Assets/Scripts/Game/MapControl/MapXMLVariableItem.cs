/// <summary>
/// 
/// 地图变量组
/// 
/// </summary>

using System;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.MapControlSpace
{
    /// <summary>
    /// 空间界限
    /// </summary>
    [Serializable]
    public class SpaceConfine
    {
        /// <summary>
        /// XY轴界限
        /// </summary>
        [XmlAttribute]
        public Vector2 XYAnnotation;

        public SpaceConfine()
        {

        }

        public virtual void ParseXML(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            if (node.Attributes.Count > 0)
            {
                try
                {
                    string[] xyAnnotation = node.Attributes["xyAnnotation"].Value.Split(',');
                    XYAnnotation = new Vector2(int.Parse(xyAnnotation[0]), int.Parse(xyAnnotation[1]));
                }
                catch (Exception)
                {
                    Debug.LogError("MapXMLVariableItem--SpaceConfine.ParseXML()读取参数失败!");
                    TLogger.LogError("MapXMLVariableItem--SpaceConfine.ParseXML()读取参数失败!");
                }
            }
        }
    }

    /// <summary>
    /// 空间对象
    /// </summary>
    [Serializable]
    public class SpaceObj
    {
        /// <summary>
        /// 对象类型
        /// </summary>
        [XmlAttribute]
        public string objType;
        /// <summary>
        /// 对象名称
        /// </summary>
        [XmlAttribute]
        public string objName;
        /// <summary>
        /// 对象ID
        /// </summary>
        [XmlAttribute]
        public string objID;
        /// <summary>
        /// 绑定的空间坐标 X轴
        /// </summary>
        [XmlAttribute]
        public Vector2 bindSpaceXY;
        /// <summary>
        /// 方向 -- 朝向
        /// </summary>
        [XmlAttribute]
        public string direction;

        public SpaceObj()
        {

        }

        public virtual void ParseXML(XmlNode node)
        {
            if (node == null)
            {
                return;
            }

            if (node.Attributes.Count > 0)
            {
                try
                {
                    objType = node.Attributes["objType"].Value;
                    objName = node.Attributes["objName"].Value;
                    objID = node.Attributes["objID"].Value;
                    string[] xybind = node.Attributes["bindSpaceXY"].Value.Split(',');
                    bindSpaceXY = new Vector2(int.Parse(xybind[0]), int.Parse(xybind[1]));
                    direction = node.Attributes["direction"].Value;
                }
                catch (Exception)
                {
                    Debug.LogError("MapXMLVariableItem--SpaceObj.ParseXML()读取参数失败!");
                    TLogger.LogError("MapXMLVariableItem--SpaceObj.ParseXML()读取参数失败!");
                }
            }
        }

    }
}