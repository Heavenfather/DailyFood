using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //初始化配置
        ConfigMgr.GetInstance().InitData();

        DontDestroyOnLoad(this);
        UIManager.GetInstance().Init();

        //打开主页
        UIManager.GetInstance().ShowUIForm(EM_WinType.MainUIPanel);
    }

}
