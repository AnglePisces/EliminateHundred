using Frankfort.Threading;
using Frankfort.Threading.Internal;
using System;
using System.Threading;
using Tools;
using UnityEngine;
public static class Loom
{
	public static Thread StartSingleThread(ThreadStart targetMethod, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal, bool safeMode = true)
	{
		return SingleThreadStarter.StartSingleThread(targetMethod, priority, safeMode);
	}
	public static Thread StartSingleThread(ParameterizedThreadStart targetMethod, object argument, System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.Normal, bool safeMode = true)
	{
		return SingleThreadStarter.StartSingleThread(targetMethod, argument, priority, safeMode);
	}
	public static void AbortRunningThreads()
	{
		SingleThreadStarter.AbortRunningThreads();
	}
	public static ThreadPoolScheduler StartMultithreadedWorkloadExecution<T>(ThreadWorkloadExecutor<T> workloadExecutor, T[] workLoad, MultithreadedWorkloadComplete<T> onComplete, MultithreadedWorkloadPackageComplete<T> onPackageComplete, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
	{
		return MultithreadedWorkloadHelper.StartMultithreadedWorkloadExecution<ThreadWorkloadExecutor<T>, T>(workloadExecutor, workLoad, null, onComplete, onPackageComplete, maxThreads, scheduler, safeMode);
	}
	public static ThreadPoolScheduler StartMultithreadedWorkloadExecution<T>(ThreadWorkloadExecutorIndexed<T> workloadExecutor, T[] workLoad, MultithreadedWorkloadComplete<T> onComplete, MultithreadedWorkloadPackageComplete<T> onPackageComplete, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
	{
		return MultithreadedWorkloadHelper.StartMultithreadedWorkloadExecution<ThreadWorkloadExecutorIndexed<T>, T>(workloadExecutor, workLoad, null, onComplete, onPackageComplete, maxThreads, scheduler, safeMode);
	}
	public static ThreadPoolScheduler StartMultithreadedWorkloadExecution<T>(ThreadWorkloadExecutorArg<T> workloadExecutor, T[] workLoad, object extraArgument, MultithreadedWorkloadComplete<T> onComplete, MultithreadedWorkloadPackageComplete<T> onPackageComplete, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
	{
		return MultithreadedWorkloadHelper.StartMultithreadedWorkloadExecution<ThreadWorkloadExecutorArg<T>, T>(workloadExecutor, workLoad, extraArgument, onComplete, onPackageComplete, maxThreads, scheduler, safeMode);
	}
	public static ThreadPoolScheduler StartMultithreadedWorkloadExecution<T>(ThreadWorkloadExecutorArgIndexed<T> workloadExecutor, T[] workLoad, object extraArgument, MultithreadedWorkloadComplete<T> onComplete, MultithreadedWorkloadPackageComplete<T> onPackageComplete, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
	{
		return MultithreadedWorkloadHelper.StartMultithreadedWorkloadExecution<ThreadWorkloadExecutorArgIndexed<T>, T>(workloadExecutor, workLoad, extraArgument, onComplete, onPackageComplete, maxThreads, scheduler, safeMode);
	}
	public static ThreadPoolScheduler StartMultithreadedWorkerObjects(IThreadWorkerObject[] workerObjects, ThreadPoolSchedulerEvent onCompleteCallBack, ThreadedWorkCompleteEvent onPackageExecuted = null, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
	{
		bool flag = scheduler == null;
		if (flag)
		{
			scheduler = Loom.CreateThreadPoolScheduler();
		}
		scheduler.StartASyncThreads(workerObjects, onCompleteCallBack, onPackageExecuted, maxThreads, safeMode);
		return scheduler;
	}
	public static ThreadPoolScheduler CreateThreadPoolScheduler()
	{
		GameObject gameObject = new GameObject("ThreadPoolScheduler");
		return gameObject.AddComponent<ThreadPoolScheduler>();
	}
	public static ThreadPoolScheduler CreateThreadPoolScheduler(string name)
	{
		GameObject gameObject = new GameObject((name == null || name == string.Empty) ? "ThreadPoolScheduler" : name);
		return gameObject.AddComponent<ThreadPoolScheduler>();
	}
	public static void WaitForNextFrame(int waitFrames = 1)
	{
		new ThreadWaitForNextFrame(waitFrames, 5);
	}
	public static void WaitForSeconds(float seconds)
	{
		new ThreadWaitForSeconds(seconds);
	}
	public static void DispatchToMainThread(CallBack dispatchCall, bool waitForExecution = false, bool safeMode = true)
	{
		MainThreadDispatcher.DispatchToMainThread(dispatchCall, waitForExecution, safeMode);
	}

	public static object DispatchToMainThreadReturn<T>(CallBackArgRturn<T> dispatchCall, T dispatchArgument, bool safeMode = true)
	{
		return MainThreadDispatcher.DispatchToMainThreadReturn<T>(dispatchCall, dispatchArgument, safeMode);
	}
	public static object DispatchToMainThreadReturn(CallBackReturn dispatchCall, bool safeMode = true)
	{
		return MainThreadDispatcher.DispatchToMainThreadReturn(dispatchCall, safeMode);
	}
	public static T DispatchToMainThreadReturn<T>(CallBackReturn<T> dispatchCall, bool safeMode = true)
	{
		return MainThreadDispatcher.DispatchToMainThreadReturn<T>(dispatchCall, safeMode);
	}
	public static void QueueOnMainThread(Action action)
	{
		MainThreadDispatcher.QueueOnMainThread(action);
	}
	public static void QueueOnMainThread(Action action, float time)
	{
		MainThreadDispatcher.QueueOnMainThread(action, time);
	}
	public static bool CheckUnityActive()
	{
		return UnityActivityWatchdog.CheckUnityActive();
	}
	public static void SleepOrAbortIfUnityInactive()
	{
		UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
	}
	public static bool CheckIfMainThread()
	{
		return MainThreadWatchdog.CheckIfMainThread();
	}
	public static void Init()
	{
		SingleThreadStarter.Init();
		MainThreadWatchdog.Init();
		MainThreadDispatcher.Init();
		UnityActivityWatchdog.Init();
	}
}
