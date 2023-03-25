using UnityEngine;
using Mirror;

public class PlayerSpawn : NetworkBehaviour
{
    [SerializeField] private string spawnPointTag = "SpawnPoint";
    private static Transform[] spawnPoints;
    private int numPlayers;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (spawnPoints == null)
        {
            // Find all spawn points with the given tag
            GameObject[] spawnPointObjects = GameObject.FindGameObjectsWithTag(spawnPointTag);
            spawnPoints = new Transform[spawnPointObjects.Length];

            for (int i = 0; i < spawnPointObjects.Length; i++)
            {
                spawnPoints[i] = spawnPointObjects[i].transform;
            }
        }

        int numPlayers = GameObject.FindGameObjectsWithTag("Player").Length;
        //Debug.Log(numPlayers);
        if (numPlayers == 1)
        {
            transform.position = spawnPoints[0].position;
        }
        else if (numPlayers == 2)
        {
            transform.position = spawnPoints[2].position;
        }
        else if (numPlayers == 3)
        {
            transform.position = spawnPoints[1].position;
        }
        else if (numPlayers == 4)
        {
            transform.position = spawnPoints[3].position;
        }
    }
}
