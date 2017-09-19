namespace EventHandleModel
{
    using Common;
    public interface IEvent : IDestroy
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        int EventType
        {
            get;

        }

        string ToSring();

    }

    /// <summary>
    /// 事件处理代理
    /// </summary>
    /// <param name="evt"></param>
    public delegate void EventHandle(IEvent evt);

    /// <summary>
    /// 同一事件处理监听列表管理
    /// </summary>
    public interface IEventHandlerManger : IEvent, IDestroy
    {
        /// <summary>
        /// 添加事件监听
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        bool AddHandler(EventHandle handle);
        /// <summary>
        /// 删除事件监听
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        bool RemoveHandler(EventHandle handle);
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="evt"></param>
        void BoardcastEvent(IEvent evt);

        void Clear();

    }


    public interface IEventCenter : IDestroy
    {
        /// <summary>
        /// 添加事件处理监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandle">事件处理函数</param>
        /// <param name="isPermanently">是否为永久性记录，不清楚</param>
        /// <returns></returns>
        bool AddEventListener(int eventType, EventHandle eventHandle, bool isPermanently = false);
        /// <summary>
        /// 删除事件处理监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandle">事件处理函数</param>
        /// <returns></returns>
        bool RemoveEventListener(int eventType, EventHandle eventHandle);
        /// <summary>
        /// 触发事件
        /// </summary>
        /// <param name="evt"></param>
        void TriggerEvent(IEvent evt);
        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="evt"></param>
        void BoardCastEvent();

    }
}