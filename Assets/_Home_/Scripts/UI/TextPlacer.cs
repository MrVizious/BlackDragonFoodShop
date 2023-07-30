using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPlacer : MonoBehaviour
{
    public PlayerData data;
    public TMP_Text text;
    private void Start()
    {
        text.text = "You earned " + data.moneyEarned + "$";
    }
}
