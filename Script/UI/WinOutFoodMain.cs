using System.Collections;
using System.Collections.Generic;
using Manager;
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
        if (OutFoodMgr.GetInstance().Model.IsRandomized())
        {
            UnityHelper.OpenAtlerWin("返回将重置随机的数据", () =>
            {
                OutFoodMgr.GetInstance().Model.ResetRandomKeys();
                CloseUIForm(GetWinType());
                OpenUIForm(EM_WinType.MainUIPanel);
            });
        }
        else
        {
            //关闭自身，显示主界面
            CloseUIForm(GetWinType());
            OpenUIForm(EM_WinType.MainUIPanel);
        }
    }

    public void OnRondomClick()
    {
        //得到一个随机的数据
        OutFood food = OutFoodMgr.GetInstance().Model.GetOneRandomFood();
        if (food == null)
        {
            UnityHelper.OpenAtlerWin("没有随机出来数据");
            return;
        }
        //打开随机面板，并且初始化数据
        OpenUIForm(EM_WinType.WinRandomOutFood);
        WinRandomOutFood randomPanel = UIManager.GetInstance().GetWinForm(EM_WinType.WinRandomOutFood) as WinRandomOutFood;
        if (randomPanel != null)
        {
            randomPanel.Init(food);
        }
    }

    public void OnAddFoodClick()
    {
        //打开添加面板
        OpenUIForm(EM_WinType.WinAddOutFood);
    }

    public void OnSearchClick()
    {
        //打开搜索面板
        OpenUIForm(EM_WinType.WinOutFoodSearch);
    }
}
