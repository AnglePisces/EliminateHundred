using System;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public static class MultithreadedWorkloadHelper
	{
		public static ThreadPoolScheduler StartMultithreadedWorkloadExecution<D, T>(D executor, T[] workLoad, object extraArgument, MultithreadedWorkloadComplete<T> onComplete, MultithreadedWorkloadPackageComplete<T> onPackageComplete, int maxThreads = -1, ThreadPoolScheduler scheduler = null, bool safeMode = true)
		{
			bool flag = scheduler == null;
			if (flag)
			{
				scheduler = Loom.CreateThreadPoolScheduler();
			}
			else
			{
				bool isBusy = scheduler.isBusy;
				if (isBusy)
				{
					Debug.LogError("Provided Scheduler stil busy!!!");
				}
			}
			bool flag2 = maxThreads <= 0;
			if (flag2)
			{
				maxThreads = Mathf.Max(SystemInfo.processorCount - 1, 1);
			}
			int num = 1;
			bool flag3 = maxThreads > 1;
			if (flag3)
			{
				num = 2;
			}
			int num2 = Mathf.Min(maxThreads * num, workLoad.Length);
			int num3 = (int)Mathf.Ceil((float)workLoad.Length / (float)num2);
			ThreadWorkDistribution<T>[] array = new ThreadWorkDistribution<T>[num2];
			Type typeFromHandle = typeof(D);
			int num4 = 0;
			int num6;
			for (int i = 0; i < num2; i = num6 + 1)
			{
				int num5 = Mathf.Min(workLoad.Length - num4, num3);
				bool flag4 = typeFromHandle == typeof(ThreadWorkloadExecutor<T>);
				if (flag4)
				{
					array[i] = new ThreadWorkDistribution<T>(executor as ThreadWorkloadExecutor<T>, workLoad, num4, num4 + num5);
				}
				else
				{
					bool flag5 = typeFromHandle == typeof(ThreadWorkloadExecutorIndexed<T>);
					if (flag5)
					{
						array[i] = new ThreadWorkDistribution<T>(executor as ThreadWorkloadExecutorIndexed<T>, workLoad, num4, num4 + num5);
					}
					else
					{
						bool flag6 = typeFromHandle == typeof(ThreadWorkloadExecutorArg<T>);
						if (flag6)
						{
							array[i] = new ThreadWorkDistribution<T>(executor as ThreadWorkloadExecutorArg<T>, workLoad, extraArgument, num4, num4 + num5);
						}
						else
						{
							bool flag7 = typeFromHandle == typeof(ThreadWorkloadExecutorArgIndexed<T>);
							if (flag7)
							{
								array[i] = new ThreadWorkDistribution<T>(executor as ThreadWorkloadExecutorArgIndexed<T>, workLoad, extraArgument, num4, num4 + num5);
							}
						}
					}
				}
				array[i].ID = i;
				num4 += num3;
				num6 = i;
			}
			ThreadWorkDistributionSession<T> threadWorkDistributionSession = new ThreadWorkDistributionSession<T>();
			threadWorkDistributionSession.workLoad = workLoad;
			threadWorkDistributionSession.onComplete = onComplete;
			threadWorkDistributionSession.onPackageComplete = onPackageComplete;
			threadWorkDistributionSession.packages = array;
			scheduler.StartASyncThreads(array, new ThreadPoolSchedulerEvent(threadWorkDistributionSession.onCompleteBubble), new ThreadedWorkCompleteEvent(threadWorkDistributionSession.onPackageCompleteBubble), maxThreads, safeMode);
			return scheduler;
		}
	}
}
