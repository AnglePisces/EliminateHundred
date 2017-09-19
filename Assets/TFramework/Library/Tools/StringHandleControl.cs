using UnityEngine;
using System.Collections;
using System;
using System.Text;

/// <summary>
/// 
///字符串处理控制
///
/// </summary>

public class StringHandleControl
{
    //从字符串中 根据指定的字符 获取每段字符
    public static string[] CutStringByCharacter(string cutString, char cutFlag)
    {
        string[] sArray = cutString.Split(new char[] { cutFlag });

        return sArray;
    }

    //根据指定分隔符 将指定字符串数组拼接为一个字符串
    public static string LinkStringArrayByCharacter(string[] stringArray, char cutFlag)
    {
        string linkString = string.Empty;

        for (int i = 0; i < stringArray.Length - 1; i++)
        {
            linkString += stringArray[i] + cutFlag;
        }
        linkString += stringArray[stringArray.Length - 1];

        return linkString;
    }

    //将string转成vector3
    public static Vector3 MakeStringToVector3(string str, char cutFlag)
    {
        string[] strArray = CutStringByCharacter(str, cutFlag);

        Vector3 vc3 = new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));

        return vc3;
    }

    //将vector3转成string
    public static string MakeVector3ToString(Vector3 vc3, char cutFlag)
    {
        string str = vc3.x.ToString() + cutFlag + vc3.y.ToString() + cutFlag + vc3.z.ToString();

        return str;
    }

    //字符转ASCII码：
    public static int Asc(string character)
    {
        if (character.Length == 1)
        {
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            int intAsciiCode = (int)asciiEncoding.GetBytes(character)[0];
            return (intAsciiCode);
        }
        else
        {
            throw new Exception("Character is not valid.");
        }

    }

    //将传入信息以C#调试信息方式输出
    public static void DebugLogLine(string log)
    {
        Console.WriteLine(log);
    }

    //去除(clone)后缀
    /// <summary>
    /// 去除(clone)后缀
    /// </summary>
    /// <param name="str">要去除的字符串</param>
    /// <returns>已经去除的字符串</returns>
    public static string RemoveCloneString(string str)
    {
        str = str.Replace("(Clone)","");

        return str;
    }

    /// <summary>
    /// 将string转换为Base64
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string StringToBase64(string data)
    {
        byte[] array = System.Text.Encoding.Default.GetBytes(data);
        return Convert.ToBase64String(array);
    }

    /// <summary>
    /// 将Base64转换为string
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string Base64ToString(string data)
    {
        byte[] outputb = Convert.FromBase64String(data);
        return Encoding.Default.GetString(outputb);
    }
}
