using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinOutFoodSearch : BaseUIForms
{
    [SerializeField]
    InputField m_inputWord;
    [SerializeField]
    Dropdown m_dropMonth;
    [SerializeField]
    InputField m_inputMinPrice;
    [SerializeField]
    InputField m_inputMaxPrice;
    [SerializeField]
    InputField m_inputMinStar;
    [SerializeField]
    InputField m_inputMaxStar;
    [SerializeField]
    GameObject m_goSearchCell;
    [SerializeField]
    GameObject m_goSearchCellParent;

    private List<FoodSearchCell> m_listCells = new List<FoodSearchCell>();

    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinOutFoodSearch;
    }

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
    }

    public void OnSearchClick()
    {

    }

    public void OnClearClick()
    {
        m_inputMaxPrice.text = "";
        m_inputMaxStar.text = "";
        m_inputMinPrice.text = "";
        m_inputWord.text = "";
        m_dropMonth.value = 0;
    }

}
