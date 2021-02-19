using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinOutFoodMain : BaseUIForms
{
    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.HideOther;
        base.CurrentUIType.UIForm_Type = UIFormType.Norlmal;
        SetCloseToDo(OnBtnCloseClick);
    }

    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinOutFoodMain;
    }

    private void OnBtnCloseClick()
    {
        //关闭自身，显示主界面
        CloseUIForm(GetWinType());
        OpenUIForm(EM_WinType.MainUIPanel);
    }

    public void OnRondomClick()
    {
        // OpenFile.GetInstance().LoadImg();
    }

    public void OnAddFoodClick()
    {
        //打开添加面板
        OpenUIForm(EM_WinType.WinAddOutFood);
    }

    public void OnSearchClick()
    {
        //打开搜索面板

    }

}
