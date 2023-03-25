using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.UI;

public class NewTestScript
{
    NetworkManager manager;
    Button bombButton;
    // A Test behaves as an ordinary method
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator do_bomb_spawn()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        SceneManager.LoadScene("Bomberman");
        yield return null;
        manager = GameObject.FindWithTag("GameController").GetComponent<NetworkManager>();
        manager.StartHost();
        yield return null;
        bombButton = GameObject.FindGameObjectWithTag("BombButton").GetComponent<Button>();
        bombButton.onClick.Invoke();
        yield return  new WaitForSeconds(1);
        GameObject bomb = GameObject.Find("Bomb(Clone)");
        Assert.IsNotNull(bomb);
        yield return new WaitForSeconds(5);

    }
}
