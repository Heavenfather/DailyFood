using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class Notifier
    {
        public delegate void StandardDelegate(params object[] args);
        private Dictionary<string, StandardDelegate> m_eventDic = new Dictionary<string, StandardDelegate>();

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="func">回调函数</param>
        public void AddEventHandle(string eventName, StandardDelegate func)
        {
            if (!m_eventDic.ContainsKey(eventName))
            {
                m_eventDic.Add(eventName, func);
            }
            else
            {
                m_eventDic[eventName] += func;
            }
        }

        /// <summary>
        /// 移除某事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="func"></param>
        public void RemoveEventHandle(string eventName, StandardDelegate func)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                if (m_eventDic[eventName] != null)
                {
                    m_eventDic[eventName] -= func;
                }
            }
        }

        /// <summary>
        /// 触发通知某个事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="args">携带的参数</param>
        public void Notifire(string eventName, params object[] args)
        {
            StandardDelegate fun = null;
            if (m_eventDic.TryGetValue(eventName, out fun))
            {
                if (null != fun)
                {
                    fun(args);
                }
            }
        }

        /// <summary>
        /// 清空所有注册
        /// </summary>
        public void ClearAllEventHandle()
        {
            if (m_eventDic.Count <= 0) return;

            List<string> dicKeys = new List<string>(m_eventDic.Keys);
            for (int i = 0; i < dicKeys.Count; i++)
            {
                RemoveAllEventHandle(dicKeys[i]);
            }
            m_eventDic.Clear();
        }

        /// <summary>
        /// 移除某事件的所有委托
        /// </summary>
        /// <param name="eventName"></param>
        public void RemoveAllEventHandle(string eventName)
        {
            if (m_eventDic.ContainsKey(eventName))
            {
                if (m_eventDic[eventName] != null)
                {
                    Delegate[] deArr = m_eventDic[eventName].GetInvocationList();
                    for (int i = 0; i < deArr.Length; i++)
                    {
                        StandardDelegate de = (StandardDelegate)deArr[i];
                        RemoveEventHandle(eventName, de);
                    }
                }
            }
        }

    }
}
