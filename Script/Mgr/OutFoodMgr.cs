using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class OutFoodMgr : ManagerSingleton<OutFoodMgr>
    {
        public OutFoodModel Model = null;
        public OutFoodMgr()
        {
            if (Model == null)
            {
                Model = new OutFoodModel();
            }
        }
    }
}
