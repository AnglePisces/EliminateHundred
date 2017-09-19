using System;
namespace Frankfort.Threading.Internal
{
	public class ThreadWorkDistributionSession<T>
	{
		public MultithreadedWorkloadComplete<T> onComplete;
		public MultithreadedWorkloadPackageComplete<T> onPackageComplete;
		public T[] workLoad;
		public ThreadWorkDistribution<T>[] packages;
		public void onCompleteBubble(IThreadWorkerObject[] finishedObjects)
		{
			bool flag = this.onComplete != null;
			if (flag)
			{
				this.onComplete(this.workLoad);
			}
		}
		public void onPackageCompleteBubble(IThreadWorkerObject finishedObject)
		{
			bool flag = this.onPackageComplete != null;
			if (flag)
			{
				ThreadWorkDistribution<T> threadWorkDistribution = (ThreadWorkDistribution<T>)finishedObject;
				bool flag2 = threadWorkDistribution != null;
				if (flag2)
				{
					this.onPackageComplete(threadWorkDistribution.workLoad, threadWorkDistribution.startIndex, threadWorkDistribution.endIndex - 1);
				}
			}
		}
	}
}
