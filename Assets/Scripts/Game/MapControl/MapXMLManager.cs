using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// 
/// 地图配置管理
/// 
/// 1、加载地图配置
/// 2、存储地图配置
/// 3、删除地图配置
/// 
/// </summary>
namespace Game.MapControlSpace
{

    [XmlRoot("MapManager")]
    [Serializable]
    public class MapXMLManager
    {
        /// <summary>
        /// 空间界限
        /// </summary>
        [XmlElement]
        public SpaceConfine _spaceConfine;
        /// <summary>
        /// 空间对象列表
        /// </summary>
        [XmlArrayItem]
        public List<SpaceObj> _spaceObjList;

        public void ParseXML(string xmlString)
        {
            if (xmlString == null || string.IsNullOrEmpty(xmlString))
            {
                Debug.LogError("MapXMLManager.ParseXML() xml内容是空的!");
                TLogger.Log("xml内容是空的!", "MapXMLManager", "ParseXML");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

            XmlNode spaceConfine = doc.SelectSingleNode("/MapManager/SpaceConfine");
            if (spaceConfine != null)
            {
                _spaceConfine = new SpaceConfine();
                _spaceConfine.ParseXML(spaceConfine);
            }
            else
            {
                Debug.LogError("MapXMLManager.ParseXML() spaceConfine节点不存在!");
                TLogger.Log("spaceConfine节点不存在!", "MapXMLManager", "ParseXML");
            }

            XmlNode SpaceObjList = doc.SelectSingleNode("/MapManager/SpaceObjList");
            if (SpaceObjList != null)
            {
                _spaceObjList = new List<SpaceObj>();
                for (int i = 0; i < SpaceObjList.ChildNodes.Count; i++)
                {
                    SpaceObj obj = new SpaceObj();
                    obj.ParseXML(SpaceObjList.ChildNodes[i]);
                    _spaceObjList.Add(obj);
                }
            }

        }

    }
}