using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.Networking;
using Manager;
using System.Threading.Tasks;

public class OpenFile : MonoBehaviour
{
    private string _lastSelectPath = "";
    private static OpenFile _instance;
    public static OpenFile GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("_OpenFile").AddComponent<OpenFile>();
        }
        return _instance;
    }

    private Dictionary<string, Sprite> m_dicCachSprite = new Dictionary<string, Sprite>();

    public void Init()
    {
        //缓存保存的图片资源
        if (Directory.Exists(PathMgr.GetInstance().ImagePath))
        {
            DirectoryInfo dir = new DirectoryInfo(PathMgr.GetInstance().ImagePath);
            FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.EndsWith(".meta"))
                    continue;
                int index = files[i].Name.LastIndexOf('.');
                string key = files[i].Name.Remove(index);

                StartCoroutine(LoadSpriteAsync(key, (obj) =>
                {
                    Sprite sp = obj as Sprite;
                    if (sp == null)
                        return;
                    if (!m_dicCachSprite.ContainsKey(key))
                    {
                        m_dicCachSprite.Add(key, sp);
                    }
                }));
            }
        }
        else
        {
            Directory.CreateDirectory(PathMgr.GetInstance().ImagePath);
        }
    }
    private void Awake()
    {
        //设置父节点
        Transform tran = GameObject.FindGameObjectWithTag(SysDefine.SYS_TAG_CANVAS).transform;
        Transform scriptHolder = UnityHelper.FindTheChildNode(tran.gameObject, SysDefine.SYS_NODE_SCRIPTSMANAGER);
        this.gameObject.transform.SetParent(scriptHolder);
    }

    public void OpenDialogAndCopyImage(string saveName, Action<UnityEngine.Object> callback)
    {
        OpenFileName ofn = new OpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png";
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        string path = "";
        if (string.IsNullOrEmpty(_lastSelectPath))
        {
            path = Application.dataPath;
        }
        else
        {
            path = _lastSelectPath;
        }
        path = path.Replace('/', '\\');
        //默认路径
        ofn.initialDir = path;
        ofn.title = "Open Project";

        ofn.defExt = "JPG";//显示文件的类型
                           //注意 以下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

        if (WindowDll.GetFile(ofn))
        {
            //记录上次选择的路径
            _lastSelectPath = ofn.file;
            _lastSelectPath = _lastSelectPath.Replace('\\', '/');
            int index = _lastSelectPath.LastIndexOf('/');
            _lastSelectPath = _lastSelectPath.Remove(index);
            StartCoroutine(LoadAndCopyImage(ofn.file, saveName, callback));
        }
    }

    IEnumerator LoadAndCopyImage(string path, string saveName, Action<UnityEngine.Object> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get("file:///" + path);
        DownloadHandlerTexture downTexture = new DownloadHandlerTexture(true);
        request.downloadHandler = downTexture;
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success && request != null)
        {
            //获取Texture
            Texture2D texture = downTexture.texture;
            //直接将选择图保存为png格式，拷贝到指定目录
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(PathMgr.GetInstance().ImagePath + saveName + ".png", bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            if (!m_dicCachSprite.ContainsKey(saveName))
            {
                m_dicCachSprite.Add(saveName, sprite);
            }
            callback(sprite);
        }
        else
        {
            LogMgr.GetInstance().Log(LogEnum.Error, "加载图片失败:" + path);
        }
    }

    IEnumerator LoadSpriteAsync(string name, Action<UnityEngine.Object> callback)
    {
        string path = PathMgr.GetInstance().ImagePath + name + ".png";
        UnityWebRequest request = UnityWebRequest.Get("file:///" + path);
        DownloadHandlerTexture downTexture = new DownloadHandlerTexture(true);
        request.downloadHandler = downTexture;
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success && request != null)
        {
            Texture2D texture = downTexture.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            callback(sprite);
        }
    }

    public Sprite GetSpriteByName(string name)
    {
        if (m_dicCachSprite.ContainsKey(name))
        {
            return m_dicCachSprite[name];
        }
        else
        {
            //没有图片
            LogMgr.GetInstance().Log(LogEnum.Error, "未缓存到图片:" + name);
            return null;
        }
    }

}

/// <summary>
/// 打开窗口的预设值
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class WindowDll
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetFile([In, Out] OpenFileName ofn)
    {
        return GetOpenFileName(ofn);
    }
}
