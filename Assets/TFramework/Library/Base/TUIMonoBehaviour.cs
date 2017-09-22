namespace TFramework.Base
{
    public abstract class TUIMonoBehaviour : TMonoBehaviour
    {
        /// <summary>
        /// 查找对象
        /// </summary>
        protected abstract void FindChild();

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}