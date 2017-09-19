using System;
using System.Collections.Generic;
using System.Threading;
namespace Tools
{
	public class TSafeQueue<T>
	{
		private Queue<T> list = new Queue<T>();
		public int Count
		{
			get
			{
				Queue<T> obj = this.list;
				Monitor.Enter(obj);
				int count;
				try
				{
					count = this.list.Count;
				}
				finally
				{
					Monitor.Exit(obj);
				}
				return count;
			}
		}
		public void Clear()
		{
			Queue<T> obj = this.list;
			Monitor.Enter(obj);
			try
			{
				this.list.Clear();
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public T Dequeue()
		{
			Queue<T> obj = this.list;
			Monitor.Enter(obj);
			T result;
			try
			{
				result = this.list.Dequeue();
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return result;
		}
		public void Enqueue(T t)
		{
			Queue<T> obj = this.list;
			Monitor.Enter(obj);
			try
			{
				this.list.Enqueue(t);
			}
			finally
			{
				Monitor.Exit(obj);
			}
		}
		public T Peek()
		{
			Queue<T> obj = this.list;
			Monitor.Enter(obj);
			T result;
			try
			{
				result = this.list.Peek();
			}
			finally
			{
				Monitor.Exit(obj);
			}
			return result;
		}
		public void DestroySelf()
		{
			bool flag = this.list != null;
			if (flag)
			{
				this.Clear();
				this.list = null;
			}
		}
	}
}
