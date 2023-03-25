using System.Collections;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum ItemType
    {
        ExtraBomb,
        BlastRadius,
        SpeedIncrease,
        StopAnimate
    }

    public ItemType type;

    private void OnItemPickup(GameObject player)
    {
        switch (type)
        {
            case ItemType.StopAnimate:
                player.GetComponent<BombControllerUI>().StopGame();
                break;

            case ItemType.ExtraBomb:
                player.GetComponent<BombControllerUI>().AddBomeFuseTime();
                break;

            case ItemType.BlastRadius:
                player.GetComponent<BombControllerUI>().Radius();
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<JoystickAnimateV2>().speed++;
                break;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }
}
