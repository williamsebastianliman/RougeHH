using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SetText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    public void setText(string msg)
    {
        text.text = msg;
    }
}
