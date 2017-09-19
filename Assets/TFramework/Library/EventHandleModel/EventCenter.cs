
using Tools;

namespace EventHandleModel
{

    using UnityEngine;
    using TFramework.Base;
    using System.Collections.Generic;

    /// <summary>
    ///
    ///写明注释 类的主要作用
    ///
    /// </summary>
    public class EventCenter : TMonoSingleton<EventCenter>
    {

        private EventCenter()
        {
        }

        //初始化
        public override void Initialization(GameObject parentOBJ)
        {
            base.Initialization(parentOBJ);
        }

        /// <summary>
        /// 保存事件监听处理列表,临时性的，可清空
        /// </summary>
        private Dictionary<int, IEventHandlerManger> eventHandlers = new Dictionary<int, IEventHandlerManger>();

        /// <summary>
        /// 永久性的，不可清空
        /// </summary>
        private Dictionary<int, IEventHandlerManger> eventHandlersPermanently =
            new Dictionary<int, IEventHandlerManger>();

        /// <summary>
        /// 事件队列
        /// </summary>
        private TSafeQueue<IEvent> eventQueue = new TSafeQueue<IEvent>();



        public void Update()
        {
            BoardCastEvent();
        }

        /// <summary>
        ///  处理永久性事件,其他事件不处理
        /// </summary>
        public void ProcessAndFilter()
        {
            while (eventQueue.Count > 0)
            {
                IEvent evt = eventQueue.Dequeue();
                if (eventHandlersPermanently.ContainsKey(evt.EventType))
                {
                    BoardCastEvent(evt);
                }

                evt.DestroySelf();

            }
        }

        public void Clear()
        {
            eventHandlers.Clear();
            ClearData();
        }

        public void ClearData()
        {
            TLogger.Log("清空事件队列");


            eventQueue.Clear();
        }

        public void ClearPermanently()
        {
            TLogger.DebugStackTraceInfo("清理永久性事件", "EventCenter", "ClearPermanently");
            this.eventHandlersPermanently.Clear();
        }




        public void ClearAll()
        {
            Clear();
            ClearPermanently();
        }

        public bool AddEventListener(int eventType, EventHandle eventHandle, bool isPermanently = false)
        {
            bool flag = false;
            if (!isPermanently)
            {
                if (!this.eventHandlers.ContainsKey(eventType))
                {
                    this.eventHandlers[eventType] = new EventReceiver();
                }
                flag = this.eventHandlers[eventType].AddHandler(eventHandle);
            }
            else
            {
                if (!this.eventHandlersPermanently.ContainsKey(eventType))
                {
                    this.eventHandlersPermanently[eventType] = new EventReceiver();
                }
                flag = this.eventHandlersPermanently[eventType].AddHandler(eventHandle);
            }

            return flag;
        }



        public bool AddEventListenerPermanently(int eventType, EventHandle eventHandle)
        {
            bool flag = false;
            if (!this.eventHandlersPermanently.ContainsKey(eventType))
            {
                this.eventHandlersPermanently[eventType] = new EventReceiver();
            }
            flag = this.eventHandlersPermanently[eventType].AddHandler(eventHandle);

            return flag;
        }






        /// <summary>
        /// 删除永久性监听事件
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="eventHandle"></param>
        /// <returns></returns>
        public bool RemoveEventListenerPermanently(int eventType, EventHandle eventHandle)
        {
            if (this.eventHandlersPermanently.ContainsKey(eventType))
            {
                bool flag = this.eventHandlersPermanently[eventType].RemoveHandler(eventHandle);
                return flag;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveEventListener(int eventType, EventHandle eventHandle)
        {
            if (this.eventHandlers.ContainsKey(eventType))
            {
                bool flag = this.eventHandlers[eventType].RemoveHandler(eventHandle);
                return flag;
            }
            else
            {
                return false;
            }
        }




        public void TriggerEvent(IEvent evt)
        {
            //if (GameConfig.DebugModule)
            //{
            //    string msg = string.Format("TriggerEvent :{0}", evt.ToSring());
            //     Logger.DebugInfo(msg, "EventCenter", "TriggerEvent");
            //}
            string msg = string.Format("TriggerEvent :{0}", evt.ToSring());
            TLogger.Log(msg);
            this.eventQueue.Enqueue(evt);
        }

        public void BoardCastEvent()
        {
            if (eventQueue.Count < 1)
            {
                return;
            }

            IEvent evt = eventQueue.Dequeue();

            BoardCastEvent(evt);

        }

        public void BoardCastEvent(IEvent evt)
        {
            if (evt == null)
            {
                TLogger.WarningInfo("evt==null :", "EventCenter", "BoardCastEvent");
                return;
            }
            int evtType = evt.EventType;

            ///永久性事件优先处理
            if (this.eventHandlersPermanently.ContainsKey(evtType))
            {
                this.eventHandlersPermanently[evtType].BoardcastEvent(evt);

            }

            if (!this.eventHandlers.ContainsKey(evtType))
            {
                TLogger.WarningInfo("eventHandlers not contain event process :" + evt.ToSring(), "EventCenter",
                    "BoardCastEvent");
                evt.DestroySelf();
                return;
            }

            this.eventHandlers[evtType].BoardcastEvent(evt);
            evt.DestroySelf();
        }

        public void DestroySelf()
        {

            Clear();
        }

        //继承重写销毁函数 里面有基类销毁管理
        protected override void OnDestroy()
        {
            base.OnDestroy();
            ClearAll();
        }

    }
}