using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.UI;

public class AddFoodPanel : BaseUIForms
{
    [SerializeField]
    InputField m_inputFoodName;
    [SerializeField]
    InputField m_InputPrice;
    [SerializeField]
    Toggle m_toggleGood;
    [SerializeField]
    Toggle m_toggleBad;

    private bool m_isChange = false;
    private int m_foodTempId = -1;

    public override EM_WinType GetWinType()
    {
        return EM_WinType.AddFoodPanel;
    }

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
    }

    public void InitPanel(string foodName, string price, bool good, int tempID)
    {
        m_inputFoodName.text = foodName;
        m_InputPrice.text = price;
        m_toggleGood.isOn = good;
        m_toggleBad.isOn = !good;
        m_isChange = true;
        m_foodTempId = tempID;
    }

    public void OnComfirmClick()
    {
        bool state = m_toggleGood.isOn;
        string foodName = m_inputFoodName.text;
        if (string.IsNullOrEmpty(foodName))
        {
            UnityHelper.OpenAtlerWin("菜名不能为空");
            return;
        }
        string price = m_InputPrice.text;
        if (string.IsNullOrEmpty(price))
        {
            UnityHelper.OpenAtlerWin("价格不能为空");
            return;
        }
        if (!m_isChange)
            EventMgr.GetInstance().NotifireEvent(EventName.Event_AddOneOutFood, foodName, price, state);
        else
            EventMgr.GetInstance().NotifireEvent(EventName.Event_ChangeOneOutFood, foodName, price, state, m_foodTempId);

        //关闭界面
        CloseUIForm(this.GetWinType());
    }


}
