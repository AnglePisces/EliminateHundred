using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 
/// 外部资源加载控制
/// 
/// </summary>

public class ExternalResourceController : MonoBehaviour
{
    //加载音频
    public IEnumerator LoadAudioEnumerator(string url)
    {
        WWW www = new WWW(@"file:///" + url);

        yield return www;

        if (www.isDone && www.error == null)
        {
            //AudioClip ac = www.audioClip;
        }
    }

    //获取指定路径图片加载到指定对象
    public IEnumerator GetTexture(string url, RawImage rawImage)
    {
        WWW www = new WWW(@"file:///" + url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            rawImage.texture = www.texture;
        }
    }
}
