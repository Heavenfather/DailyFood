using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
    }

    public void Init(OutFood food)
    {
        m_data = food;
        m_txtStoreName.text = food.V_StoreName;
        m_txtAdress.text = food.V_Adress;
        m_txtStar.text = food.V_Star.ToString();
        m_txtTotalPrice.text = food.GetTotalPrice() + "";
        string foodName = "";
        string[] goodFoods = food.V_GoodFoodName.Split(';');
        string[] badFoods = food.V_BadFoodName.Split(';');

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

}
