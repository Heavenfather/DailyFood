using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// 管理类单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ManagerSingleton<T> where T : new()
    {
        private static T _t;
        public static T GetInstance()
        {
            if (_t == null)
            {
                _t = new T();
            }
            return _t;
        }
    }
}
