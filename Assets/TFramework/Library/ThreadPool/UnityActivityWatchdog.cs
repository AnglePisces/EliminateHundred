using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
	public class UnityActivityWatchdog : MonoBehaviour
	{
		private static bool helperCreated = false;
		private static bool unityRunning = true;
		private static bool unityPaused = false;
		private static bool unityFocused = true;
		private static bool combinedActive = true;
		public static bool CheckUnityRunning()
		{
			return UnityActivityWatchdog.unityRunning;
		}
		public static bool CheckUnityActive()
		{
			UnityActivityWatchdog.Init();
			return UnityActivityWatchdog.combinedActive;
		}
		public static void SleepOrAbortIfUnityInactive()
		{
			UnityActivityWatchdog.Init();
			while (!UnityActivityWatchdog.combinedActive && !MainThreadWatchdog.CheckIfMainThread())
			{
				bool flag = UnityActivityWatchdog.unityRunning;
				if (flag)
				{
					Thread.Sleep(100);
				}
				else
				{
					Thread.CurrentThread.Interrupt();
					Thread.CurrentThread.Join();
				}
			}
		}
		public static void Init()
		{
			bool flag = !UnityActivityWatchdog.helperCreated;
			if (flag)
			{
				UnityActivityWatchdog.CreateHelperGameObject();
			}
		}
		private static void CreateHelperGameObject()
		{
			GameObject gameObject = new GameObject("UnityActivityHelper");
			gameObject.AddComponent<UnityActivityWatchdog>();
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
			UnityActivityWatchdog.helperCreated = true;
		}
		private void OnApplicationQuit()
		{
			UnityActivityWatchdog.unityRunning = false;
			UnityActivityWatchdog.UpdateStatus();
		}
		private void OnApplicationPause(bool pause)
		{
			UnityActivityWatchdog.unityPaused = pause;
			UnityActivityWatchdog.UpdateStatus();
		}
		private void OnApplicationFocus(bool focus)
		{
			UnityActivityWatchdog.unityFocused = focus;
			UnityActivityWatchdog.UpdateStatus();
		}
		private static void UpdateStatus()
		{
			UnityActivityWatchdog.combinedActive = (UnityActivityWatchdog.unityRunning && !UnityActivityWatchdog.unityPaused);
		}
	}
}
