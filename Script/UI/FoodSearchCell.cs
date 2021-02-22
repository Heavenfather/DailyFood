using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodSearchCell : MonoBehaviour
{
    [SerializeField]
    Text m_name;

    private OutFood m_data;

    public void Refresh(OutFood data)
    {
        if (data == null) return;
        m_data = data;
        m_name.text = data.V_StoreName + "(" + data.V_Star + "分)";
    }

    public void OnCheckClick()
    {
        //通用的查看出去吃数据的面板
        UIManager.GetInstance().ShowUIForm(EM_WinType.WinRandomOutFood);
        WinRandomOutFood randomPanel = UIManager.GetInstance().GetWinForm(EM_WinType.WinRandomOutFood) as WinRandomOutFood;
        if (randomPanel != null)
        {
            randomPanel.Init(m_data.V_Key);
        }
    }
}
