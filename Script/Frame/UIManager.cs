﻿using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Manager;

//UI管理器  作用：是整个UI框架的核心，用户通过这个脚本，来实现框架绝大多数的功能实现
public class UIManager : MonoBehaviour
{
    //UI窗体预设路径
    private Dictionary<EM_WinType, string> _DictionaryPaths;
    //缓存所有UI窗体
    private Dictionary<EM_WinType, BaseUIForms> _DicAllUiFormses;
    //当前显示的UI窗体
    private Dictionary<EM_WinType, BaseUIForms> _DicCurrentUIForm;

    //UI根节点
    private Transform _TraCanvasTranform = null;

    //全屏幕显示的节点
    private Transform _TraNormal = null;


    //固定显示的节点
    private Transform _TraFixed = null;

    //弹出节点
    private Transform _TraPopUp = null;

    //UI管理脚本的节点
    private Transform _UIScripts = null;

    private static UIManager _instance = null;
    //得到实例
    public static UIManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("_UIManager").AddComponent<UIManager>();   //为了保证这个脚本是挂载在一个对象上
        }

        return _instance;
    }

    public void Init()
    {
        //初始化窗体资源列表
        WinList.InitResPath();
        _DictionaryPaths = WinList.DicUIResPath;
        //字段初始化
        _DicAllUiFormses = new Dictionary<EM_WinType, BaseUIForms>();
        _DicCurrentUIForm = new Dictionary<EM_WinType, BaseUIForms>();
    }

    //初始化核心数据，加载“UI窗体路径”到集合中
    void Awake()
    {
        //初始化加载（根UI窗体） canvas预设
        InitRootCanvasLoading();

        //得到各个节点  根节点、全屏节点、固定节点、弹出节点
        _TraCanvasTranform = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;

        _TraNormal = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_NORMAL);
        _TraFixed = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_FIXED);
        _TraPopUp = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_POPUP);
        _UIScripts = UnityHelper.FindTheChildNode(_TraCanvasTranform.gameObject, SysDefine.SYS_NODE_SCRIPTSMANAGER);

        //把本脚本作为根UI窗体的子节点
        this.gameObject.transform.SetParent(_UIScripts, false);

        //根UI窗体 在场景转换的时候不允许销毁
        DontDestroyOnLoad(_TraCanvasTranform);
    }

    /// <summary>
    /// 显示UI窗体
    ///     功能：
    ///         1.加载与判断指定的UI窗体的名称，加载到“所有UI窗体”缓存集合中
    ///         2.根据不同的UI窗体的显示模式，分别做不同 的加载处理
    /// </summary>
    /// <param name="UIFormName">参数是UI窗体预设名称</param>
    public void ShowUIForm(EM_WinType winType)
    {
        BaseUIForms baseUiForms = null;        //UI窗体基类

        //根据窗体名称，加载到窗体缓存集合中
        baseUiForms = LoadFormToAllUIFormCatch(winType);
        if (baseUiForms == null) return;

        //把当前窗口加载到“当前窗体”集合中
        LoadUIToCurrentForm(winType);
    }

    /// <summary>
    /// 关闭（返回上一个）窗体
    /// </summary>
    /// <param name="winType">窗口枚举</param>
    public void CloseUIForm(EM_WinType winType)
    {
        BaseUIForms baseUiForms;
        //“所有窗体”集合中，如果没有则返回
        _DicAllUiFormses.TryGetValue(winType, out baseUiForms);
        if (baseUiForms == null)
        {

            return;
        }

        //窗体关闭
        ExitUIForm(winType);
    }

    /// <summary>
    /// 得到窗口 因为资源都是同步加载的，直接取就行
    /// </summary>
    /// <param name="winType"></param>
    /// <returns></returns>
    public BaseUIForms GetWinForm(EM_WinType winType)
    {
        if (_DicAllUiFormses.ContainsKey(winType))
        {
            return _DicAllUiFormses[winType];
        }
        else
        {
            LogMgr.GetInstance().Log(LogEnum.Error, "当前没有打开" + winType.ToString() + "窗口");
            return null;
        }
    }

    #region 私有方法
    //初始化加载（根UI窗体）canvas预设
    private void InitRootCanvasLoading()
    {
        ResourcesMgr.GetInstance().LoadAsset(SysDefine.SYS_PATH_CANVAS, false);
    }

    /// <summary>
    /// 加载与判断指定的UI窗体的名称，加载到“所有UI窗体”缓存集合中
    /// </summary>
    /// <param name="winType">窗体枚举</param>
    /// <returns></returns>
    private BaseUIForms LoadFormToAllUIFormCatch(EM_WinType winType)
    {
        BaseUIForms baseUiFormsResult = null;       //加载的返回UI窗体基类

        //先尝试查询里面有没有值.没有再做判断向里面加载
        _DicAllUiFormses.TryGetValue(winType, out baseUiFormsResult);
        if (baseUiFormsResult == null)
        {
            //加载指定路径的UI窗体
            baseUiFormsResult = LoadUIForm(winType);
        }

        return baseUiFormsResult;
    }

    /// <summary>
    /// 加载指定名称UI窗体
    /// 功能：
    ///     1.根据“UI窗体枚举”，加载预设克隆体
    ///     2.根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到根窗体下不同的节点
    ///     3.隐藏刚创建的UI克隆体
    ///     4.把克隆体加入到所有UI窗体的缓存集合中
    /// </summary>
    /// <param name="winType">UI窗体枚举</param>
    /// <returns></returns>
    private BaseUIForms LoadUIForm(EM_WinType winType)
    {
        string strUIFormPath = null;    //UI窗体的查询路径
        GameObject goCloneUIPrefab = null;  //创建的UI克隆体
        BaseUIForms baseUiForms = null;  //窗体基类

        //根据UI窗体名称，得到对应的加载路径
        _DictionaryPaths.TryGetValue(winType, out strUIFormPath);


        //根据UI窗体名称，加载预设克隆体
        if (!string.IsNullOrEmpty(strUIFormPath))
        {
            goCloneUIPrefab = ResourcesMgr.GetInstance().LoadAsset(strUIFormPath, false);
        }
        else
        {
            LogMgr.GetInstance().Log(LogEnum.Error, "加载UI窗体出错，没有找到" + winType.ToString() + "窗口的路径");
            return null;
        }

        //设置UI克隆体的父节点  根据克隆体中带的脚本中不同的“位置信息”
        if (_TraCanvasTranform != null && goCloneUIPrefab != null)
        {
            baseUiForms = goCloneUIPrefab.GetComponent<BaseUIForms>();        //得到这个克隆体里面的BaseUIForms脚本

            if (baseUiForms == null)
            {
                LogMgr.GetInstance().Log(LogEnum.Error, "baseUIForms为空，请先确认窗体预设对象是否已经加载了子类脚本,UIFormName=" + winType.ToString());
                return null;
            }

            if (baseUiForms.GetUIType() == UIFormType.Full)
            {
                goCloneUIPrefab.transform.SetParent(_TraNormal, false);
            }
            else if (baseUiForms.GetUIType() == UIFormType.PopUp)
            {
                goCloneUIPrefab.transform.SetParent(_TraPopUp, false);
            }

            //设置隐藏   初始时应该是隐藏的，因为不知道需不需要一开始就显示
            goCloneUIPrefab.SetActive(false);

            //把克隆体加入到所有UI窗体缓存集合中
            if (!_DicAllUiFormses.ContainsKey(winType))
                _DicAllUiFormses.Add(winType, baseUiForms);

            return baseUiForms;
        }

        return null;

    }

    /// <summary>
    /// 把当前窗口加载到“当前窗体”集合中
    /// </summary>
    /// <param name="winType">UI窗体枚举</param>
    private void LoadUIToCurrentForm(EM_WinType winType)
    {
        BaseUIForms baseUiForm;                 //UI窗体基类
        BaseUIForms baseUiFormsByAllUIForm;     //从“所有窗体”集合中得到的窗体

        //首先先检查当前的显示窗口中是否已经有这个预设，如果已经有了就直接返回
        _DicCurrentUIForm.TryGetValue(winType, out baseUiForm);
        if (baseUiForm != null) return;

        //把当前的窗体加载到“正在显示”的集合中来
        _DicAllUiFormses.TryGetValue(winType, out baseUiFormsByAllUIForm); //如果_DicCurrentUIForm里面没有，就从_DicAllUiFormses里面拿出来，这个里面肯定是有的
        if (baseUiFormsByAllUIForm != null)
        {
            _DicCurrentUIForm.Add(winType, baseUiFormsByAllUIForm);       //从“所有窗体”集合中拿到之后，就添加到“当前显示”窗体集合中
            baseUiFormsByAllUIForm.Display();       //显示窗口
        }

    }


    /// <summary>
    /// 普通窗体关闭
    /// </summary>
    /// <param name="winType">窗体枚举</param>
    private void ExitUIForm(EM_WinType winType)
    {
        BaseUIForms baseUiForms;
        //检查“当前显示窗体”集合中是否存在，如果不存在则直接返回
        _DicCurrentUIForm.TryGetValue(winType, out baseUiForms);
        if (baseUiForms == null)
            return;

        //把窗体干掉
        baseUiForms.Hiding();
        //把当前指定显示的UI窗体作关闭处理，且从“当前显示窗体”集合中移除
        _DicAllUiFormses.Remove(winType);
        _DicCurrentUIForm.Remove(winType);
    }

    #endregion
}
