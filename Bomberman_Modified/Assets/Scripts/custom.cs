using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class custom : NetworkManager
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<Message>(OnCreateCharacter);
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        // you can send the message here, or wherever else you want
        Message characterMessage = new Message
        {
            character = PlayerPrefs.GetString("type")
        };
        NetworkClient.Send(characterMessage);
    }

    void OnCreateCharacter(NetworkConnectionToClient conn, Message message)
    {
        if (message.character.Equals("pref2"))
        {
            GameObject gameObject = Instantiate(playerPrefab2,new Vector3(-13.09f, 4.83f, 97.13162f), Quaternion.identity);
            JoystickAnimateV2 py = gameObject.GetComponent<JoystickAnimateV2>();
            NetworkServer.AddPlayerForConnection(conn, gameObject);
        }
        else if (message.character.Equals("pref3"))
        {
            GameObject gameObject = Instantiate(playerPrefab3,new Vector3(-13.09f, 4.83f, 97.13162f), Quaternion.identity);
            JoystickAnimateV2 py = gameObject.GetComponent<JoystickAnimateV2>();
            NetworkServer.AddPlayerForConnection(conn, gameObject);
        }
        else if (message.character.Equals("pref4"))
        {
            GameObject gameObject = Instantiate(playerPrefab4,new Vector3(-13.09f, 4.83f, 97.13162f), Quaternion.identity);
            JoystickAnimateV2 py = gameObject.GetComponent<JoystickAnimateV2>();
            NetworkServer.AddPlayerForConnection(conn, gameObject);
        }
        else if (message.character.Equals("pref1"))
        {
            Debug.Log(conn);
            GameObject gameObject = Instantiate(playerPrefab1, new Vector3(-13.09f, 4.83f, 97.13162f), Quaternion.identity);
            JoystickAnimateV2 py = gameObject.GetComponent<JoystickAnimateV2>();
            NetworkServer.AddPlayerForConnection(conn, gameObject);
        }
    }
}
