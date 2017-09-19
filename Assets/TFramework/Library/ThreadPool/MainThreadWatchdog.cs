using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class MainThreadWatchdog
	{
		private static Thread mainThread = null;
		public static void Init()
		{
			bool flag = MainThreadWatchdog.mainThread == null;
			if (flag)
			{
				Thread currentThread = Thread.CurrentThread;
				bool flag2 = currentThread.GetApartmentState() == ApartmentState.MTA || currentThread.ManagedThreadId > 1 || currentThread.IsThreadPoolThread;
				if (flag2)
				{
					Debug.Log("Trying to Init a WorkerThread as the MainThread! ");
				}
				else
				{
					MainThreadWatchdog.mainThread = currentThread;
				}
			}
		}
		public static bool CheckIfMainThread()
		{
			MainThreadWatchdog.Init();
			return Thread.CurrentThread == MainThreadWatchdog.mainThread;
		}
	}
}
