using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class ThreadWaitForSeconds
	{
		public ThreadWaitForSeconds(float seconds)
		{
			bool flag = MainThreadWatchdog.CheckIfMainThread();
			if (flag)
			{
				Debug.Log("Its not allowed to put the MainThread to sleep!");
			}
			else
			{
				bool flag2 = !UnityActivityWatchdog.CheckUnityRunning();
				if (!flag2)
				{
					Thread.Sleep((int)Mathf.Max(1f, Mathf.Round(seconds * 1000f)));
					while (!UnityActivityWatchdog.CheckUnityActive())
					{
						Thread.Sleep(5);
					}
				}
			}
		}
	}
}
