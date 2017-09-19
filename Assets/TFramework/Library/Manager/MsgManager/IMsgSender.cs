using UnityEngine;
using System.Collections;

namespace TFramework.Manager.EventManager
{

    /// <summary>
    /// 1.发送者需要,实现IMsgSender接口
    /// 2.调用this.SendLogicMsg发送Receiver Show Sth消息,并传入两个参数
    /// </summary>
    public interface IMsgSender
    {

    }
}
