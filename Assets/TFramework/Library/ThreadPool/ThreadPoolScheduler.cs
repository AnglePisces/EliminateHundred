using Frankfort.Threading.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading
{
    public delegate void MultithreadedWorkloadComplete<T>(T[] workLoad);
    public delegate void MultithreadedWorkloadPackageComplete<T>(T[] workLoad, int firstIndex, int lastIndex);
    public delegate void ThreadDispatchDelegate();
    public delegate void ThreadDispatchDelegateArg(object arg);
    public delegate object ThreadDispatchDelegateArgReturn(object arg);
    public delegate object ThreadDispatchDelegateReturn();
    public delegate void ThreadedWorkCompleteEvent(IThreadWorkerObject finishedObject);
    public delegate void ThreadPoolSchedulerEvent(IThreadWorkerObject[] finishedObjects);


    public class ThreadPoolScheduler : MonoBehaviour
    {
        public bool DebugMode = false;
        public bool ForceToMainThread = false;
        public float WaitForSecondsTime = 0.001f;
        private bool _providerThreadBusy;
        private bool _shedularBusy;
        private bool _isAborted;
        private ASyncThreadWorkData workData;
        private Thread providerThread;
        private int workObjectIndex;
        private ThreadPoolSchedulerEvent onCompleteCallBack;
        private ThreadedWorkCompleteEvent onWorkerObjectDoneCallBack;
        private bool safeMode;
        public bool isBusy
        {
            get
            {
                return this._shedularBusy;
            }
        }
        public float Progress
        {
            get
            {
                bool flag = this.workData == null || this.workData.workerPackages == null || this.workData.workerPackages.Length == 0;
                float result;
                if (flag)
                {
                    result = 1f;
                }
                else
                {
                    int num = 0;
                    int num2 = this.workData.workerPackages.Length;
                    while (true)
                    {
                        int num3 = num2 - 1;
                        num2 = num3;
                        if (num3 <= -1)
                        {
                            break;
                        }
                        bool finishedWorking = this.workData.workerPackages[num2].finishedWorking;
                        if (finishedWorking)
                        {
                            num3 = num;
                            num = num3 + 1;
                        }
                    }
                    result = (float)num / (float)this.workData.workerPackages.Length;
                }
                return result;
            }
        }
        protected virtual void Awake()
        {
            MainThreadWatchdog.Init();
            MainThreadDispatcher.Init();
            UnityActivityWatchdog.Init();
        }
        protected virtual void OnApplicationQuit()
        {
            Debug.Log("ThreadPoolScheduler.OnApplicationQuit!");
            this.AbortASyncThreads();
        }
        protected virtual void OnDestroy()
        {
            Debug.Log("ThreadPoolScheduler.OnDestroy!");
            this.AbortASyncThreads();
        }
        public void StartASyncThreads(IThreadWorkerObject[] workerObjects, ThreadPoolSchedulerEvent onCompleteCallBack, ThreadedWorkCompleteEvent onPackageExecuted = null, int maxThreads = -1, bool safeMode = true)
        {
            bool shedularBusy = this._shedularBusy;
            if (shedularBusy)
            {
                Debug.LogError("You are trying the start a new ASync threading-process, but is still Busy!");
            }
            else
            {
                bool flag = workerObjects == null || workerObjects.Length == 0;
                if (flag)
                {
                    Debug.LogError("Please provide an Array with atleast \"IThreadWorkerObject\"-object!");
                }
                else
                {
                    this._isAborted = false;
                    this._shedularBusy = true;
                    this._providerThreadBusy = true;
                    this.onCompleteCallBack = onCompleteCallBack;
                    this.onWorkerObjectDoneCallBack = onPackageExecuted;
                    bool flag2 = !this.ForceToMainThread;
                    if (flag2)
                    {
                        base.StartCoroutine("WaitForCompletion");
                        this.workData = new ASyncThreadWorkData(workerObjects, safeMode, maxThreads);
                        this.providerThread = new Thread(new ThreadStart(this.InvokeASyncThreadPoolWork));
                        this.providerThread.Start();
                    }
                    else
                    {
                        base.StartCoroutine(this.WaitAndExecuteWorkerObjects(workerObjects));
                    }
                }
            }
        }
        private IEnumerator WaitAndExecuteWorkerObjects(IThreadWorkerObject[] workerObjects)
        {
            int num = 0;
            while (num == 0)
            {
                num++;
                yield return new WaitForEndOfFrame();
            }
            if (num != 1)
            {
                yield break;
            }
            int num2;
            for (int i = 0; i < workerObjects.Length; i = num2 + 1)
            {
                workerObjects[i].ExecuteThreadedWork();
                bool flag = this.onWorkerObjectDoneCallBack != null;
                if (flag)
                {
                    this.onWorkerObjectDoneCallBack(workerObjects[i]);
                }
                num2 = i;
            }
            this._shedularBusy = false;
            this._providerThreadBusy = false;
            bool flag2 = this.onCompleteCallBack != null;
            if (flag2)
            {
                this.onCompleteCallBack(workerObjects);
            }
            yield break;
        }
        private IEnumerator WaitForCompletion()
        {
            while (true)
            {
                int num = 0;
                if (num != 0)
                {
                    if (num != 1)
                    {
                        break;
                    }
                    bool isAborted = this._isAborted;
                    if (isAborted)
                    {
                        goto Block_4;
                    }
                    int finishedPackagesCount = this.GetFinishedPackagesCount();
                    bool flag = finishedPackagesCount == this.workData.workerPackages.Length;
                    if (flag)
                    {
                        goto Block_5;
                    }
                    int unhandledFinishedPackagesCount = this.GetUnhandledFinishedPackagesCount();
                    bool debugMode = this.DebugMode;
                    if (debugMode)
                    {
                        Debug.Log(string.Concat(new object[]
                        {
                            " ----- unhandledPackages: ",
                            unhandledFinishedPackagesCount,
                            " ( out of: ",
                            finishedPackagesCount,
                            " completed so far...)"
                        }));
                    }
                    bool flag2 = unhandledFinishedPackagesCount > 0;
                    if (flag2)
                    {
                        ThreadWorkStatePackage[] array = this.workData.workerPackages;
                        for (int i = 0; i < array.Length; i++)
                        {
                            ThreadWorkStatePackage threadWorkStatePackage = array[i];
                            bool flag3 = threadWorkStatePackage.finishedWorking && !threadWorkStatePackage.eventFired;
                            if (flag3)
                            {
                                bool flag4 = this.onWorkerObjectDoneCallBack != null;
                                if (flag4)
                                {
                                    this.onWorkerObjectDoneCallBack(threadWorkStatePackage.workerObject);
                                }
                                threadWorkStatePackage.eventFired = true;
                            }
                            threadWorkStatePackage = null;
                        }
                        array = null;
                    }
                }
                else
                {
                    bool debugMode2 = this.DebugMode;
                    if (debugMode2)
                    {
                        num++;
                        Debug.Log(" ----- WaitForCompletion: " + Thread.CurrentThread.ManagedThreadId);
                    }
                }
                if (this._isAborted)
                {
                    goto IL_21E;
                }
                yield return new WaitForSeconds(this.WaitForSecondsTime);
            }
            yield break;
            Block_4:
            Block_5:
            IL_21E:
            bool flag5 = !this._isAborted;
            if (flag5)
            {
                bool debugMode3 = this.DebugMode;
                if (debugMode3)
                {
                    Debug.Log(" ----- Coroutine knows its done!");
                }
                IThreadWorkerObject[] finishedObjects = this.GetWorkerObjectsFromPackages();
                this.workData.Dispose();
                this.workData = null;
                this._shedularBusy = false;
                bool flag6 = this.onCompleteCallBack != null;
                if (flag6)
                {
                    this.onCompleteCallBack(finishedObjects);
                }
                finishedObjects = null;
            }
            yield break;
        }
        public void AbortASyncThreads()
        {
            bool flag = !this._providerThreadBusy;
            if (!flag)
            {
                this._isAborted = true;
                base.StopCoroutine("WaitForCompletion");
                bool flag2 = this.workData != null && this.workData.workerPackages != null;
                if (flag2)
                {
                    ThreadWorkStatePackage[] workerPackages = this.workData.workerPackages;
                    Monitor.Enter(workerPackages);
                    try
                    {
                        ThreadWorkStatePackage[] workerPackages2 = this.workData.workerPackages;
                        for (int i = 0; i < workerPackages2.Length; i++)
                        {
                            ThreadWorkStatePackage threadWorkStatePackage = workerPackages2[i];
                            bool flag3 = threadWorkStatePackage.running && !threadWorkStatePackage.finishedWorking;
                            if (flag3)
                            {
                                threadWorkStatePackage.workerObject.AbortThreadedWork();
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(workerPackages);
                    }
                }
                bool flag4 = this.providerThread != null && this.providerThread.IsAlive;
                if (flag4)
                {
                    Debug.Log("ThreadPoolScheduler.AbortASyncThreads - Interrupt!");
                    this.providerThread.Interrupt();
                    this.providerThread.Join();
                }
                else
                {
                    Debug.Log("ThreadPoolScheduler.AbortASyncThreads!");
                }
                this._providerThreadBusy = false;
            }
        }
        public void InvokeASyncThreadPoolWork()
        {
            UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
            int num = this.workData.workerPackages.Length;
            int num2 = Mathf.Clamp(this.workData.maxWorkingThreads, 1, num);
            bool debugMode = this.DebugMode;
            if (debugMode)
            {
                Debug.Log(string.Concat(new object[]
                {
                    " ----- InvokeASyncThreadPoolWork. startBurst: ",
                    num2,
                    ", totalWork: ",
                    num
                }));
            }
            int num3 = 0;
            while (num3 < num2 && !this._isAborted)
            {
                bool flag = this.workData.workerPackages[num3] != null;
                if (flag)
                {
                    this.workData.workerPackages[num3].started = true;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(this.workData.workerPackages[num3].ExecuteThreadWork), num3);
                }
                int num4 = num3;
                num3 = num4 + 1;
            }
            bool debugMode2 = this.DebugMode;
            if (debugMode2)
            {
                Debug.Log(" ----- Burst with WorkerObjects being processed!");
            }
            this.workObjectIndex = num2;
            while (this.workObjectIndex < num && !this._isAborted)
            {
                UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
                AutoResetEvent[] startedPackageEvents = this.GetStartedPackageEvents();
                bool flag2 = startedPackageEvents.Length > 0;
                if (flag2)
                {
                    WaitHandle.WaitAny(startedPackageEvents);
                }
                this.workData.workerPackages[this.workObjectIndex].started = true;
                ThreadPool.QueueUserWorkItem(new WaitCallback(this.workData.workerPackages[this.workObjectIndex].ExecuteThreadWork), this.workObjectIndex);
                int num4 = this.workObjectIndex;
                this.workObjectIndex = num4 + 1;
            }
            bool debugMode3 = this.DebugMode;
            if (debugMode3)
            {
                Debug.Log(" ----- all packages fed to the pool!");
            }
            AutoResetEvent[] startedPackageEvents2 = this.GetStartedPackageEvents();
            bool flag3 = startedPackageEvents2.Length > 0;
            if (flag3)
            {
                UnityActivityWatchdog.SleepOrAbortIfUnityInactive();
                WaitHandle.WaitAll(startedPackageEvents2);
            }
            bool debugMode4 = this.DebugMode;
            if (debugMode4)
            {
                Debug.Log(" ----- PROVIDER THREAD DONE");
            }
            this._providerThreadBusy = false;
        }
        private AutoResetEvent[] GetStartedPackageEvents()
        {
            List<AutoResetEvent> list = new List<AutoResetEvent>();
            int num;
            for (int i = 0; i < this.workData.workerPackages.Length; i = num + 1)
            {
                ThreadWorkStatePackage threadWorkStatePackage = this.workData.workerPackages[i];
                bool flag = threadWorkStatePackage.started && !threadWorkStatePackage.finishedWorking;
                if (flag)
                {
                    list.Add(threadWorkStatePackage.waitHandle);
                }
                num = i;
            }
            return list.ToArray();
        }
        private IThreadWorkerObject[] GetWorkerObjectsFromPackages()
        {
            bool flag = this.workData == null || this.workData.workerPackages == null;
            IThreadWorkerObject[] result;
            if (flag)
            {
                result = new IThreadWorkerObject[0];
            }
            else
            {
                IThreadWorkerObject[] array = new IThreadWorkerObject[this.workData.workerPackages.Length];
                int num = this.workData.workerPackages.Length;
                while (true)
                {
                    int num2 = num - 1;
                    num = num2;
                    if (num2 <= -1)
                    {
                        break;
                    }
                    array[num] = this.workData.workerPackages[num].workerObject;
                }
                result = array;
            }
            return result;
        }
        public int GetFinishedPackagesCount()
        {
            bool flag = this.workData == null || this.workData.workerPackages == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                int num = 0;
                int num2 = this.workData.workerPackages.Length;
                while (true)
                {
                    int num3 = num2 - 1;
                    num2 = num3;
                    if (num3 <= -1)
                    {
                        break;
                    }
                    bool finishedWorking = this.workData.workerPackages[num2].finishedWorking;
                    if (finishedWorking)
                    {
                        num3 = num;
                        num = num3 + 1;
                    }
                }
                result = num;
            }
            return result;
        }
        public int GetUnhandledFinishedPackagesCount()
        {
            bool flag = this.workData == null || this.workData.workerPackages == null;
            int result;
            if (flag)
            {
                result = 0;
            }
            else
            {
                int num = 0;
                int num2 = this.workData.workerPackages.Length;
                while (true)
                {
                    int num3 = num2 - 1;
                    num2 = num3;
                    if (num3 <= -1)
                    {
                        break;
                    }
                    bool flag2 = this.workData.workerPackages[num2].finishedWorking && !this.workData.workerPackages[num2].eventFired;
                    if (flag2)
                    {
                        num3 = num;
                        num = num3 + 1;
                    }
                }
                result = num;
            }
            return result;
        }
    }
}
