using UnityEngine;

namespace TFramework.Base
{
    /// <summary>
    /// 
    /// 使用Unity生命周期的单例
    /// 
    /// 1 单例模式
    /// 2 支持消息事件
    /// 
    /// </summary>
    public abstract class TMonoSingleton<T> : TMonoBehaviour where T : TMonoSingleton<T>
    {
        protected static T _instance = null;
        private static object _instancelock = new object();

        public static T Instance
        {
            get
            {
                if (QuitGameControl.IsQuitGame)
                {
                    Debug.LogWarning("[TMonoSingleton] Instance '" + typeof(T) +
                            "' already destroyed on application quit." +
                            " Won't create again - returning null.");
                    return null;
                }

                lock (_instancelock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[TMonoSingleton] Something went really wrong " +
                                    " - there should never be more than 1 singleton!" +
                                    " Reopenning the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();


                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(monoSingleton) " + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);

                            Debug.Log("[TMonoSingleton] An instance of " + typeof(T) +
                                    " is needed in the scene, so '" + singleton +
                                    "' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log("[TMonoSingleton] Using instance already created: " +
                                    _instance.gameObject.name);
                        }
                    }

                    return _instance;
                }
            }
        }

        protected override void OnDestroy()
        {
            _instance = null;
        }
    }
}
