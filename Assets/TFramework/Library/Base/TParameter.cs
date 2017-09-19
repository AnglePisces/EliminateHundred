using TFramework.Global;

namespace TFramework.Base
{
    /// <summary>
    /// 变量基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TParameter<T>
    {
        /// <summary>
        /// 委托
        /// 无参回调
        /// </summary>
        public VoidDelegate.WithVoid _observerOfSetDelegate = null;
        /// <summary>
        /// 委托
        /// 将自身值 传入
        /// </summary>
        public VoidDelegate.WithParam _observerOfSetSendValueDelegate = null;

        private T m_t = default(T);
        public T value
        {
            set
            {
                if (null != m_t && m_t.Equals(value))
                {
                    return;
                }
                m_t = value;
                if (_observerOfSetDelegate != null)
                {
                    _observerOfSetDelegate();
                }
                if (_observerOfSetSendValueDelegate != null)
                {
                    _observerOfSetSendValueDelegate(m_t);
                }
            }
            get
            {
                return m_t;
            }
        }
        public void SetDefaultValue(T value)
        {
            m_t = value;
        }
        public void Clear()
        {
            m_t = default(T);
        }
    }
}
