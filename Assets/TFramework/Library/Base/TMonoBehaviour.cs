using System;
using System.Collections.Generic;
using TFramework.Global;
using TFramework.Manager.EventManager;
using UnityEngine;

namespace TFramework.Base
{
    /// <summary>
    /// 
    /// TFramwork的Mono生命周期对象扩展类
    /// 
    /// 实现接口:
    /// 1 消息收发器,实现消息收发管理,消息注册释放
    /// 
    /// </summary>
    public class TMonoBehaviour : TMono, IMsgTransceiver
    {
        #region 委托消息维护

        //记录委托消息 词典
        /// <summary>
        /// 记录委托消息 词典
        /// 每种类型 存一个消息名称链表
        /// </summary>
        protected Dictionary<TMsgChannel, List<string>> m_msgHandlerDict = new Dictionary<TMsgChannel, List<string>>();

        //注册消息
        /// <summary>
        /// 注册消息 -- 默认模式,只注册到全局类型 TMsgChannel.Global
        /// </summary>
        /// <param name="msgName">消息名称</param>
        /// <param name="callback">回调</param>
        protected void GoRegisterGlobalMsg(string msgName, VoidDelegate.WithParams callback)
        {
            // 检测空变量
            if (string.IsNullOrEmpty(msgName))
            {
                Debug.LogError("RegisterMsg:" + msgName + " is Null or Empty");
                return;
            }

            // 检测空变量
            if (null == callback)
            {
                Debug.LogError("RegisterMsg:" + msgName + " callback is Null");
                return;
            }

            GoRegisterMsgByChannel(TMsgChannel.Global, msgName, callback);
        }
        //注册消息
        /// <summary>
        /// 注册消息 -- 自定义类型模式
        /// </summary>
        /// <param name="channel">消息类型</param>
        /// <param name="msgName">消息名称</param>
        /// <param name="callback">回调</param>
        protected void GoRegisterMsgByChannel(TMsgChannel channel, string msgName, VoidDelegate.WithParams callback)
        {
            // 检测空变量
            if (string.IsNullOrEmpty(msgName))
            {
                Debug.LogError("RegisterMsg:" + msgName + " is Null or Empty");
                return;
            }

            // 检测空变量
            if (null == callback)
            {
                Debug.LogError("RegisterMsg:" + msgName + " callback is Null");
                return;
            }

            //如果词典为空 则新建
            if (m_msgHandlerDict == null)
            {
                m_msgHandlerDict = new Dictionary<TMsgChannel, List<string>>();
            }

            // 添加消息通道
            if (!m_msgHandlerDict.ContainsKey(TMsgChannel.Global))
            {
                m_msgHandlerDict[channel] = new List<string>();
            }

            // 检测是否存在 不存在 则添加
            if (!m_msgHandlerDict[channel].Contains(msgName))
            {
                m_msgHandlerDict[channel].Add(msgName);
            }

            this.RegisterMsgByChannel(channel, msgName, callback);
        }

        //释放注册消息
        /// <summary>
        /// 释放注册消息
        /// 默认模式,只释放全局类型 TMsgChannel.Global
        /// </summary>
        /// <param name="msgName">消息名称</param>
        protected void GoUnRegisterGlobalMsg(string msgName)
        {
            // 检测空变量
            if (string.IsNullOrEmpty(msgName))
            {
                Debug.LogError("UnRegisterMsg:" + msgName + " is Null or Empty");
                return;
            }

            GoUnRegisterMsgByChannel(TMsgChannel.Global, msgName);
        }
        /// <summary>
        /// 释放注册消息 -- 自定义模式
        /// </summary>
        /// <param name="channel">消息类型</param>
        /// <param name="msgName">消息名称</param>
        protected void GoUnRegisterMsgByChannel(TMsgChannel channel, string msgName)
        {
            // 检测空变量
            if (string.IsNullOrEmpty(msgName))
            {
                Debug.LogError("UnRegisterMsg:" + msgName + " is Null or Empty");
                return;
            }

            this.UnRegisterMsgByChannel(channel, msgName);
        }

        //释放所有注册消息 -- 一般在生命周期结束时释放
        /// <summary>
        /// 释放所有注册消息 -- 一般在生命周期结束时释放
        /// </summary>
        protected void GoUnAllReqisterMsg()
        {
            if (m_msgHandlerDict != null)
            {
                foreach (TMsgChannel channel in m_msgHandlerDict.Keys)
                {
                    List<string> list = m_msgHandlerDict[channel];

                    for (int i = 0; i < list.Count; i++)
                    {
                        GoUnRegisterMsgByChannel(channel, list[i]);
                    }
                }

                m_msgHandlerDict = null;
            }
        }

        //发送消息
        /// <summary>
        /// 发送消息 -- 默认模式,发送到全局类型 TMsgChannel.Global
        /// </summary>
        protected void GoSendGlobalMsg(string msgName, params object[] paramList)
        {
            if (string.IsNullOrEmpty(msgName))
            {
                return;
            }

            this.SendGlobalMsg(msgName, paramList);
        }
        /// <summary>
        /// 发送消息 -- 自定义模式,发送到全局类型 TMsgChannel.Global
        /// </summary>
        protected void GoSendMsgByChannel(TMsgChannel channel, string msgName, params object[] paramList)
        {
            if (string.IsNullOrEmpty(msgName))
            {
                return;
            }

            this.SendMsgByChannel(channel, msgName, paramList);
        }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parentOBJ">节点对象</param>
        /// /// <param name="beChild">是否成为节点对象的子节点</param>
        public override void Initialization(GameObject obj, bool beChild)
        {
            if (obj != null)
            {
                if (beChild)
                {
                    this.transform.parent = obj.transform;
                }
            }
        }

        /// <summary>
        /// 销毁回调 
        /// 1 卸载所有注册消息
        /// </summary>
        protected virtual void OnDestroy()
        {
            GoUnAllReqisterMsg();
        }

    }
}
