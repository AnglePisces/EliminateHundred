using System;
namespace Frankfort.Threading.Internal
{
	public class ThreadWorkDistribution<T> : IThreadWorkerObject
	{
		public int ID;
		public ThreadWorkloadExecutor<T> workloadExecutor;
		public ThreadWorkloadExecutorIndexed<T> workloadExecutorIndexed;
		public ThreadWorkloadExecutorArg<T> workloadExecutorArg;
		public ThreadWorkloadExecutorArgIndexed<T> workloadExecutorArgIndexed;
		public int startIndex;
		public int endIndex;
		public T[] workLoad;
		public object extraArgument;
		private bool _isAborted = false;
		public ThreadWorkDistribution(ThreadWorkloadExecutor<T> workloadExecutor, T[] workLoad, int startIndex, int endIndex)
		{
			this.workloadExecutor = workloadExecutor;
			this.workLoad = workLoad;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}
		public ThreadWorkDistribution(ThreadWorkloadExecutorIndexed<T> workloadExecutorIndexed, T[] workLoad, int startIndex, int endIndex)
		{
			this.workloadExecutorIndexed = workloadExecutorIndexed;
			this.workLoad = workLoad;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
		}
		public ThreadWorkDistribution(ThreadWorkloadExecutorArg<T> workloadExecutorArg, T[] workLoad, object extraArgument, int startIndex, int endIndex)
		{
			this.workloadExecutorArg = workloadExecutorArg;
			this.workLoad = workLoad;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.extraArgument = extraArgument;
		}
		public ThreadWorkDistribution(ThreadWorkloadExecutorArgIndexed<T> workloadExecutorArgIndexed, T[] workLoad, object extraArgument, int startIndex, int endIndex)
		{
			this.workloadExecutorArgIndexed = workloadExecutorArgIndexed;
			this.workLoad = workLoad;
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.extraArgument = extraArgument;
		}
		public void ExecuteThreadedWork()
		{
			bool flag = this.workLoad == null || this.workLoad.Length == 0;
			if (!flag)
			{
				bool flag2 = this.workloadExecutor != null;
				if (flag2)
				{
					int num = this.startIndex;
					while (num < this.endIndex && !this._isAborted)
					{
						UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
						this.workloadExecutor(this.workLoad[num]);
						int num2 = num;
						num = num2 + 1;
					}
				}
				else
				{
					bool flag3 = this.workloadExecutorIndexed != null;
					if (flag3)
					{
						int num3 = this.startIndex;
						while (num3 < this.endIndex && !this._isAborted)
						{
							UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
							this.workloadExecutorIndexed(this.workLoad[num3], num3);
							int num2 = num3;
							num3 = num2 + 1;
						}
					}
					else
					{
						bool flag4 = this.workloadExecutorArg != null;
						if (flag4)
						{
							int num4 = this.startIndex;
							while (num4 < this.endIndex && !this._isAborted)
							{
								UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
								this.workloadExecutorArg(this.workLoad[num4], this.extraArgument);
								int num2 = num4;
								num4 = num2 + 1;
							}
						}
						else
						{
							bool flag5 = this.workloadExecutorArgIndexed != null;
							if (flag5)
							{
								int num5 = this.startIndex;
								while (num5 < this.endIndex && !this._isAborted)
								{
									UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
									this.workloadExecutorArgIndexed(this.workLoad[num5], num5, this.extraArgument);
									int num2 = num5;
									num5 = num2 + 1;
								}
							}
						}
					}
				}
			}
		}
		public void AbortThreadedWork()
		{
			this._isAborted = true;
		}
	}
}
