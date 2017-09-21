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


        //是否成功
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

    //登录事件
    public class EHLoginEvent : IEvent
    {

        public int EventType
        {
            get { return (int)EHGameProcessEventID.Process_Login_Event; }
        }


        //是否成功
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

    //注册事件
    public class EHRegisterEvent : IEvent
    {

        public int EventType
        {
            get { return (int)EHGameProcessEventID.Process_Register_Event; }
        }


        //是否成功
        public bool _state;
        //错误信息
        public string _log;
        //注册的帐号
        public string _id;
        //注册的密码
        public string _pwd;

        public void DestroySelf()
        {
        }

        public string ToSring()
        {
            string msg = string.Format("EventType:{0},State:{1},Log:{2}", EHGameProcessEventID.Process_Register_Event.ToString(), _state, _log);
            return msg;
        }

    }
}
