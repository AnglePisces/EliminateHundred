using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class ThreadWorkStatePackage
	{
		public bool safeMode = true;
		public bool started;
		public bool running;
		public bool finishedWorking;
		public bool eventFired;
		public IThreadWorkerObject workerObject;
		public AutoResetEvent waitHandle;
		public void ExecuteThreadWork(object obj)
		{
			this.running = true;
			bool flag = this.workerObject == null || this.waitHandle == null;
			if (!flag)
			{
				bool flag2 = this.safeMode;
				if (flag2)
				{
					try
					{
						this.workerObject.ExecuteThreadedWork();
					}
					catch (Exception arg_43_0)
					{
						Exception e2 = arg_43_0;
						Exception e = e2;
						Loom.DispatchToMainThread(delegate
						{
							Debug.LogError(e.Message + e.StackTrace + "\n\n");
						}, true, true);
					}
				}
				else
				{
					this.workerObject.ExecuteThreadedWork();
				}
				this.running = false;
				this.finishedWorking = true;
				this.waitHandle.Set();
			}
		}
	}
}
