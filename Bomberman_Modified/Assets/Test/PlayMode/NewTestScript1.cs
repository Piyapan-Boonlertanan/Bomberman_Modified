using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewTestScript1
{
    Button PlayButton;
    // A Test behaves as an ordinary method


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
[SetUp]
public void Setup()
{
    SceneManager.LoadScene("Menu");
}
[UnityTest]
    public IEnumerator do_menu_to_playerselect_change()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        PlayButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        PlayButton.onClick.Invoke();
        yield return null;
        Scene currentscene =SceneManager.GetActiveScene();
        Assert.AreEqual("Play",currentscene.name);
        yield return null;

    }
}
