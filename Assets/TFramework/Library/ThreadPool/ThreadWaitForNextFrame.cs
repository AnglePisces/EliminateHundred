using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class ThreadWaitForNextFrame
	{
		public ThreadWaitForNextFrame(int waitFrames = 1, int sleepTime = 5)
		{
			bool flag = waitFrames > 0;
			if (flag)
			{
				bool flag2 = MainThreadWatchdog.CheckIfMainThread();
				if (flag2)
				{
					Debug.Log("Its not allowed to put the MainThread to sleep!");
				}
				else
				{
					int currentFrame = MainThreadDispatcher.currentFrame;
					bool flag3 = !UnityActivityWatchdog.CheckUnityRunning();
					if (!flag3)
					{
						Thread.Sleep(sleepTime);
						while (!UnityActivityWatchdog.CheckUnityActive() || currentFrame + waitFrames >= MainThreadDispatcher.currentFrame)
						{
							Thread.Sleep(sleepTime);
						}
					}
				}
			}
		}
	}
}
