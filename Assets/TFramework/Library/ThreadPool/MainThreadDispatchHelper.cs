using System;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class MainThreadDispatchHelper : MonoBehaviour
	{
		private float WaitForSecondsTime = 0.005f;
		private void Awake()
		{
			MainThreadWatchdog.Init();
			UnityActivityWatchdog.Init();
			base.InvokeRepeating("UpdateMainThreadDispatcher", this.WaitForSecondsTime, this.WaitForSecondsTime);
		}
		private void Update()
		{
			MainThreadDispatcher.currentFrame = Time.frameCount;
		}
		private void UpdateMainThreadDispatcher()
		{
			MainThreadDispatcher.DispatchActionsIfPresent();
			MainThreadDispatcher.RunActioin();
			MainThreadDispatcher.RunDelayAction();
		}
	}
}
