using System;
using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;

/*UI窗体的父类
 * 功能：定义所有UI窗体的父类
 * 定义四个生命周期
 * 1.Display 显示状态
 * 2.Hiding 隐藏状态
 * 3.ReDisplay 再显示状态
 * 4.Freeze 冻结状态
 *
 */

public class BaseUIForms : MonoBehaviour
{
    //字段
    private UIType _CurrentUIType = new UIType();  //当前的UI类型

    #region 窗口的四种状态
    /// <summary>
    /// 属性  当前UI窗体类型
    /// </summary>
    public UIType CurrentUIType
    {
        get { return _CurrentUIType; }
        set { _CurrentUIType = value; }
    }

    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void Display()
    {
        this.gameObject.SetActive(true);
        //设置UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().SetMaskWindow(this.gameObject, CurrentUIType.UIForm_Luceny);
        }
    }

    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hiding()
    {
        this.gameObject.SetActive(false);

        //取消UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().CancelMaskWindow();
        }
    }

    /// <summary>
    /// 再显示状态
    /// </summary>
    public virtual void ReDisplay()
    {
        this.gameObject.SetActive(true);
        //设置UI遮罩
        if (_CurrentUIType.UIForm_Type == UIFormType.PopUp)
        {
            UIMaskManager.GetInstance().SetMaskWindow(this.gameObject, CurrentUIType.UIForm_Luceny);
        }
    }

    /// <summary>
    /// 冻结状态
    /// </summary>
    public virtual void Freeze()
    {
        this.gameObject.SetActive(true);
    }


    #endregion

    #region 封装子类常用方法
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
