using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Manager;

/*UI窗体的父类
 * 功能：定义所有UI窗体的父类
 * 定义四个生命周期
 * 1.Display 显示状态
 * 2.Hiding 隐藏状态
 * 3.ReDisplay 再显示状态
 * 4.Freeze 冻结状态
 *
 */

public abstract class BaseUIForms : MonoBehaviour
{
    [SerializeField]
    Button m_btnClose;
    //字段
    private UIType _CurrentUIType = new UIType();  //当前的UI类型

    private UnityAction OnCloseAction;

    #region 窗口的四种状态
    /// <summary>
    /// 属性  当前UI窗体类型
    /// </summary>
    public UIType CurrentUIType
    {
        get { return _CurrentUIType; }
        set { _CurrentUIType = value; }
    }

    public abstract UIFormType GetUIType();
    public abstract EM_WinType GetWinType();

    public virtual void OnCloseTodo()
    {
        CloseUIForm(GetWinType());
    }

    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        //设置UI遮罩
        if (GetUIType() == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().SetMaskWindow(this, CurrentUIType.UIForm_Luceny);
        }
        if (m_btnClose != null && OnCloseAction == null)
        {
            m_btnClose.onClick.AddListener(OnCloseTodo);
        }
        if (m_btnClose != null && OnCloseAction != null)
        {
            m_btnClose.onClick.AddListener(OnCloseAction);
        }
    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hiding()
    {
        //取消UI遮罩
        if (GetUIType() == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().CancelMaskWindow(this);
        }
        if (m_btnClose != null && OnCloseAction == null)
        {
            m_btnClose.onClick.RemoveListener(OnCloseTodo);
        }
        if (m_btnClose != null && OnCloseAction != null)
        {
            m_btnClose.onClick.RemoveListener(OnCloseAction);
        }
        //删除自身
        GameObject.Destroy(this.gameObject);
    }

    #endregion

    #region 封装子类常用方法
    /// <summary>
    /// 子类自定义的关闭方法
    /// </summary>
    /// <param name="func"></param>
    protected void SetCloseToDo(UnityAction func)
    {
        OnCloseAction = func;
    }

    /// <summary>
    /// 绑定监听事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    protected void BindEvent(string eventName, Notifier.StandardDelegate func)
    {
        EventMgr.GetInstance().AddEventHandle(eventName, func);
    }
    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="func"></param>
    protected void UnBindEvent(string eventName, Notifier.StandardDelegate func)
    {
        EventMgr.GetInstance().RemoveEventHandle(eventName, func);
    }

    /// <summary>
    /// 给按钮注册方法
    /// </summary>
    /// <param name="btnName">按钮名称</param>
    /// <param name="dele">委托事件</param>
    protected void RegisterButtonEvent(string btnName, EventTriggerListener.VoidDelegate dele)
    {
        Transform traLoginUISysButton = UnityHelper.FindTheChildNode(this.gameObject, btnName);

        //给按钮注册方法
        if (traLoginUISysButton != null)
        {
            //通过自己写的脚本把事件动态注册到这个按钮上
            EventTriggerListener.Get(traLoginUISysButton.gameObject).onClick = dele;
        }
    }

    /// <summary>
    /// 打开一个UI窗体
    /// </summary>
    /// <param name="winType"></param>
    protected void OpenUIForm(EM_WinType winType)
    {
        UIManager.GetInstance().ShowUIForm(winType);
    }

    /// <summary>
    /// 关闭UI窗体
    /// </summary>
    protected void CloseUIForm(EM_WinType winType)
    {
        UIManager.GetInstance().CloseUIForm(winType);
    }
    #endregion
}
