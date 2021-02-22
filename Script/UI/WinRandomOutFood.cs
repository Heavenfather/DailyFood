using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Manager;
using System;

public class WinRandomOutFood : BaseUIForms
{
    [SerializeField]
    Text m_txtStoreName;
    [SerializeField]
    Text m_txtStar;
    [SerializeField]
    Text m_txtTotalPrice;
    [SerializeField]
    Text m_txtAdress;
    [SerializeField]
    Text m_txtFoods;

    OutFood m_data = null;

    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinRandomOutFood;
    }

    public override UIFormType GetUIType()
    {
        return UIFormType.PopUp;
    }

    private void Start()
    {
        BindEvent(EventName.Event_RefreshOutFoodData, Refresh);
    }

    private void OnDestroy()
    {
        UnBindEvent(EventName.Event_RefreshOutFoodData, Refresh);
    }

    private void Refresh(object[] args)
    {
        int key = (int)args[0];
        Init(key);
    }

    public void Init(int key)
    {
        //根据key来实时取数据
        m_data = OutFoodMgr.GetInstance().Model.GetOutFoodInfoByKey(key);
        m_txtStoreName.text = m_data.V_StoreName;
        m_txtAdress.text = m_data.V_Adress;
        m_txtStar.text = m_data.V_Star.ToString();
        m_txtTotalPrice.text = m_data.GetTotalPrice() + "";
        string foodName = "";
        string[] goodFoods = m_data.V_GoodFoodName.Split(';');
        string[] badFoods = m_data.V_BadFoodName.Split(';');

        for (int i = 0; i < goodFoods.Length; i++)
        {
            if (string.IsNullOrEmpty(goodFoods[i]))
                continue;
            string[] foodStr = goodFoods[i].Split('-');
            foodName += foodStr[0] + "   价格：" + foodStr[1] + "--" + "好吃" + "\n";
        }


        for (int i = 0; i < badFoods.Length; i++)
        {
            if (string.IsNullOrEmpty(badFoods[i]))
                continue;
            string[] foodStr = badFoods[i].Split('-');
            foodName += foodStr[0] + "   价格：" + foodStr[1] + "--" + "不好吃" + "\n";
        }

        m_txtFoods.text = foodName;
    }

    public void OnEvaluateClick()
    {
        UnityHelper.OpenAtlerWin(m_data.V_Evaluate);
    }

    public void OnLineClick()
    {
        UnityHelper.OpenAtlerWin(m_data.V_Line);
    }

    public void OnChangeClick()
    {
        OpenUIForm(EM_WinType.WinAddOutFood);
        WinAddOutFood panel = UIManager.GetInstance().GetWinForm(EM_WinType.WinAddOutFood) as WinAddOutFood;
        if (panel != null)
        {
            panel.Init(m_data);
        }
    }

}
