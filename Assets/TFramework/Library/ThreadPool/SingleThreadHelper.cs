using System;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class SingleThreadHelper : MonoBehaviour
	{
		private void OnApplicationQuit()
		{
			SingleThreadStarter.AbortRunningThreads();
		}
	}
}
