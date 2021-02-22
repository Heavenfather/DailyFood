using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    
    void Start()
    {
        //初始化配置
        ConfigMgr.GetInstance().InitData();

        DontDestroyOnLoad(this);
        //初始化数据
        UIManager.GetInstance().Init();
        OutFoodMgr.GetInstance().Model.Init();
        HomeFoodMgr.GetInstance().Model.Init();
        OpenFile.GetInstance().Init();

        //打开主页
        UIManager.GetInstance().ShowUIForm(EM_WinType.MainUIPanel);
    }

}
