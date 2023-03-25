using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setting : MonoBehaviour
{
    public int number_pref;
    public bool dead;

    public void Awake(){
        number_pref = 1;
        dead = false;
        PlayerPrefs.SetString("type","pref1");
    }

    public void change_pref1(){
        number_pref = 1;
        PlayerPrefs.SetString("type","pref1");
    }
    public void change_pref2(){
        number_pref = 2;
        PlayerPrefs.SetString("type","pref2");
    }
    public void change_pref3(){
        number_pref = 3;
        PlayerPrefs.SetString("type","pref3");
    }
    public void change_pref4(){
        number_pref = 4;
        PlayerPrefs.SetString("type","pref4");
    }
    
    public void Dead()
    {
        dead = true;
    }
}
