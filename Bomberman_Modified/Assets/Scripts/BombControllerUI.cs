using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Mirror;
using System;

public class BombControllerUI : NetworkBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab;
    public int bombAmount = 1;
    private int bombsRemaining;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public float explosionDuration = 1f;

    [Header("Destructible")]
    public Tilemap destructibleTiles;
    public Destructible destructiblePrefab;

    [Header("UI")]
    public Button bombButton;

    private void Start()
    {
        destructibleTiles = GameObject.Find("Destructibles").GetComponent<Tilemap>();
        bombButton = GameObject.FindGameObjectWithTag("BombButton").GetComponent<Button>();
        if (this.isLocalPlayer && !isServer)
        {
            bombButton.onClick.AddListener(CmdPlaceBomb);
            bombButton.onClick.AddListener(create_bomb_dummy);
        }
        else if (this.isLocalPlayer && isServer)
        {
            bombButton.onClick.AddListener(RpcPlaceBomb);
        }
    }

    private void OnEnable()
    {
        bombsRemaining = bombAmount;
    }

    private void create_bomb_dummy()
    {
        StartCoroutine(PlaceBomb_dummy());
    }

    [ClientRpc]
    public void RpcPlaceBomb()
    {
        if (bombsRemaining > 0)
        {
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(bomb); // Spawn the bomb on the server

            bombsRemaining--;

            StartCoroutine(ExplodeAfterDelay(bomb, position));
        }
    }

    [Command]
    public void CmdPlaceBomb()
    {
        if (bombsRemaining > 0)
        {
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            NetworkServer.Spawn(bomb); // Spawn the bomb on the server

            bombsRemaining--;

            StartCoroutine(ExplodeAfterDelay(bomb, position));
        }
    }

    private IEnumerator PlaceBomb_dummy()
    {
        if (bombsRemaining > 0)
        {
            Vector2 position = transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            GameObject bomb = Instantiate(bombPrefab, position, Quaternion.identity);
            bombsRemaining--;

            yield return new WaitForSeconds(bombFuseTime);

            position = bomb.transform.position;
            position.x = Mathf.Round(position.x);
            position.y = Mathf.Round(position.y);

            Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
            explosion.SetActiveRenderer(explosion.start);
            explosion.DestroyAfter(explosionDuration);

            Explode(position, Vector2.up, explosionRadius);
            Explode(position, Vector2.down, explosionRadius);
            Explode(position, Vector2.left, explosionRadius);
            Explode(position, Vector2.right, explosionRadius);

            Destroy(bomb);
            bombsRemaining++;
            /*yield return new WaitForSeconds(bombFuseTime);
            Destroy(bomb);
            bombsRemaining++;*/
        }
    }

    private IEnumerator ExplodeAfterDelay(GameObject bomb, Vector2 position)
    {
        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        NetworkServer.Destroy(bomb); // Destroy the bomb on the server
        bombsRemaining++;

        // Call the explosion on all clients
        RpcExplode(position);
    }

    [ClientRpc] // Call this method on all clients
    private void RpcExplode(Vector2 position)
    {
        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
        {
            return;
        }

        position += direction;

        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, explosionLayerMask))
        {
            ClearDestructible(position);
            return;
        }

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }

    private void ClearDestructible(Vector2 position)
    {
        Vector3Int cell = destructibleTiles.WorldToCell(position);
        TileBase tile = destructibleTiles.GetTile(cell);

        if (tile != null)
        {
            Instantiate(destructiblePrefab, position, Quaternion.identity);
            destructibleTiles.SetTile(cell, null);
        }
    }

    public void AddBomb()
    {
        bombAmount++;
        bombsRemaining++;
    }

    //----------------------------------------------------------------------
    public float StopTime = 3f;

    public void StopGame()
    {
        StartCoroutine(Stop());
    }

    private IEnumerator Stop()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(StopTime);
        Time.timeScale = 1f;
    }

    //----------------------------------------------------------------------
    public float bombFuseTime = 1f;

    public void AddBomeFuseTime()
    {
        bombFuseTime++;
    }

    //----------------------------------------------------------------------
    public int explosionRadius = 1;

    public void Radius()
    {
        explosionRadius++;
    }
}
