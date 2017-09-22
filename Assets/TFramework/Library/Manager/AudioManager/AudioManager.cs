using UnityEngine;
using System.Collections;
using TFramework.Base;
using System;

/// <summary>
/// 
/// 音频播放管理器
/// 
/// 1 内部或外部加载音频播放
/// 2 两种模式 新建AudioSource或替换AudioSource中的Clip
/// 
/// </summary>

public class AudioManager : TMonoSingleton<AudioManager>
{
    public AudioManager() { }

    //初始化
    public override void Initialization(GameObject parentOBJ, bool beChild)
    {
        base.Initialization(parentOBJ, beChild);
    }

    #region Public Function

    //从AudioXml中或者Assets中加载音频 新建AudioSource
    public AudioSource NewSourceFromLocal(string clipName)
    {
        GameObject obj = new GameObject(clipName);

        AudioSource audioS = obj.AddComponent<AudioSource>();

        audioS.clip = ResourcesManager.Instance.GetAudioClipResourceByName(clipName);
        audioS.playOnAwake = false;

        return audioS;
    }

    //从AudioXml中或者Assets中加载音频 替换AudioSource中的Clip
    public AudioSource ChangeClipFromLocal(AudioSource audioS, string clipName)
    {
        audioS.Stop();
        audioS.clip = ResourcesManager.Instance.GetAudioClipResourceByName(clipName);

        return audioS;
    }

    #endregion
}
