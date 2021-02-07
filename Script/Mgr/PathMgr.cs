using System.IO;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class PathMgr : ManagerSingleton<PathMgr>
    {
        #region 资源路径
        private const string UI_Path = "Prefabs/UI";
        #endregion
        private Dictionary<EM_ResPathStyle, string> m_resDic = new Dictionary<EM_ResPathStyle, string>();

        public PathMgr()
        {
            m_resDic.Add(EM_ResPathStyle.UI, UI_Path);
        }

        private string dataPath = "";
        /// <summary>
        /// 程序资源所在路径
        /// </summary>
        /// <value></value>
        public string PathRes
        {
            get
            {
                if (string.IsNullOrEmpty(dataPath))
                {
                    dataPath = Application.persistentDataPath.Replace("\\", "/");
                }
                return dataPath;
            }
        }

        /// <summary>
        /// 存放stream资源的路径
        /// </summary>
        /// <value></value>
        public string StreamPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// 存放json文件的路径
        /// </summary>
        /// <value></value>
        public string JsonDataPath
        {
            get
            {
                return Application.streamingAssetsPath.Replace("\\", "/") + "/JsonData";
            }
        }

        /// <summary>
        /// 检查某个路径下文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool CheckFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 根据资源类型，拼接路径
        /// </summary>
        /// <param name="style"></param>
        /// <param name="resName"></param>
        /// <returns></returns>
        public string GetResPath(EM_ResPathStyle style, string resName)
        {
            if (string.IsNullOrEmpty(resName))
            {
                return null;
            }
            if (m_resDic.ContainsKey(style))
            {
                return string.Format("{0}/{1}", m_resDic[style], resName);
            }
            else
            {
                LogMgr.GetInstance().Log(LogEnum.Error, "拼接的资源类型不存在 style=" + style.ToString());
                return "";
            }
        }

    }

    public enum EM_ResPathStyle
    {
        UI
    }

}
