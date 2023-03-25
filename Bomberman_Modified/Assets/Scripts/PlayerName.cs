using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    public string Name;
    public TMP_InputField inputField;
    public TextMeshProUGUI textDisplay;

    void Start()
    {
        textDisplay.text = "Enter your name here";
    }

    public void StoreName()
    {
        Name = inputField.text;
        textDisplay.text = Name;
    }
}
