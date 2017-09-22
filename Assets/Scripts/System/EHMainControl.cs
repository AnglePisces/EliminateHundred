using TFramework.System;

/// <summary>
///
///消消百人斩的入口函数
///
/// </summary>
public class EHMainControl : MainControl
{

    //程序入口
    protected override void Awake()
    {
        base.Awake();
        EHGameManager.Instance.Initialization(this.gameObject, true);

    }
}