using EventHandleModel;
/// <summary>
/// 
/// 事件类
/// 
/// </summary>
namespace EHEvent
{
    //加载事件
    public class EHLoadingEvent : IEvent
    {

        public int EventType
        {
            get { return (int)EHGameProcessEventID.Process_Loading_Event; }
        }


        //是否下载正确
        public bool _state;
        //错误信息
        public string _log;

        public void DestroySelf()
        {
        }

        public string ToSring()
        {
            string msg = string.Format("EventType:{0},State:{1},Log:{2}", EHGameProcessEventID.Process_Loading_Event.ToString(), _state, _log);
            return msg;
        }

    }

    //加载事件
    public class EHLoginEvent : IEvent
    {

        public int EventType
        {
            get { return (int)EHGameProcessEventID.Process_Login_Event; }
        }


        //是否登录成功
        public bool _state;
        //错误信息
        public string _log;

        public void DestroySelf()
        {
        }

        public string ToSring()
        {
            string msg = string.Format("EventType:{0},State:{1},Log:{2}", EHGameProcessEventID.Process_Login_Event.ToString(), _state, _log);
            return msg;
        }

    }

}
