namespace EventHandleModel
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// 管理同一类事件处理
    /// </summary>
    public class EventReceiver : IEventHandlerManger
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        private int eventType;
        /// <summary>
        /// 事件处理监听列表
        /// </summary>
        private List<EventHandle> events = new List<EventHandle>();


        public bool AddHandler(EventHandle handle)
        {
            if (this.events.Exists((EventHandle item) => item == handle))
            {
                return false;
            }

            this.events.Add(handle);
            return true;
        }

        public bool RemoveHandler(EventHandle handle)
        {

            if (this.events.Contains(handle))
            {
                this.events.Remove(handle);
                return true;
            }
            return false;
        }

        public void BoardcastEvent(IEvent evt)
        {

            List<EventHandle> list = new List<EventHandle>();
            list.AddRange(this.events);
            foreach (EventHandle current in list)
            {
                current(evt);
            }


        }

        public int EventType
        {
            get
            {
                return eventType;
            }
            set
            {
                eventType = value;
            }
        }

        public void Clear()
        {

            this.events.Clear();
        }

        public void DestroySelf()
        {
            if (events != null)
            {
                Clear();
                events = null;
            }

        }



        public string ToSring()
        {
            return string.Empty;
        }
    }
}
