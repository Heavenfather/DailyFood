using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : BaseUIForms
{
    #region 序列化控件引用
    [SerializeField]
    Text m_logText;
    [SerializeField]
    GameObject m_logPanel;
    [SerializeField]
    Dropdown m_dropSelect;
    #endregion

    #region 变量
    List<Dropdown.OptionData> m_mainMenu = new List<Dropdown.OptionData>();

    #endregion

    #region Unity函数
    public override UIFormType GetUIType()
    {
        return UIFormType.Full;

    }

    public override EM_WinType GetWinType()
    {
        return EM_WinType.MainUIPanel;
    }

    private void Start()
    {
        // InitMenu();

        // m_dropSelect.options = m_mainMenu;

        m_logPanel.SetActive(false);
        EventMgr.GetInstance().AddEventHandle(EventName.Event_ShowPanelLog, onShowLogPanel);
    }


    private void OnDestroy()
    {
        EventMgr.GetInstance().RemoveEventHandle(EventName.Event_ShowPanelLog, onShowLogPanel);
    }

    private void Update()
    {
        //按下Esc键结束进程
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //进程结束，保存log
            if (ConfigMgr.GetInstance().V_IsOpenDebugger)
                LogMgr.GetInstance().SaveToLocal();

            //清除所有的事件监听
            EventMgr.GetInstance().ClearAllEventHandle();

            Application.Quit();
        }

    }

    #endregion

    #region 点击事件
    public void OnOutClick()
    {
        UIManager.GetInstance().ShowUIForm(EM_WinType.WinOutFoodMain);
        //关闭自身
        CloseUIForm(GetWinType());
    }
    public void OnHomeClick()
    {
        // SceneManager.LoadScene("HomeScene");
    }

    /// <summary>
    /// 下拉菜单选择
    /// </summary>
    /// <param name="value"></param>
    public void OnMenuClick(int value)
    {
        LogMgr.GetInstance().Log(LogEnum.Normal, m_mainMenu[value].text);
    }

    /// <summary>
    /// 退出
    /// </summary>
    public void OnQuitClick()
    {
        //进程结束，保存log
        if (ConfigMgr.GetInstance().V_IsOpenDebugger)
            LogMgr.GetInstance().SaveToLocal();

        //清除所有的事件监听
        EventMgr.GetInstance().ClearAllEventHandle();

        Application.Quit();
    }

    #endregion

    #region 事件响应
    private void onShowLogPanel(object[] args)
    {
        string logTxt = "";
        if (args.Length > 0)
        {
            logTxt = (string)args[0];
        }
        m_logPanel.gameObject.SetActive(true);
        m_logText.text = logTxt;
    }

    #endregion

}
