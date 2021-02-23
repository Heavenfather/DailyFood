using System.Net.Http.Headers;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

namespace Manager
{
    /// <summary>
    /// 读取配置
    /// </summary>
    public class ConfigMgr : ManagerSingleton<ConfigMgr>
    {
        /// <summary>
        /// 保存配置枚举
        /// </summary>
        /// <typeparam name="ConfigEm"></typeparam>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private Dictionary<ConfigEm, string> m_dicConfig = new Dictionary<ConfigEm, string>();

        private int v_MaxImageIndex = 0;

        public bool V_IsOpenDebugger
        {
            get
            {
                if (GetConfig(ConfigEm.Debugger) == "1")
                    return true;

                return false;
            }
        }

        /// <summary>
        /// 保存图片的最大索引(图片名称)
        /// </summary>
        /// <value></value>
        public int V_MaxImageIndex { get => v_MaxImageIndex; set => v_MaxImageIndex = value; }

        private static readonly string CONFIGJSON = "/config.json";

        /// <summary>
        /// 初始化配置信息
        /// </summary>
        public void InitData()
        {
            string file = PathMgr.GetInstance().StreamPath + CONFIGJSON;
            if (PathMgr.GetInstance().CheckFile(file))
            {
                try
                {
                    StreamReader reader = new StreamReader(file);
                    string jsontxt = reader.ReadToEnd();
                    JsonData jsonData = JsonMapper.ToObject(jsontxt);
                    if (null != jsonData)
                    {
                        m_dicConfig.Add(ConfigEm.Debugger, jsonData["debugger"].ToString());
                        m_dicConfig.Add(ConfigEm.SourceDataPath, jsonData["sourceDataPath"].ToString());
                        m_dicConfig.Add(ConfigEm.LogMaxCount, jsonData["LogMaxCount"].ToString());
                    }
                }
                catch (Exception e)
                {
                    LogMgr.GetInstance().Log(LogEnum.Error, e.Message);
                }
            }
            else
            {
                LogMgr.GetInstance().ShowLogToPanel("配置文件不存在，请把config.json文件存放在StreamingAssets下");
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public string GetConfig(ConfigEm em)
        {
            if (m_dicConfig.ContainsKey(em))
            {
                return m_dicConfig[em];
            }
            else
            {
                return "Config enum is null";
            }
        }

    }

    public enum ConfigEm
    {
        //开启debugger
        Debugger,
        //导入数据源路径
        SourceDataPath,
        //日志输出行数总数
        LogMaxCount,

    }

}