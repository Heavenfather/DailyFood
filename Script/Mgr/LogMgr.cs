using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

namespace Manager
{
    public class LogMgr:ManagerSingleton<LogMgr>
    {

        /// <summary>
        /// 打印到面板上面的日志
        /// </summary>
        private string m_log = "";
        /// <summary>
        /// 保存到本地的log日志
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private List<string> m_lstLog = new List<string>();

        /// <summary>
        /// log保存的路径
        /// </summary>
        /// <returns></returns>
        private static readonly string LOGPATH = PathMgr.GetInstance().PathRes + "/log.txt";
        /// <summary>
        /// log最大存储行数
        /// </summary>
        private static int MAX_LOG_ROW_COUNT
        {
            get
            {
                int count = int.Parse(ConfigMgr.GetInstance().GetConfig(ConfigEm.LogMaxCount));
                return count;
            }
        }

        /// <summary>
        /// 添加输出打印 调试用
        /// </summary>
        /// <param name="logTxt"></param>
        public void AddLog(string logTxt)
        {
            m_log += logTxt + "\n";
        }

        /// <summary>
        /// 显示log日志到面板
        /// </summary>
        /// <returns></returns>
        public string ShowLog()
        {
            return m_log;
        }

        /// <summary>
        /// 输出日志
        /// </summary>
        /// <param name="em"></param>
        /// <param name="txt"></param>
        public void Log(LogEnum em, string logtxt)
        {
            if (ConfigMgr.GetInstance().V_IsOpenDebugger)
            {
                string txt = string.Format("[{0}]{1} {2}", DateTime.Now.ToLocalTime().ToString(), em.ToString(), logtxt);
                m_lstLog.Add(txt);
                //超过了阀值移除第一条log
                if (m_lstLog.Count >= MAX_LOG_ROW_COUNT)
                {
                    m_lstLog.RemoveAt(0);
                }
            }
#if UNITY_EDITOR
            switch (em)
            {
                case LogEnum.Warming:
                    Debug.LogWarning(logtxt);
                    break;
                case LogEnum.Normal:
                    Debug.Log(logtxt);
                    break;
                case LogEnum.Error:
                    Debug.LogError(logtxt);
                    break;
            }
#endif

        }

        /// <summary>
        /// 通知界面显示log
        /// </summary>
        public void ShowLogToPanel(string logTxt)
        {
            AddLog(logTxt);
            #if UNITY_EDITOR
            EventMgr.GetInstance().Notifire(EventName.Event_ShowPanelLog, m_log);
            #endif
        }

        /// <summary>
        /// 保存日志到本地
        /// </summary>
        public void SaveToLocal()
        {
            bool isSavelLog = false;
            string filePath = LOGPATH;
            if (!PathMgr.GetInstance().CheckFile(filePath))
            {
                StreamWriter logStream = File.CreateText(filePath);
                if (logStream != null)
                {
                    isSavelLog = true;
                    logStream.Close();
                }
                else
                {
                    ShowLogToPanel("创建log文件失败");
                }
            }
            else
            {
                isSavelLog = true;
            }

            if (isSavelLog)
            {
                string[] arr = File.ReadAllLines(filePath);
                if (arr.Length > MAX_LOG_ROW_COUNT)
                {
                    File.WriteAllLines(filePath, m_lstLog.ToArray());
                }
                else
                {
                    List<string> lst = new List<string>(arr);
                    lst.AddRange(m_lstLog);
                    File.WriteAllLines(filePath, lst.ToArray());
                }
                m_lstLog.Clear();
            }

        }

    }

    public enum LogEnum
    {
        Normal,
        Warming,
        Error,
    }

}
