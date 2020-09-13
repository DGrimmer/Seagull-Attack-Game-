using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UpdateText : MonoBehaviour
{
    private Text text;
    private bool isInitialized = false;
    private string orginalText;
    void Awake()
    {
        text = GetComponent<Text>();
        orginalText = text.text;
        isInitialized = true;
    }

    public void UpdateUITextWithValue(int value)
    {
        if(!isInitialized)
            return;
        text.text = orginalText + value;
    }

    public void SetColor(Color color)
    {
        text.color = color;
    }
}
