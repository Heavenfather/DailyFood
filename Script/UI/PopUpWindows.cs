using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpWindows : BaseUIForms
{
    [SerializeField]
    Text m_contenTxt;
    [SerializeField]
    Button m_btnComfirm;
    [SerializeField]
    Button m_btnCancel;

    UnityAction m_comfirmAction = null;
    UnityAction m_cancelAction = null;

    private void Awake()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.Normal;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
    }

    public override void Hiding()
    {
        base.Hiding();
        //移除监听
        if (m_comfirmAction != null)
        {
            m_btnComfirm.onClick.RemoveListener(m_comfirmAction);
        }
        if (m_cancelAction != null)
        {
            m_btnCancel.onClick.RemoveListener(m_cancelAction);
        }
    }

    public void Init(string content, UnityAction okCallback = null, UnityAction cancelCallback = null)
    {
        m_contenTxt.text = content;
        if (okCallback != null)
        {
            m_btnComfirm.onClick.AddListener(okCallback);
            m_comfirmAction = okCallback;
        }
        else
        {
            m_btnComfirm.onClick.AddListener(OnOkClick);
        }
        if (cancelCallback != null)
        {
            m_btnCancel.onClick.AddListener(cancelCallback);
            m_cancelAction = cancelCallback;
        }
        else
        {
            m_btnCancel.onClick.AddListener(OnCancelClick);
        }
    }

    private void OnOkClick()
    {
        CloseUIForm(EM_WinType.PopUpWindows);
    }

    private void OnCancelClick()
    {
        CloseUIForm(EM_WinType.PopUpWindows);
    }

}
