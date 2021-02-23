using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Manager;

/// <summary>
///  资源加载管理器
/// </summary>
namespace SUIFW
{
    public class ResourcesMgr : ManagerSingleton<ResourcesMgr>
    {
        private Hashtable ht = null;                        //容器键值对集合

        public ResourcesMgr()
        {
            ht = new Hashtable();
        }

        void Awake()
        {
            ht = new Hashtable();
        }

        /// <summary>
        /// 调用资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">路径</param>
        /// <param name="isCatch">是否缓存</param>
        /// <returns></returns>
        public T LoadResource<T>(string path, bool isCatch) where T : UnityEngine.Object
        {
            if (ht.Contains(path))
            {
                return ht[path] as T;
            }
            T TResource = Resources.Load<T>(path);
            if (TResource == null)
            {
                LogMgr.GetInstance().Log(LogEnum.Error, "提取的资源找不到，请检查。 path=" + path);
            }
            else if (isCatch)
            {
                ht.Add(path, TResource);
            }

            return TResource;
        }

        /// <summary>
        /// 调用资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="isCatch">是否缓存</param>
        /// <returns></returns>
        public GameObject LoadAsset(string path, bool isCatch = false)
        {
            GameObject goObj = LoadResource<GameObject>(path, isCatch);
            GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj);
            if (goObjClone == null)
            {
                LogMgr.GetInstance().Log(LogEnum.Error, "克隆资源不成功，请检查。 path=" + path);
            }
            return goObjClone;
        }        

    }
}