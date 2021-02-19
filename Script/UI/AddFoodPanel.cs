using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddFoodPanel : BaseUIForms
{
    public override EM_WinType GetWinType()
    {
        return EM_WinType.AddFoodPanel;
    }

    public override void InitUIType()
    {
        base.CurrentUIType.UIForm_ShowMode = UIFormShowMode.ReverseChange;
        base.CurrentUIType.UIForm_Type = UIFormType.PopUp;
        
    }


}
