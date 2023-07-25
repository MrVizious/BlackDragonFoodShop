using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameEvents;

public class TextListener : MonoBehaviour
{
    public TMP_Text text;

    public void SetText(string newText)
    {
        text.text = newText;
    }
}
