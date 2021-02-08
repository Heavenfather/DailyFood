using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

public class OutFoodModel : Model
{
    /// <summary>
    /// 出去吃的所有集合
    /// </summary>
    /// <typeparam name="int"></typeparam>
    /// <typeparam name="OutFood"></typeparam>
    /// <returns></returns>
    private Dictionary<int, OutFood> m_dicOutFood = new Dictionary<int, OutFood>();
    /// <summary>
    /// 缓存搜索结果
    /// </summary>
    /// <returns></returns>
    private Dictionary<string, List<OutFood>> m_dicFuzzyChecked = new Dictionary<string, List<OutFood>>();
    /// <summary>
    /// 出去吃的菜单最大索引值
    /// </summary>
    public int V_OutFoodMaxIndex = 0;

    /// <summary>
    /// 出去吃的json数据
    /// </summary>
    private JsonData m_outFoodJsonData = null;
    public JsonData OutFoodJsonData { get => m_outFoodJsonData; }

    public override void Init()
    {
        base.Init();
        JsonData datas = JsonParse.ParseToJsonData(SysDefine.OutFoodJsonName);
        if (datas == null) return;
        V_OutFoodMaxIndex = datas.Keys.Count;
        for (int i = 0; i < datas.Keys.Count; i++)
        {
            int key = i + 1;
            string adress = datas[i][OutFoodJsonEm.Adress.ToString()].ToString();
            string storeName = datas[i][OutFoodJsonEm.StoreName.ToString()].ToString();
            string goodFoodName = datas[i][OutFoodJsonEm.GoodFoodName.ToString()].ToString();
            string badFoodName = datas[i][OutFoodJsonEm.BadFoodName.ToString()].ToString();
            string evaluate = datas[i][OutFoodJsonEm.Evaluate.ToString()].ToString();
            string date = datas[i][OutFoodJsonEm.Date.ToString()].ToString();
            float star = (float)datas[i][OutFoodJsonEm.Star.ToString()];
            string image = datas[i][OutFoodJsonEm.Image.ToString()].ToString();
            OutFood food = new OutFood(key, adress, storeName, goodFoodName, badFoodName, evaluate, date, star, image);
            if (!m_dicOutFood.ContainsKey(key))
                m_dicOutFood.Add(key, food);
        }
    }

    public override void Release()
    {
        base.Release();
        V_OutFoodMaxIndex = 0;
        m_dicOutFood.Clear();
        m_dicFuzzyChecked.Clear();
        m_outFoodJsonData = null;
    }


    /// <summary>
    /// 通过key得到出去吃的信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public OutFood GetOutFoodInfoByKey(int key)
    {
        OutFood food = null;
        if (m_dicOutFood.TryGetValue(key, out food))
        {
            return food;
        }
        return null;
    }

    /// <summary>
    /// 通过关键字，得到可能有多个的数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<OutFood> GetOutFoodsByName(string name)
    {
        //之前检索过，从缓存里直接返回
        if (m_dicFuzzyChecked.ContainsKey(name))
        {
            return m_dicFuzzyChecked[name];
        }
        List<OutFood> lst = new List<OutFood>();

        foreach (var item in m_dicOutFood)
        {
            bool b1 = UnityHelper.FuzzyCheck(name, item.Value.V_Adress);
            bool b2 = UnityHelper.FuzzyCheck(name, item.Value.V_GoodFoodName);
            bool b3 = UnityHelper.FuzzyCheck(name, item.Value.V_StoreName);
            bool b4 = UnityHelper.FuzzyCheck(name, item.Value.V_BadFoodName);
            if (b1 || b2 || b3 || b4)
            {
                lst.Add(item.Value);
            }
        }
        if (lst.Count > 0)
        {
            if (!m_dicFuzzyChecked.ContainsKey(name))
                m_dicFuzzyChecked.Add(name, lst);
        }
        return lst;
    }

    /// <summary>
    /// 添加一条出去吃的数据
    /// </summary>
    /// <param name="adress"></param>
    /// <param name="storeName"></param>
    /// <param name="foodName"></param>
    /// <param name="image"></param>
    public void AddOutFood(string adress, string storeName, string goodFoodName, string badFoodName, string evaluate, string date, float star, string image)
    {
        //添加进来一条数据 索引加1
        V_OutFoodMaxIndex += 1;

        OutFood food = new OutFood(V_OutFoodMaxIndex, adress, storeName, goodFoodName, badFoodName, evaluate, date, star, image);
        if (!m_dicOutFood.ContainsKey(V_OutFoodMaxIndex))
            m_dicOutFood.Add(V_OutFoodMaxIndex, food);
        //写入json数据
        if (m_outFoodJsonData != null)
        {
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()] = new JsonData();
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.Adress.ToString()] = adress;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.StoreName.ToString()] = storeName;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.GoodFoodName.ToString()] = goodFoodName;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.BadFoodName.ToString()] = badFoodName;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.Evaluate.ToString()] = evaluate;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.Date.ToString()] = date;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.Star.ToString()] = star;
            m_outFoodJsonData[V_OutFoodMaxIndex.ToString()][OutFoodJsonEm.Image.ToString()] = image;
        }
    }

    /// <summary>
    /// 根据店名获取保存上次吃的总价
    /// </summary>
    /// <param name="storeName"></param>
    /// <returns></returns>
    public float GetTotalPrice(string storeName)
    {
        float price = -1;
        foreach (var item in m_dicOutFood)
        {
            bool check = UnityHelper.FuzzyCheck(storeName, item.Value.V_StoreName);
            if (check)
            {
                price = item.Value.GetTotalPrice();
                break;
            }
        }
        if (price < 0)
        {
            UnityHelper.OpenAtlerWin("搜索的店名不存在");
        }
        return price;
    }
    /// <summary>
    /// 根据食物名称 获取所有的该食物信息
    /// </summary>
    /// <param name="foodName"></param>
    /// <returns></returns>
    public Dictionary<string, OutFood> GetFoodsByFoodName(string foodName)
    {
        Dictionary<string, OutFood> dic = new Dictionary<string, OutFood>();

        foreach (var item in m_dicOutFood)
        {
            //只检索好吃的
            bool chcek = UnityHelper.FuzzyCheck(foodName, item.Value.V_GoodFoodName);
            if (chcek)
            {
                dic.Add(foodName, item.Value);
            }
        }

        return dic;
    }

    /// <summary>
    /// 根据年月得到所有数据
    /// </summary>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <returns></returns>
    public List<OutFood> GetDatasByDate(int year, int month)
    {
        List<OutFood> foods = new List<OutFood>();
        foreach (var item in m_dicOutFood)
        {
            if (item.Value.V_Date.Year == year && item.Value.V_Date.Month == month)
            {
                foods.Add(item.Value);
            }
        }
        return foods;
    }

}
/// <summary>
/// 出去吃的Json构造数据
/// </summary>
public class OutFood
{
    public int V_Key = 0;
    /// <summary>
    /// 地址
    /// </summary>
    public string V_Adress = "";
    /// <summary>
    /// 店名
    /// </summary>
    public string V_StoreName = "";
    /// <summary>
    /// 好吃的菜
    /// </summary>
    public string V_GoodFoodName = "";
    /// <summary>
    /// 难吃的菜
    /// </summary>
    public string V_BadFoodName = "";
    /// <summary>
    /// 评价
    /// </summary>
    public string V_Evaluate = "";
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime V_Date;
    /// <summary>
    /// 星级
    /// </summary>
    public float V_Star = 0;
    /// <summary>
    /// 总价
    /// </summary>
    public float V_TotalPrice = 0;
    /// <summary>
    /// 图片名称
    /// </summary>
    public string V_Iamge = "";
    //每道菜的单价
    private Dictionary<string, float> m_dicFoodPrice = new Dictionary<string, float>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key">主键</param>
    /// <param name="adress">地址</param>
    /// <param name="storename">店名</param>
    /// <param name="goodFoodName">好吃的菜</param>
    /// <param name="badFoodName">不好吃的菜</param>
    /// <param name="evaluate">评价</param>
    /// <param name="date">日期</param>
    /// <param name="start">星级</param>
    /// <param name="image">图片</param>
    public OutFood(int key, string adress, string storename, string goodFoodName, string badFoodName, string evaluate, string date, float star, string image)
    {
        V_Key = key;
        V_Adress = adress;
        V_StoreName = storename;
        V_GoodFoodName = goodFoodName;
        V_BadFoodName = badFoodName;
        V_Evaluate = evaluate;
        V_Date = UnityHelper.ParseStrToDateTime(date);
        V_Star = star;
        V_Iamge = image;
        //计算总价
        if (!string.IsNullOrEmpty(goodFoodName))
        {
            string[] goodsStrs = goodFoodName.Split(';');
            for (int i = 0; i < goodsStrs.Length; i++)
            {
                string[] priceStr = goodsStrs[i].Split('-');
                float price = 0;
                if (priceStr.Length > 1)
                {
                    if (float.TryParse(priceStr[1], out price))
                    {
                        V_TotalPrice += price;
                    }
                }
                //保存单价
                if (!m_dicFoodPrice.ContainsKey(priceStr[0]))
                    m_dicFoodPrice.Add(priceStr[0], price);
            }
        }
        if (!string.IsNullOrEmpty(badFoodName))
        {
            string[] badsStrs = badFoodName.Split(';');
            for (int i = 0; i < badsStrs.Length; i++)
            {
                string[] priceStr = badsStrs[i].Split('-');
                float price = 0;
                if (priceStr.Length > 1)
                {
                    if (float.TryParse(priceStr[1], out price))
                    {
                        V_TotalPrice += price;
                    }
                }
                //保存单价
                if (!m_dicFoodPrice.ContainsKey(priceStr[0]))
                    m_dicFoodPrice.Add(priceStr[0], price);
            }
        }

    }
    /// <summary>
    /// 获取最后一次保存的总价
    /// </summary>
    /// <returns></returns>
    public float GetTotalPrice()
    {
        return V_TotalPrice;
    }

    /// <summary>
    /// 得到一道菜的单价
    /// </summary>
    /// <param name="name">菜名</param>
    /// <returns></returns>
    public float GetOnePrice(string name)
    {
        if (m_dicFoodPrice.ContainsKey(name))
        {
            return m_dicFoodPrice[name];
        }
        else
        {
            return -1;
        }
    }

}

/// <summary>
/// 出去吃json数据枚举
/// </summary>
public enum OutFoodJsonEm
{
    Adress,
    StoreName,
    GoodFoodName,
    BadFoodName,
    Evaluate,
    Date,
    Star,
    Image,

}

