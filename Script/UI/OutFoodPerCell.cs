using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutFoodPerCell : MonoBehaviour
{
    [SerializeField]
    Text m_txtFoodName;

    private int m_tempId;

    public int TempId { get => m_tempId; }

    public void Init(int id, string txt)
    {
        m_tempId = id;
        m_txtFoodName.text = txt;
    }

    public void OnDeleteClick()
    {

    }

    public void OnChangeClick()
    {

    }
}
