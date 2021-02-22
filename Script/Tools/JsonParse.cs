using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using Manager;
using System.Text.RegularExpressions;

public class JsonParse
{
    /// <summary>
    /// 解析json数据
    /// </summary>
    /// <param name="jsonName">json名字</param>
    /// <returns>Objec类型的JsonData</returns>
    public static JsonData ParseToJsonData(string jsonName)
    {
        string path = PathMgr.GetInstance().StreamPath + "/" + jsonName + ".json";
        if (PathMgr.GetInstance().CheckFile(path))
        {
            try
            {
                StreamReader r = new StreamReader(path);
                string json = r.ReadToEnd();
                r.Close();
                JsonData data = JsonMapper.ToObject(json);
                return data;
            }
            catch (Exception e)
            {
                LogMgr.GetInstance().Log(LogEnum.Error, "解析Json出错:" + e.Message);
                return null;
            }
        }
        return null;
    }

    /// <summary>
    /// 保存json数据到本地
    /// </summary>
    /// <param name="data"></param>
    public static bool SaveJsonDataToLocal(JsonData data, string saveName)
    {
        if (data == null) return false;
        try
        {
            string saveData = JsonMapper.ToJson(data);
            //转译中文
            Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
            saveData = reg.Replace(saveData, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
            StreamWriter streamWriter;
            if (!PathMgr.GetInstance().CheckFile(PathMgr.GetInstance().StreamPath + "/" + saveName + ".json"))
            {
                streamWriter = File.CreateText(PathMgr.GetInstance().StreamPath + "/" + saveName + ".json");
            }
            else
            {
                streamWriter = new StreamWriter(PathMgr.GetInstance().StreamPath + "/" + saveName + ".json");
            }
            streamWriter.Write(saveData);
            streamWriter.Dispose();
            streamWriter.Close();
            return true;
        }
        catch (Exception e)
        {
            LogMgr.GetInstance().Log(LogEnum.Error, "保存" + saveName + ".json失败" + e.Message);
            return false;
        }
    }

}
