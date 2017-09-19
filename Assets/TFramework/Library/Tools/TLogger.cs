using System;
using System.IO;
using System.Text;
using System.Threading;
using Tools;
using UnityEngine;
public class TLogger
{
    private static TLogger instance;
    private static TSafeQueue<string> LogBuff = null;
    private static StreamWriter file = null;
    private static Thread PrintThread = null;
    private static bool IsPlaying = false;
    public static TLogger Instance
    {
        get
        {
            bool flag = TLogger.instance == null;
            if (flag)
            {
                TLogger.instance = new TLogger();
            }
            return TLogger.instance;
        }
    }
    private TLogger()
    {
        this.Init();
    }
    private void Init()
    {
        Debug.Log("初始化日志打印!!!");
        TLogger.IsPlaying = true;
        bool flag = TLogger.file != null;
        if (flag)
        {
            TLogger.file.Close();
            TLogger.file = null;
        }
        string text = null;
        RuntimePlatform platform = Application.platform;
        bool flag2 = platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.OSXEditor;
        if (flag2)
        {
            Directory.CreateDirectory("./Log");
            text = "./Log/U3D_OutLog.text";
        }
        else
        {
            bool flag3 = platform == RuntimePlatform.WindowsPlayer;
            if (flag3)
            {

                Directory.CreateDirectory("./Log");
                text = "./Log/U3D_OutLog.text";

            }
            else
            {
                bool flag4 = platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer;
                if (flag4)
                {
                    text = Application.persistentDataPath + "/U3D_OutLog.txt";
                }
            }
        }
        bool flag5 = string.IsNullOrEmpty(text);
        if (!flag5)
        {
            bool flag6 = File.Exists(text);
            if (flag6)
            {
                File.Delete(text);
            }
            TLogger.file = new StreamWriter(text, true, Encoding.Unicode);
            TLogger.file.AutoFlush = true;
            Application.logMessageReceived += new Application.LogCallback(this.HandleLog);
            bool flag7 = TLogger.LogBuff == null;
            if (flag7)
            {
                TLogger.LogBuff = new TSafeQueue<string>();
            }
            bool flag8 = TLogger.PrintThread == null;
            if (flag8)
            {
                TLogger.PrintThread = new Thread(new ThreadStart(TLogger.Run));
                TLogger.PrintThread.IsBackground = true;
                TLogger.PrintThread.Priority = System.Threading.ThreadPriority.Lowest;
                TLogger.PrintThread.Start();
            }
        }
    }
    private void HandleLog(string condition, string stackTrace, LogType type)
    {
        TLogger.LogFile(condition);
        bool flag = type == LogType.Exception || type == LogType.Error;
        if (flag)
        {
            TLogger.LogFile(stackTrace);
        }
    }
    private static void LogFile(string info)
    {
        bool flag = TLogger.file == null;
        if (!flag)
        {
            TLogger.file.WriteLine(info);
            TLogger.file.Flush();
        }
    }
    public static void LogStr(string info, LogType type = LogType.Log)
    {
        switch (type)
        {
            case LogType.Error:
                Debug.LogError(info);
                break;
            case LogType.Assert:
                Debug.Log(info);
                break;
            case LogType.Warning:
                Debug.LogWarning(info);
                break;
            case LogType.Log:
                Debug.Log(info);
                break;
        }
    }
    private static void Run()
    {
        try
        {
            while (TLogger.IsPlaying)
            {
                bool flag = TLogger.file == null;
                if (flag)
                {
                    break;
                }
                bool flag2 = TLogger.LogBuff.Count > 0;
                if (flag2)
                {
                    TLogger.PrintAll();
                    Thread.Sleep(5);
                }
                else
                {
                    Thread.Sleep(30);
                }
            }
        }
        finally
        {
            bool flag3 = TLogger.file != null;
            if (flag3)
            {
                TLogger.file.Close();
            }
            Debug.Log("打印线程执行完毕!");
        }
    }
    private static void PrintAll()
    {
        bool flag = TLogger.LogBuff == null || TLogger.LogBuff.Count < 1;
        if (!flag)
        {
            string value = TLogger.LogBuff.Dequeue();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(value);
            while (TLogger.LogBuff.Count > 0)
            {
                value = TLogger.LogBuff.Dequeue();
                stringBuilder.AppendLine(value);
            }
            TLogger.LogFile(stringBuilder.ToString());
        }
    }
    public static void Quit()
    {
        TLogger.PrintAll();
        TLogger.IsPlaying = false;
    }
    public static void TraceInfo(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, true, LogType.Log);
    }
    public static void Log(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, false, LogType.Log);
    }
    public static void DebugStackTraceInfo(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, true, LogType.Log);
    }
    public static void WarningInfo(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, false, LogType.Warning);
    }
    public static void LogError(string msg, Type t = null)
    {
        string loggerTypeL = "";
        bool flag = t != null;
        if (flag)
        {
            loggerTypeL = t.ToString();
        }
        TLogger.ErrorInfo(msg, loggerTypeL, null);
    }
    public static void LogException(Exception e)
    {
        Debug.LogException(e);
    }
    public static void ErrorInfo(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, true, LogType.Error);
    }
    public static void DebugInfoCPlusPlus(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, true, LogType.Log);
    }
    public static void ErrorInfoCPlusPlus(string content, string loggerTypeL1 = null, string loggerTypeL2 = null)
    {
        TLogger.OutPutLog(content, loggerTypeL1, loggerTypeL2, true, LogType.Error);
    }
    private static string GetFuncStackString()
    {
        return string.Empty;
    }
    private static void OutPutLog(string msg, string loggerTypeL1 = null, string loggerTypeL2 = null, bool printfFunc = true, LogType logtype = LogType.Log)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string value = string.Concat(new object[]
        {
            "[",
            DateTime.Now.Hour,
            ":",
            DateTime.Now.Minute,
            ":",
            DateTime.Now.Second,
            "_",
            DateTime.Now.Millisecond,
            "]"
        });
        stringBuilder.Append(value);
        bool flag = !string.IsNullOrEmpty(loggerTypeL1);
        if (flag)
        {
            stringBuilder.Append("[").Append(loggerTypeL1).Append("]");
        }
        bool flag2 = !string.IsNullOrEmpty(loggerTypeL2);
        if (flag2)
        {
            stringBuilder.Append("[").Append(loggerTypeL2).Append("]");
        }
        stringBuilder.Append("\t\t");
        stringBuilder.Append(msg);
        if (printfFunc)
        {
            string funcStackString = TLogger.GetFuncStackString();
            stringBuilder.Append(funcStackString);
        }
        TLogger.LogBuff.Enqueue(stringBuilder.ToString());
    }
}
