using UnityEngine;
using Mirror;

public class Destructible : NetworkBehaviour
{
    [SyncVar]
    private int randomIndex;

    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float itemSpawnChance = 0.2f;

    public GameObject[] spawnableItems;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (spawnableItems.Length > 0 && Random.value < itemSpawnChance)
        {
            randomIndex = Random.Range(0, spawnableItems.Length);
            Debug.Log("Random index: " + randomIndex);
            CmdSpawnItem(randomIndex, transform.position);
        }
    }

    [ServerCallback]
    private void CmdSpawnItem(int index, Vector3 position)
    {
        GameObject item = Instantiate(spawnableItems[index], position, Quaternion.identity);
        NetworkServer.Spawn(item);

        // set the spawned item's SyncVar to store the index of the spawned item
        Item itemComponent = item.GetComponent<Item>();
        if (itemComponent != null)
        {
            itemComponent.index = index;
        }
    }

    // method called on the client-side to spawn the item
    [ClientCallback]
    private void SpawnItemOnClient(int index, Vector3 position)
    {
        GameObject item = Instantiate(spawnableItems[index], position, Quaternion.identity);
    }

    // ClientRpc called from the spawned item to notify all clients of its index
    [ClientRpc]
    public void RpcNotifyIndex(int index)
    {
        Debug.Log("Item index: " + index);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer || other.isTrigger || other.CompareTag("Player")) return;
        NetworkServer.Destroy(gameObject);
    }
}

// Item script to be attached to spawned items
public class Item : NetworkBehaviour
{
    [SyncVar]
    public int index;

    [System.Obsolete]
    public override void OnStartClient()
    {
        // notify all clients of the item's index
        if (hasAuthority)
        {
            GetComponent<Destructible>().RpcNotifyIndex(index);
        }
    }
}
