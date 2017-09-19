using System;
namespace Frankfort.Threading
{
	public interface IThreadWorkerObject
	{
		void ExecuteThreadedWork();
		void AbortThreadedWork();
	}
}
