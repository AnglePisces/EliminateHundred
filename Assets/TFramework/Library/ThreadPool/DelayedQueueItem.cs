using System;
namespace Frankfort.Threading.Internal
{
	public struct DelayedQueueItem
	{
		public float time;
		public Action action;
	}
}
