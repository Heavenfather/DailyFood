using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class HomeFoodMgr : ManagerSingleton<HomeFoodMgr>
    {
        public HomeFoodModel Model = null;
        public HomeFoodMgr()
        {
            if (Model == null)
            {
                Model = new HomeFoodModel();
            }
        }

    }
}
