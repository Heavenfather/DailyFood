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

    public override EM_WinType GetWinType()
    {
        return EM_WinType.AddFoodPanel;
    }

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;

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
        EventMgr.GetInstance().NotifireEvent(EventName.Event_AddOneOutFood, foodName, price, state);

        //关闭界面
        CloseUIForm(this.GetWinType());
    }


}
