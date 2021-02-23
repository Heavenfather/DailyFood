using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageCell : MonoBehaviour
{
    [SerializeField]
    Image m_img;

    private Sprite m_sp;
    private string m_imgName;
    private UnityEngine.Object m_Panel;

    public void SetImage(string name, UnityEngine.Object panel)
    {
        Sprite sp = OpenFile.GetInstance().GetSpriteByName(name);
        if (sp != null)
        {
            m_img.sprite = sp;
            m_imgName = name;
        }
        if (panel != null)
        {
            m_Panel = panel;
        }
    }

    public void OnCheckClick()
    {

    }

    public void OnDeleteClick()
    {
        if (m_Panel != null)
        {
            WinAddOutFood win = m_Panel as WinAddOutFood;
            win.DeleteOneImg(m_imgName);
        }
    }

}
