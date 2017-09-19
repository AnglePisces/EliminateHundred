using UnityEngine;
using UnityEngine.EventSystems;

namespace TFramework.Global
{
    /// <summary>
    /// 返回空类型的回调定义
    /// </summary>
    public class VoidDelegate
    {

        public delegate void WithVoid();

        public delegate void WithGo(GameObject go);

        public delegate void WithParams(params object[] paramList);

        public delegate void WithParam(object param);

        public delegate void WithEvent(BaseEventData data);
    }

}
