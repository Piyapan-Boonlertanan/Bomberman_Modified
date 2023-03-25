using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundeffect : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1, sfx2, sfx3;

    public void playButton()
    {
        src.clip = sfx1;
        src.Play();

    }

    public void SettingButton()
    {
        src.clip = sfx2;
        src.Play();
    }

    public void QuitButton()
    {
        src.clip = sfx3;
        src.Play();
    }
}


