using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingChecker : MonoBehaviour
{
    [SerializeField] private PlayerMoveComponent playerMove;
    private void Start()
    {
        playerMove = FindObjectOfType<PlayerMoveComponent>() as PlayerMoveComponent;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)

        {
            playerMove.OnCollisionCeilingEnter();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)

        {
            playerMove.OnCollisionCeilingExit();
        }
    }
}
