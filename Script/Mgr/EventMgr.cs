using Model;
using UnityEngine;

namespace Manager
{
    public class EventMgr : Notifier
    {
        private static EventMgr _instance;

        public static EventMgr GetInstance()
        {
            if (_instance == null)
            {
                _instance = new EventMgr();
            }
            return _instance;
        }

    }
}
