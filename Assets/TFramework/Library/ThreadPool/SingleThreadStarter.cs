using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public static class SingleThreadStarter
	{
		private class SafeSingleThreadSession
		{
			private ThreadStart targetMethod;
			private ParameterizedThreadStart paramTargetMethod;
			public SafeSingleThreadSession(ThreadStart targetMethod)
			{
				this.targetMethod = targetMethod;
			}
			public SafeSingleThreadSession(ParameterizedThreadStart targetMethod)
			{
				this.paramTargetMethod = targetMethod;
			}
			public void SafeExecte_ThreadStart()
			{
				try
				{
					this.targetMethod();
				}
				catch (ThreadInterruptedException e)
				{
					Thread.CurrentThread.Interrupt();
                    Loom.DispatchToMainThread(delegate
                    {
                        Debug.LogError(e.Message + e.StackTrace + "\n\n");
                    },true,true);
				}
				catch (Exception e)
				{
					Loom.DispatchToMainThread(delegate
					{
						Debug.LogError(e.Message + e.StackTrace + "\n\n");
					},true,true);
				}
			}
			public void SafeExecte_ParamThreadStart(object argument)
			{
				try
				{
					this.paramTargetMethod(argument);
				}
				catch (Exception arg_18_0)
				{
					Exception e2 = arg_18_0;
					Exception e = e2;
					Loom.DispatchToMainThread(delegate
					{
						Debug.LogError(e.Message + e.StackTrace + "\n\n");
					}, true, true);
				}
			}
		}
		private static List<Thread> startedThreads = new List<Thread>();
		public static Thread StartSingleThread(ThreadStart targetMethod, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal, bool safeMode = true)
		{
			SingleThreadStarter.Init();
			MainThreadWatchdog.Init();
			MainThreadDispatcher.Init();
			UnityActivityWatchdog.Init();
			Thread thread;
			if (safeMode)
			{
				SingleThreadStarter.SafeSingleThreadSession @object = new SingleThreadStarter.SafeSingleThreadSession(targetMethod);
				thread = new Thread(new ThreadStart(@object.SafeExecte_ThreadStart));
			}
			else
			{
				thread = new Thread(targetMethod);
			}
			thread.Priority = priority;
			SingleThreadStarter.startedThreads.Add(thread);
			thread.Start();
			return thread;
		}
		public static Thread StartSingleThread(ParameterizedThreadStart targetMethod, object argument, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal, bool safeMode = true)
		{
			SingleThreadStarter.Init();
			MainThreadWatchdog.Init();
			MainThreadDispatcher.Init();
			UnityActivityWatchdog.Init();
			Thread thread;
			if (safeMode)
			{
				SingleThreadStarter.SafeSingleThreadSession @object = new SingleThreadStarter.SafeSingleThreadSession(targetMethod);
				thread = new Thread(new ParameterizedThreadStart(@object.SafeExecte_ParamThreadStart));
			}
			else
			{
				thread = new Thread(targetMethod);
			}
			thread.Priority = priority;
			SingleThreadStarter.startedThreads.Add(thread);
			thread.Start(argument);
			return thread;
		}
		public static void Init()
		{
			SingleThreadStarter.ValidateThreadStates();
		}
		public static void AbortRunningThreads()
		{
			SingleThreadStarter.ValidateThreadStates();
			Debug.Log("Abort running Threads: " + SingleThreadStarter.startedThreads.Count);
			foreach (Thread current in SingleThreadStarter.startedThreads)
			{
				current.Interrupt();
				current.Join(100);
			}
			SingleThreadStarter.startedThreads.Clear();
		}
		private static void ValidateThreadStates()
		{
			int num = SingleThreadStarter.startedThreads.Count;
			while (true)
			{
				int num2 = num - 1;
				num = num2;
				if (num2 <= -1)
				{
					break;
				}
				Thread thread = SingleThreadStarter.startedThreads[num];
				bool flag = thread.ThreadState == ThreadState.Aborted || thread.ThreadState == ThreadState.AbortRequested || thread.ThreadState == ThreadState.Stopped || thread.ThreadState == ThreadState.StopRequested || thread.ThreadState == ThreadState.Unstarted;
				if (flag)
				{
					SingleThreadStarter.startedThreads.RemoveAt(num);
				}
			}
		}
	}
}
