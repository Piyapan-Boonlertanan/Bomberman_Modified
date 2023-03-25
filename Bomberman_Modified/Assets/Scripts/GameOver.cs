using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverPanel;
    private bool check;

    public void Over()
    {
        gameOverPanel.SetActive(true);
    }
    void Update()
    {
        check = GameObject.Find("Setting").GetComponent<setting>().dead;
        if(check)
        {
            Over();
        }
    }
}

