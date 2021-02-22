using System.Collections;
using System.Collections.Generic;
using Manager;
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
    [SerializeField]
    GameObject m_nullTips;

    private List<FoodSearchCell> m_listCells = new List<FoodSearchCell>();
    private List<OutFood> m_curSearchOutFood = new List<OutFood>();

    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinOutFoodSearch;
    }

    public override UIFormType GetUIType()
    {
        return UIFormType.PopUp;
    }

    private void Start()
    {
        //初始创建所有的数据出来
        List<OutFood> lst = new List<OutFood>();
        Dictionary<int, OutFood> foods = OutFoodMgr.GetInstance().Model.GetAllOutFood();
        foreach (var item in foods)
        {
            lst.Add(item.Value);
        }
        m_curSearchOutFood = lst;
        RefreshCells(lst);

    }

    public void OnSearchClick()
    {
        if (m_curSearchOutFood.Count > 0)
            m_curSearchOutFood.Clear();
        string word = m_inputWord.text;
        if (!string.IsNullOrEmpty(word))
        {
            m_curSearchOutFood.AddRange(OutFoodMgr.GetInstance().Model.GetOutFoodsByName(word));
        }
        if (m_dropMonth.value != 0)
        {
            CheckRepeat(OutFoodMgr.GetInstance().Model.GetDatasByDate(m_dropMonth.value));
        }
        //价格区间的处理
        if (!string.IsNullOrEmpty(m_inputMinPrice.text) || !string.IsNullOrEmpty(m_inputMaxPrice.text))
        {
            float price = 0;
            List<OutFood> foods = null;
            //只填了最低值
            if (!string.IsNullOrEmpty(m_inputMinPrice.text) && string.IsNullOrEmpty(m_inputMaxPrice.text))
            {
                price = float.Parse(m_inputMinPrice.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByPriceArea(price, -1);
                CheckRepeat(foods);
            }
            else if (string.IsNullOrEmpty(m_inputMinPrice.text) && !string.IsNullOrEmpty(m_inputMaxPrice.text))
            {
                //只填了最大值
                price = float.Parse(m_inputMaxPrice.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByPriceArea(-1, price);
                CheckRepeat(foods);
            }
            else
            {
                price = float.Parse(m_inputMinPrice.text);
                float maxPrice = float.Parse(m_inputMaxPrice.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByPriceArea(price, maxPrice);
                CheckRepeat(foods);
            }
        }
        //评分区间的处理
        if (!string.IsNullOrEmpty(m_inputMinStar.text) || !string.IsNullOrEmpty(m_inputMaxStar.text))
        {
            float star = 0;
            List<OutFood> foods = null;
            //只填了最低值
            if (!string.IsNullOrEmpty(m_inputMinStar.text) && string.IsNullOrEmpty(m_inputMaxStar.text))
            {
                star = float.Parse(m_inputMinStar.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByStarArea(star, -1);
                CheckRepeat(foods);
            }
            else if (string.IsNullOrEmpty(m_inputMinStar.text) && !string.IsNullOrEmpty(m_inputMaxStar.text))
            {
                //只填了最大值
                star = float.Parse(m_inputMaxStar.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByStarArea(-1, star);
                CheckRepeat(foods);
            }
            else
            {
                star = float.Parse(m_inputMinStar.text);
                float maxStar = float.Parse(m_inputMaxStar.text);
                foods = OutFoodMgr.GetInstance().Model.GetOutFoodsByStarArea(star, maxStar);
                CheckRepeat(foods);
            }
        }
        RefreshCells(m_curSearchOutFood);
    }

    private void RefreshCells(List<OutFood> datas)
    {
        m_nullTips.SetActive(datas.Count <= 0);

        for (int i = 0; i < datas.Count; i++)
        {
            if (i >= m_listCells.Count)
            {
                GameObject go = Instantiate(m_goSearchCell, m_goSearchCellParent.transform);
                go.SetActive(true);
                FoodSearchCell cell = go.GetComponent<FoodSearchCell>();
                if (cell != null)
                {
                    m_listCells.Add(cell);
                }
            }
            FoodSearchCell searchCell = m_listCells[i];
            if (searchCell != null)
            {
                if (!searchCell.gameObject.activeInHierarchy)
                    searchCell.gameObject.SetActive(true);
                searchCell.Refresh(datas[i]);
            }
        }
        //隐藏多余的
        for (int i = datas.Count; i < m_listCells.Count; i++)
        {
            m_listCells[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 检查当前的搜索当中是否已有重复项
    /// </summary>
    /// <param name="foods"></param>
    /// <returns></returns>
    private void CheckRepeat(List<OutFood> foods)
    {
        for (int i = 0; i < foods.Count; i++)
        {
            bool isRepeat = false;
            for (int j = 0; j < m_curSearchOutFood.Count; j++)
            {
                if (m_curSearchOutFood[j].V_Key == foods[i].V_Key)
                {
                    isRepeat = true;
                    break;
                }
            }
            if (!isRepeat)
            {
                m_curSearchOutFood.Add(foods[i]);
            }
        }
    }

    public void OnClearClick()
    {
        m_inputMaxPrice.text = "";
        m_inputMaxStar.text = "";
        m_inputMinPrice.text = "";
        m_inputWord.text = "";
        m_dropMonth.value = 0;
        for (int i = 0; i < m_listCells.Count; i++)
        {
            m_listCells[i].gameObject.SetActive(false);
        }
        m_curSearchOutFood.Clear();
    }

}
