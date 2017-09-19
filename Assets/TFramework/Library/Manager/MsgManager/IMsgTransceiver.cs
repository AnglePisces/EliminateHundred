using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFramework.Manager.EventManager
{
    /// <summary>
    /// 
    /// 消息收发器
    /// 
    /// 1.同时是接收者(实现IMsgReceiver接口)和发送者(实现IMsgSender接口)
    /// 2.使用this.RegisterLogicMsg注册消息和回调函数
    /// 2.调用this.SendLogicMsg发送消息等
    /// </summary>
    public interface IMsgTransceiver : IMsgSender, IMsgReceiver
    {

    }
}
