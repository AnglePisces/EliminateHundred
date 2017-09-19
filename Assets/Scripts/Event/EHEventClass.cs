using EventHandleModel;
/// <summary>
/// 
/// 事件类
/// 
/// </summary>
namespace EHEvent
{

    public class EHLoadingEvent : IEvent
    {

        public int EventType
        {
            get { return (int) EHGameProcessEventID.Process_Loading_Event; }
        }


        //是否下载正确
        public bool _LoadingState;

        public void DestroySelf()
        {
        }

        public string ToSring()
        {
            string msg = string.Format("EventType:{0},ConnectState:{1}", EHGameProcessEventID.Process_Loading_Event.ToString(), _LoadingState);
            return msg;
        }


    }

}
