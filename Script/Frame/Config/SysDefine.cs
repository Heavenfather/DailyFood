using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*框架核心参数
 *功能：
 *  1.系统常量
 *  2.全局性方法
 *  3.系统枚举类型
 *  4.委托定义
 */

#region 系统枚举类型

//UI窗体类型
public enum UIFormType
{
    Norlmal,        //普通窗体
    Fixed,          //固定窗体
    PopUp           //弹出窗体
}

//UI窗体的显示类型
public enum UIFormShowMode
{
    Normal,         //普通
    ReverseChange,  //反向切换窗体
    HideOther
}

//UI窗体的透明度类型
public enum UIFormLucencyType
{
    Lucency,        //完全透明 不能穿透
    Translucency,    //半透明  不能穿透
    ImPeneterabla,  //低透明度 不能穿透
    Pentrate        //能穿透
}

#endregion

public class SysDefine
{
    #region 系统常量
    //路径常量
    public const string SYS_PATH_CANVAS = "Canvas";
    //标签常量
    public const string SYS_TAG_CANVAS = "_TagCanvas";
    public const string SYS_TAG_UICAMERA = "_TagUICamera";

    //节点常量
    public const string SYS_NODE_NORMAL = "NormalPanel";
    public const string SYS_NODE_FIXED = "FixedPanel";
    public const string SYS_NODE_POPUP = "PopUpPanel";
    public const string SYS_NODE_SCRIPTSMANAGER = "_UIScriptsHolder";

    //json名称
    public const string OutFoodJsonName = "outfood";

    #endregion

}
