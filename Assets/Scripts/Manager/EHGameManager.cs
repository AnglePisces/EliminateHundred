using EHEvent;
using EventHandleModel;
using UnityEngine;

/// <summary>
/// 
/// 游戏管理
/// 
/// </summary>

public class EHGameManager : GameManager<EHGameManager>
{

    //加载界面
    protected LoadingUI _loadingUI;
    //登录界面
    protected LoginUI _loginUI;
    //主界面
    protected MainUI _mainUI;

    //初始化
    public override void Initialization(GameObject parentOBJ)
    {
        base.Initialization(parentOBJ);

        //初始化完成 启动加载界面
        LoadingGame();
    }

    //初始化游戏 加载起始界面
    protected void LoadingGame()
    {
        //添加监听
        EventCenter.Instance.AddEventListenerPermanently((int)EHGameProcessEventID.Process_Loading_Event, FinishLoadingGame);

        _loadingUI = ResourcesManager.Instance.GetIniPrefabResourceByName("LoadingUI").AddComponent<LoadingUI>();
        _loadingUI.Initialization();
    }

    //加载游戏完成
    protected void FinishLoadingGame(IEvent evt)
    {

        EHLoadingEvent e = evt as EHLoadingEvent;

        if (!e._state)
        {
            //游戏加载失败
        }

        Destroy(_loadingUI.gameObject);
        _loadingUI = null;

        _loginUI = ResourcesManager.Instance.GetIniPrefabResourceByName("LoginUI").AddComponent<LoginUI>();
        _loginUI.Initialization();

        EventCenter.Instance.RemoveEventListenerPermanently((int)EHGameProcessEventID.Process_Loading_Event, FinishLoadingGame);
    }

    //进入游戏主界面
    public void IntoMainUI()
    {
        Destroy(_loginUI.gameObject);
        _loginUI = null;

        _mainUI = ResourcesManager.Instance.GetIniPrefabResourceByName("MainUI").AddComponent<MainUI>();
        _mainUI.Initialization();
    }

}
