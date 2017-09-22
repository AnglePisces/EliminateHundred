using UnityEngine;

namespace TFramework.Base
{

    /// <summary>
    /// 
    /// TFramwork的Mono生命周期对象基类
    /// 
    /// </summary>
    public abstract class TMono : MonoBehaviour
    {

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="parentOBJ">节点对象</param>
        /// /// <param name="beChild">是否成为节点对象的子节点</param>
        public abstract void Initialization(GameObject obj, bool beChild = false);
    }

}