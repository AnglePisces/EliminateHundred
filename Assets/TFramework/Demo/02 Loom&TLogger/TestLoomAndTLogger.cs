using UnityEngine;
using System.Collections;

public class TestLoomAndTLogger : MonoBehaviour
{

    void Start()
    {
        TLogger.Log("测试输出一个Log~~~", "TestLoomAndTLogger", "Start");

        Loom.StartSingleThread(() =>
        {
            TLogger.Log("测试输出StartSingleThread线程~~~", "TestLoomAndTLogger", "Loom -- StartSingleThread");
        });

        Loom.QueueOnMainThread(() =>
        {
            TLogger.Log("测试输出QueueOnMainThread线程~~~", "TestLoomAndTLogger", "Loom -- QueueOnMainThread");
        });
    }
}
