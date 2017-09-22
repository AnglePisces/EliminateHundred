using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

public class TMonoBGO : TMonoBehaviour
{
    public string index = "0";

    //注册消息事件
    public void RegistMsg(string msgName)
    {
        GoRegisterGlobalMsg(msgName, ReceiverMsg);
        Debug.Log(index + " 事件注册完成");
    }

    //回调 -- 接受到消息 开始用Itween位移
    void ReceiverMsg(params object[] paramList)
    {
        Debug.Log("接收到 " + paramList[0].ToString() + " 发送的指令 -- " + index + " 开始响应");
        iTween.MoveTo(this.gameObject, iTween.Hash("position", new Vector3(10, 10, 10), "time", 2.0f, "looptype", "pingpong"));
    }

    //发送消息事件
    public void SendMsg(string msgName)
    {
        GoSendGlobalMsg(msgName, index);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
