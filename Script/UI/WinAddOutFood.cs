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

    private List<GameObject> m_foodCells = new List<GameObject>();

    private void Start()
    {
        //构造年份数据
        List<Dropdown.OptionData> lst = new List<Dropdown.OptionData>();
        for (int i = 0; i < m_Years.Count; i++)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(m_Years[i]);
            lst.Add(data);
        }
        m_dropdownYear.options = lst;
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

    public void Clear()
    {
        m_inputAdress.text = "";
        m_inputEvaluate.text = "";
        m_inputLine.text = "";
        m_inputStar.text = "";
        m_inputStoreName.text = "";
        // if (m_foodCells.Count > 0)
        // {
        //     for (int i = 0; i < m_foodCells.Count; i++)
        //     {
        //         GameObject.Destroy(m_foodCells[i]);
        //     }
        //     m_foodCells.Clear();
        // }
    }

}
