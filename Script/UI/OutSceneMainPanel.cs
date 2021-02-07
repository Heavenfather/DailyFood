using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutSceneMainPanel : MonoBehaviour
{
    #region 序列化控件引用
    [SerializeField]
    GameObject m_addPanel;
    [SerializeField]
    InputField m_inputAdress;
    [SerializeField]
    InputField m_inputStore;
    [SerializeField]
    InputField m_inputFood;

    #endregion

    #region 变量
    private string m_strInputAdress;
    private string m_strInputStore;
    private string m_strInputFood;

    private string m_strAddPic;

    #endregion

    #region Unity函数
    private void Start()
    {

    }


    private void OnDestroy()
    {

    }

    private void Update()
    {

    }

    #endregion

    #region 初始化

    private void ClearInput()
    {
        m_strInputAdress = "";
        m_strInputFood = "";
        m_strInputStore = "";
        m_addPanel.SetActive(false);
    }

    #endregion

    #region 点击事件
    /// <summary>
    /// 确定添加
    /// </summary>
    public void OnAddPanelComfirm()
    {
        m_strInputAdress = m_inputAdress.text;
        m_strInputFood = m_inputFood.text;
        m_strInputStore = m_inputStore.text;

        if (string.IsNullOrEmpty(m_strInputAdress) || string.IsNullOrEmpty(m_strInputFood) || string.IsNullOrEmpty(m_strInputStore))
        {
            LogMgr.GetInstance().Log(LogEnum.Error, "添加出去吃的菜单不能有一项是空的");
            return;
        }

        ClearInput();
    }
    /// <summary>
    /// 取消添加
    /// </summary>
    public void OnAddPanelCancel()
    {
        ClearInput();
    }

    /// <summary>
    /// 随机
    /// </summary>
    public void OnRandomClick()
    {

    }

    /// <summary>
    /// 返回上一页
    /// </summary>
    public void OnReturnClick()
    {
        SceneManager.LoadScene("CaiScene");
    }

    /// <summary>
    /// 添加菜品
    /// </summary>
    public void OnAddClick()
    {
        m_addPanel.SetActive(true);
    }

    #endregion

    #region 事件响应

    #endregion

}
