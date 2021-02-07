using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnityHelper帮助脚本
/// 作用：
///     1.集成整个项目中通用的方法
/// </summary>
public class UnityHelper : MonoBehaviour {

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
    public static T GetChildNodeComponentScript<T>(GameObject goParent, string childName) where T:Component     //限定这个泛型是一个组件
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
    /// 给子节点添加脚本
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    /// <param name="goParen">对象</param>
    /// <param name="childName">子对象名称</param>
    /// <returns></returns>
    public static T AddChildNodeComponent<T>(GameObject goParen, string childName) where T : Component
    {
        Transform searchTransform = null;       //查找子节点的结果
        //查找特定子节点
        searchTransform = FindTheChildNode(goParen, childName);

        //如果查找成功，再考虑这个对象中是否已经存在了相同脚本，有就删除，没有就添加

        if (searchTransform != null)
        {
            T[] componentScriptsArray = searchTransform.GetComponents<T>();

            for (int i = 0; i < componentScriptsArray.Length; i++)
            {
                if(componentScriptsArray[i]!=null)
                    Destroy(componentScriptsArray[i]);
            }

            return searchTransform.gameObject.AddComponent<T>();

        }
        //如果查找不成功，返回一个null
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
        childNode.localPosition=Vector3.zero;
        childNode.localScale=Vector3.one;
        childNode.localEulerAngles=Vector3.zero;
    }
}
