using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutFoodPerCell : MonoBehaviour
{
    [SerializeField]
    Text m_txtFoodName;

    private int m_tempId;
    private bool m_good;
    private WinAddOutFood m_panel = null;
    private string m_foodName;
    private string m_price;

    public int TempId { get => m_tempId; }
    public bool Good { get => m_good; }
    public string FoodName { get => m_foodName;}
    public string Price { get => m_price;}

    public void Init(int id, string foodName, string price, bool good, WinAddOutFood panel)
    {
        m_tempId = id;
        m_good = good;
        string strState = good ? "好吃" : "不好吃";
        m_foodName = foodName;
        m_price = price;
        string txt = "";
        txt += foodName + "-";
        txt += price + "-";
        txt += strState;
        m_txtFoodName.text = txt;
        m_panel = panel;
    }

    public void Refresh(string foodName,string price, bool good)
    {
        m_good = good;
        string strState = good ? "好吃" : "不好吃";
        m_foodName=foodName;
        m_price=price;
        string txt = "";
        txt += foodName + "-";
        txt += price + "-";
        txt += strState;
        m_txtFoodName.text = txt;
    }

    public void OnDeleteClick()
    {
        //上层删除
        if (m_panel != null)
        {
            m_panel.DeleteOneFood(m_tempId);
        }
    }

    public void OnChangeClick()
    {
        //携带消息过去打开界面
        UIManager.GetInstance().ShowUIForm(EM_WinType.AddFoodPanel);
        AddFoodPanel panel = UIManager.GetInstance().GetWinForm(EM_WinType.AddFoodPanel) as AddFoodPanel;
        if (panel != null)
        {
            string txt = m_txtFoodName.text;
            string[] datas = txt.Split('-');
            if (datas.Length < 2)
                return;

            panel.InitPanel(datas[0], datas[1], m_good, m_tempId);
        }
    }
}
