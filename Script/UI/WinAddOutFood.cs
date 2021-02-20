using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
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
        base.BindEvent(EventName.Event_ChangeOneOutFood, ChangeOneFood);
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
        base.UnBindEvent(EventName.Event_ChangeOneOutFood, ChangeOneFood);
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
            cell.Init(m_tempId, name, price, good, this);
            m_dicFoodCells.Add(m_tempId, cell);
            m_tempId++;
        }

    }

    private void ChangeOneFood(object[] args)
    {
        if (args.Length < 3)
            return;

        string name = (string)args[0];
        string price = (string)args[1];
        bool good = (bool)args[2];
        string strState = good ? "好吃" : "不好吃";
        int tempId = (int)args[3];
        foreach (var item in m_dicFoodCells)
        {
            if (item.Key != tempId)
                continue;
            OutFoodPerCell cell = item.Value;
            cell.Refresh(name, price, good);
        }
    }

    public void DeleteOneFood(int tempId)
    {
        OutFoodPerCell cell = null;
        if (m_dicFoodCells.TryGetValue(tempId, out cell))
        {
            m_dicFoodCells.Remove(tempId);
            GameObject.Destroy(cell.gameObject);
        }
    }

    public void OnComfirmClick()
    {
        //检查参数
        string adress = m_inputAdress.text;
        if (string.IsNullOrEmpty(adress))
        {
            UnityHelper.OpenAtlerWin("地址不能为空");
            return;
        }
        string storeName = m_inputStoreName.text;
        if (string.IsNullOrEmpty(adress))
        {
            UnityHelper.OpenAtlerWin("店名不能为空");
            return;
        }
        string line = m_inputLine.text;
        string evaluate = m_inputEvaluate.text;
        float star = 0;
        float.TryParse(m_inputStar.text, out star);
        string year = m_Years[m_dropdownYear.value];
        string month = m_dropdownMonth.options[m_dropdownMonth.value].text;
        string goodFood = "";
        string badFood = "";
        foreach (var item in m_dicFoodCells)
        {
            OutFoodPerCell cell = item.Value;
            if (cell.Good)
            {
                goodFood += cell.FoodName + "-";
                goodFood += cell.Price + ";";
            }
            else
            {
                badFood += cell.FoodName + "-";
                badFood += cell.Price + ";";
            }
        }
        if (!string.IsNullOrEmpty(goodFood))
        {
            goodFood = goodFood.TrimEnd(';');
        }
        if (!string.IsNullOrEmpty(badFood))
        {
            badFood = badFood.TrimEnd(';');
        }
        UnityHelper.OpenAtlerWin("是否确定添加?", () =>
        {
            OutFoodMgr.GetInstance().Model.AddOutFood(adress, storeName, goodFood, badFood, evaluate, year + "-" + month, star, line, "");
            //关闭弹窗
            CloseUIForm(EM_WinType.PopUpWindows);
            //关闭添加菜单界面
            CloseUIForm(EM_WinType.WinAddOutFood);
        });
    }

    public void OnAddOneFoodClick()
    {
        //添加一道菜
        OpenUIForm(EM_WinType.AddFoodPanel);
    }

}
