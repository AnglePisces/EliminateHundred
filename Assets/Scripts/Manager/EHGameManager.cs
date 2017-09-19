using EHEvent;
using EventHandleModel;
using UnityEngine;

public class EHGameManager : GameManager<EHGameManager>
{

    //加载界面
    protected LoadingUI _loadingUI;

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

        _loadingUI = ResourcesManager.Instance.GetIniPrefabResourceByName("Loading").GetComponent<LoadingUI>();
        _loadingUI.Initialization();
    }
    
    //加载游戏完成
    protected void FinishLoadingGame(IEvent evt)
    {
        


    }

}
