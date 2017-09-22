using TFramework.Base;
using UnityEngine;

public class DemoGameManager : GameManager<DemoGameManager>
{
    public DemoGameManager()
    {
    }

    //初始化
    public override void Initialization(GameObject parentOBJ,bool beChild)
    {
        base.Initialization(parentOBJ, beChild);

        Debug.Log("游戏初始化后 游戏开始");

    }

    public void Test()
    {
        
    }
}
