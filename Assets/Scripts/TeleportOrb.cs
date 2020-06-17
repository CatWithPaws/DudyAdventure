using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOrb : MonoBehaviour
{
    void Start()
    {
        EventHolder.OnPlayerDeadEvent.AddListener(OnDeadPlayer);
    }
    void OnDeadPlayer()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player;
        if ((player = collision.gameObject.GetComponent<Player>()) && !player.CanTeleport()) { 
        player?.RestoreCanTeleport();
            gameObject.SetActive(false);
        }
    }

}
