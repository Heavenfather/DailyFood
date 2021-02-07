using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI遮罩管理器
/// </summary>
public class UIMaskManager : MonoBehaviour
{
    private static UIMaskManager _instance;
    public static UIMaskManager GetInstance()
    {
        if (_instance == null)
        {
            _instance=new GameObject("_UIMaskManager").AddComponent<UIMaskManager>(); //为了保证这个脚本是挂载在一个对象上
        }

        return _instance;
    }

    /* 字段 */
    //UI根节点对象
    private GameObject _GoCanvasRoot = null;
    //UI脚本根节点对象
    private Transform _TraUIScriptsHolder = null;
    //顶层面板
    private GameObject _GoTopPanel = null;
    //遮罩面板
    private GameObject _GoMaskPanel = null;
    //UI摄像机
    private Camera _UICamera;
    //UI摄像机原始的层深
    private float _OriginalUICameraDepth;

    void Awake()
    {
        //得到UI根节点对象、UI脚本根节点对象
        _GoCanvasRoot=GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS);
        _TraUIScriptsHolder = UnityHelper.FindTheChildNode(_GoCanvasRoot, "_UIScriptsHolder");

        //得到顶层面板、遮罩面板
        _GoTopPanel = _GoCanvasRoot;
        _GoMaskPanel = UnityHelper.FindTheChildNode(_GoCanvasRoot, "UIMaskPanel").gameObject;

        //得到UI摄像机、UI摄像机原始层深
        _UICamera = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_UICAMERA).GetComponent<Camera>();
        if (_UICamera != null)
        {
            _OriginalUICameraDepth = _UICamera.depth;
        }
        else
        {
            Debug.LogError(GetType()+"UI摄像机为null，请检查!");
        }
        
    }

    /// <summary>
    /// 设置遮罩面板
    /// </summary>
    /// <param name="goDisplayUIForm">需要显示的UI窗体</param>
    /// <param name="lucencyType">透明度属性</param>
    public void SetMaskWindow(GameObject goDisplayUIForm, UIFormLucencyType lucencyType = UIFormLucencyType.Lucency)
    {
        //顶层面板下移
        _GoTopPanel.transform.SetAsLastSibling();       //下移
        //启用遮罩窗体以及设置透明度
        switch (lucencyType)
        { 
            case UIFormLucencyType.Lucency:             //完全透明 不能穿透
                _GoMaskPanel.SetActive(true);
                Color newColor1 = new Color(255/255,255 / 255, 255 / 255, 0 / 255);
                _GoMaskPanel.GetComponent<Image>().color = newColor1;
                break;
            case UIFormLucencyType.Translucency:        //半透明  不能穿透
                _GoMaskPanel.SetActive(true);
                Color newColor2=new Color(255 / 255, 255 / 255, 255 / 255, 100 / 255);
                _GoMaskPanel.GetComponent<Image>().color = newColor2;
                break;
            case UIFormLucencyType.ImPeneterabla:       //低透明度 不能穿透
                _GoMaskPanel.SetActive(true);
                Color newColor3 = new Color(255 / 255, 255 / 255, 255 / 255, 200 / 255);
                _GoMaskPanel.GetComponent<Image>().color = newColor3;
                break;
            case UIFormLucencyType.Pentrate:            //完全透明 能穿透
                if (_GoMaskPanel.activeInHierarchy)
                {
                    _GoMaskPanel.SetActive(false);
                }
                break;
            default:
                break;
        }

        //遮罩窗体下移
        _GoMaskPanel.transform.SetAsLastSibling();      //遮罩的窗体下移，可以实现把前面的UI窗体防止穿透，下面的显示UI窗体也下移，但是它比遮罩面板晚下移，所以它还是会在遮罩面板的下面，这样子遮罩面板就不会挡住显示的UI窗体
        //显示的UI窗体下移
        goDisplayUIForm.transform.SetAsLastSibling();
        //设置摄像机层深(保证UI摄像机始终在最前面)
        if (_UICamera != null)
        {
            _UICamera.depth += 100;
        }
        else
        {
            Debug.LogError(GetType()+"UI摄像机为null，请检查!");
        }

    }

    /// <summary>
    /// 取消遮罩面板
    /// </summary>
    public void CancelMaskWindow()
    {
        //顶层面板上移
        _GoTopPanel.transform.SetAsFirstSibling();
        //禁用遮罩窗体以及设置透明度
        if (_GoMaskPanel.activeInHierarchy)     //如果它在面板中是活动的，就禁用
        {
            //隐藏
            _GoMaskPanel.SetActive(false);
        }

        //恢复UI摄像机层深
        if (_UICamera != null)
        {
            //设置层深
            _UICamera.depth = _OriginalUICameraDepth;
        }
        else
        {
            Debug.LogError(GetType() + "UI摄像机为null，请检查!");
        }
    }

}
