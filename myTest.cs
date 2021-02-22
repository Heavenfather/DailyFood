using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Manager;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class myTest : MonoBehaviour
{
    [SerializeField]
    Image m_img;

    void Start()
    {
    }

    public void SetImage()
    {
        OpenFile.GetInstance().OpenDialogAndCopyImage("2", (obj) =>
        {
            if (this == null)
                return;
            if (obj == null)
                return;
            Sprite sp = obj as Sprite;
            m_img.sprite = sp;
        });
    }


}
