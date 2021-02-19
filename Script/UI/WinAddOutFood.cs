using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WinAddOutFood : BaseUIForms
{
    [SerializeField]
    InputField m_inputAdress;
    [SerializeField]
    InputField m_inputStoreName;
    [SerializeField]
    InputField m_inputLine;
    [SerializeField]
    InputField m_inputEvaluate;
    [SerializeField]
    InputField m_inputStar;
    [SerializeField]
    Dropdown m_dropdownYear;
    [SerializeField]
    Dropdown m_dropdownMonth;
    [SerializeField]
    GameObject m_outFoodPerCell;
    [SerializeField]
    GameObject m_outFoodPerCellParent;
    [SerializeField]
    List<string> m_Years;

    private Dictionary<int, OutFoodPerCell> m_dicFoodCells = new Dictionary<int, OutFoodPerCell>();
    /// <summary>
    /// 添加菜的临时id
    /// </summary>
    private int m_tempId = 0;

    private void Start()
    {
        base.BindEvent(EventName.Event_AddOneOutFood, AddOneFood);
        //构造年份数据
        List<Dropdown.OptionData> lst = new List<Dropdown.OptionData>();
        for (int i = 0; i < m_Years.Count; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(m_Years[i]);
            lst.Add(data);
        }
        m_dropdownYear.options = lst;
    }


    private void OnDestroy()
    {
        base.UnBindEvent(EventName.Event_AddOneOutFood, AddOneFood);
        m_tempId = 0;
        m_dicFoodCells.Clear();
    }

    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinAddOutFood;
    }

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        //刷新布局
        // LayoutRebuilder.ForceRebuildLayoutImmediate(m_outFoodPerCellParent.transform as RectTransform);
    }

    private void AddOneFood(object[] args)
    {
        if (args.Length < 2)
            return;
        string name = (string)args[0];
        string price = (string)args[1];
        bool good = (bool)args[2];
        string strState = good ? "好吃" : "不好吃";

        //直接clone一个cell出来
        GameObject go = Instantiate(m_outFoodPerCell, m_outFoodPerCellParent.transform);
        go.SetActive(true);
        OutFoodPerCell cell = go.GetComponent<OutFoodPerCell>();
        if (cell != null)
        {
            string txt = "";
            txt += name + "-";
            txt += price + "-";
            txt += strState;
            cell.Init(m_tempId, txt);
            m_dicFoodCells.Add(m_tempId, cell);
            m_tempId++;
        }

    }

    public void OnComfirmClick()
    {
        // string year = m_Years[m_dropdownYear.value];
        // string month = m_dropdownMonth.options[m_dropdownMonth.value].text;
        // Debug.Log(year);
        // Debug.Log(month);
    }

    public void OnAddOneFoodClick()
    {
        //添加一道菜
        OpenUIForm(EM_WinType.AddFoodPanel);
    }

}
