using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Mirror;

public class CharacterManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> Character = new List<Sprite>();
    private int CharacterSelect = 0;
    public GameObject PlayerCharacter;
    private Camera MainCamera;
    
    
    public void NextOption()
    {
        CharacterSelect = CharacterSelect + 1;
        if (CharacterSelect == Character.Count)
        {
            CharacterSelect = 0;
        }
        sr.sprite = Character[CharacterSelect];
    }

    public void BackOption()
    {
        CharacterSelect = CharacterSelect - 1;
        if (CharacterSelect < 0)
        {
            CharacterSelect = Character.Count - 1;
        }
        sr.sprite = Character[CharacterSelect];
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void PlayGame()
    {
        MainCamera.transform.position = new Vector3(-1.1f, 0.49f, -10f);
    }

    public void Start()
    {
        MainCamera = Camera.main;
    }

    public void Pause()
    {
        MainCamera.transform.position = new Vector3(83.5f, -0.6f, -10f);
    }

    public void ButtonYes()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);   
    }

    public void ButtonNo()
    {
        MainCamera.transform.position = new Vector3(-1.1f, 0.49f, -10f);
    }
}