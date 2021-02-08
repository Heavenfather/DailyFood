using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UnityHelper帮助脚本
/// 作用：
///     1.集成整个项目中通用的方法
/// </summary>
public class UnityHelper
{

    /// <summary>
    /// 查找父节点下的子节点
    /// 内部使用递归算法
    /// </summary>
    /// <param name="goParent">父节点</param>
    /// <param name="child">查找子对象名称</param>
    /// <returns>Transform</returns>
    public static Transform FindTheChildNode(GameObject goParent, string child)
    {
        Transform searchTransform = null;       //查找结果

        searchTransform = goParent.transform.Find(child);
        if (searchTransform == null)
        {
            foreach (Transform tra in goParent.transform)
            {
                //使用递归一层一层的找
                searchTransform = FindTheChildNode(tra.gameObject, child);
                if (searchTransform != null)
                    return searchTransform;
            }
        }

        return searchTransform;
    }

    /// <summary>
    /// 获取子节点对象
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParent">父对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T GetChildNodeComponentScript<T>(GameObject goParent, string childName) where T : Component     //限定这个泛型是一个组件
    {
        Transform searchTransform = null;       //查找结果
        searchTransform = FindTheChildNode(goParent, childName);

        if (searchTransform != null)
        {
            return searchTransform.gameObject.GetComponent<T>();
        }
        else
        {
            return null;
        }

    }

    /// <summary>
    /// 给子节点添加父对象
    /// </summary>
    /// <param name="parentNode">父对象方位</param>
    /// <param name="childNode">子对象方位</param>
    public static void AddChildNodeToParentNode(Transform parentNode, Transform childNode)
    {
        childNode.SetParent(parentNode);
        childNode.localPosition = Vector3.zero;
        childNode.localScale = Vector3.one;
        childNode.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// 打开提示窗口
    /// </summary>
    /// <param name="content"></param>
    /// <param name="okCallback"></param>
    /// <param name="cancelCallback"></param>
    public static void OpenAtlerWin(string content, UnityAction okCallback = null, UnityAction cancelCallback = null)
    {
        UIManager.GetInstance().ShowUIForm(EM_WinType.PopUpWindows);
        PopUpWindows win = UIManager.GetInstance().GetWinForm(EM_WinType.PopUpWindows) as PopUpWindows;
        if (win != null)
        {
            win.Init(content, okCallback, cancelCallback);
        }
    }

    /// <summary>
    /// 检索
    /// </summary>
    /// <param name="check">需要检索的字</param>
    /// <param name="result">检索目标</param>
    public static bool FuzzyCheck(string check, string result)
    {
        Regex regex = new Regex(check);
        Match mat = regex.Match(result);
        return mat.Success;
    }

    /// <summary>
    /// 将字符串转换成时间格式
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ParseStrToDateTime(string date)
    {
        DateTime time = DateTime.Parse(date);
        return time;
    }

}
