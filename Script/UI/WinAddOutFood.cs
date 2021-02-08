using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAddOutFood : BaseUIForms
{
    public override EM_WinType GetWinType()
    {
        return EM_WinType.WinAddOutFood;
    }

    public override void InitUIType()
    {        
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.Normal;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
    }
}
