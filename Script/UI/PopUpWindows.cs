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

    public override UIFormType GetUIType()
    {
        return UIFormType.PopUp;
    }

    public override EM_WinType GetWinType()
    {
        return EM_WinType.PopUpWindows;
    }

    public override void Hiding()
    {
        //移除监听
        if (m_comfirmAction != null)
        {
            m_btnComfirm.onClick.RemoveListener(m_comfirmAction);
        }
        if (m_cancelAction != null)
        {
            m_btnCancel.onClick.RemoveListener(m_cancelAction);
        }
        base.Hiding();
    }

    public void Init(string content, UnityAction okCallback = null, UnityAction cancelCallback = null)
    {
        m_contenTxt.text = content;
        if (okCallback != null)
        {
            m_btnComfirm.onClick.AddListener(okCallback);
            m_btnComfirm.onClick.AddListener(OnOkClick);
            m_comfirmAction = okCallback;
        }
        else
        {
            m_btnComfirm.onClick.AddListener(OnOkClick);
        }
        if (cancelCallback != null)
        {
            m_btnCancel.onClick.AddListener(cancelCallback);
            m_btnCancel.onClick.AddListener(OnCancelClick);
            m_cancelAction = cancelCallback;
        }
        else
        {
            m_btnCancel.onClick.AddListener(OnCancelClick);
        }
    }

    private void OnOkClick()
    {
        CloseUIForm(GetWinType());
    }

    private void OnCancelClick()
    {
        CloseUIForm(GetWinType());
    }

}
