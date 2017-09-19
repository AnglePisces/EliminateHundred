using System;
using System.Threading;
using UnityEngine;
namespace Frankfort.Threading.Internal
{
    public class ASyncThreadWorkData
    {
        public ThreadWorkStatePackage[] workerPackages;
        public int maxWorkingThreads;
        public ASyncThreadWorkData(IThreadWorkerObject[] workerObjects, bool safeMode, int maxWorkingThreads = -1)
        {
            bool flag = workerObjects == null;
            if (!flag)
            {
                this.workerPackages = new ThreadWorkStatePackage[workerObjects.Length];
                int num = workerObjects.Length;
                while (true)
                {
                    int num2 = num - 1;
                    num = num2;
                    if (num2 <= -1)
                    {
                        break;
                    }
                    ThreadWorkStatePackage threadWorkStatePackage = new ThreadWorkStatePackage();
                    threadWorkStatePackage.waitHandle = new AutoResetEvent(false);
                    threadWorkStatePackage.workerObject = workerObjects[num];
                    threadWorkStatePackage.safeMode = safeMode;
                    this.workerPackages[num] = threadWorkStatePackage;
                }
                bool flag2 = maxWorkingThreads <= 0;
                if (flag2)
                {
                    maxWorkingThreads = Mathf.Max(SystemInfo.processorCount - 1, 1);
                }
                else
                {
                    this.maxWorkingThreads = maxWorkingThreads;
                }
            }
        }
        public void Dispose()
        {
            bool flag = this.workerPackages != null;
            if (flag)
            {
                ThreadWorkStatePackage[] array = this.workerPackages;
                for (int i = 0; i < array.Length; i++)
                {
                    ThreadWorkStatePackage threadWorkStatePackage = array[i];
                    bool flag2 = threadWorkStatePackage.waitHandle != null;
                    if (flag2)
                    {
                        threadWorkStatePackage.waitHandle.Close();
                    }
                    threadWorkStatePackage.waitHandle = null;
                    threadWorkStatePackage.workerObject = null;
                }
            }
            this.workerPackages = null;
        }
    }
}
