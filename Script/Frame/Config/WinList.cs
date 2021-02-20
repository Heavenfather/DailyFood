using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class WinList
{
    private static Dictionary<EM_WinType, string> dicUIResPath = new Dictionary<EM_WinType, string>();

    public static Dictionary<EM_WinType, string> DicUIResPath { get => dicUIResPath; set => dicUIResPath = value; }

    public static void InitResPath()
    {
        string path = "";
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "MainPanel");
        dicUIResPath.Add(EM_WinType.MainUIPanel, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "PopUpWindows");
        dicUIResPath.Add(EM_WinType.PopUpWindows, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "WinOutFoodMain");
        dicUIResPath.Add(EM_WinType.WinOutFoodMain, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "WinAddOutFood");
        dicUIResPath.Add(EM_WinType.WinAddOutFood, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "AddFoodPanel");
        dicUIResPath.Add(EM_WinType.AddFoodPanel, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "WinRandomOutFood");
        dicUIResPath.Add(EM_WinType.WinRandomOutFood, path);
        path = PathMgr.GetInstance().GetResPath(EM_ResPathStyle.UI, "WinOutFoodSearch");
        dicUIResPath.Add(EM_WinType.WinOutFoodSearch, path);
    }

}
