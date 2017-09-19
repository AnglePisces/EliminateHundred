using System;

namespace TFramework.Base
{
    /// <summary>
    /// 
    /// 基本的C#单例
    /// 
    /// </summary>
    public abstract class TSingleton<T> where T : class
    {
        protected static T _instance = null;
        private static readonly object _instanceLock = new object(); // 锁，控制多线程冲突

        public static T Instance
        {

            get
            {
                // 此处双重检测，确保多线程无冲突
                if (_instance == null)
                {
                    lock (_instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = (T)Activator.CreateInstance(typeof(T), true);
                        }
                    }
                }
                return _instance;
            }
        }

        public void DelMe()
        {
            // 此处双重检测，确保多线程无冲突
            if (_instance != null)
            {
                lock (_instanceLock)
                {
                    if (_instance != null)
                    {
                        _instance = null;
                    }
                }
            }
        }

    }
}