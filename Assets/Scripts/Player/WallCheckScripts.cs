using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckScripts : MonoBehaviour
{
    [SerializeField] private PlayerMoveComponent playerMove;
    [SerializeField] private int side;
    [SerializeField] private LayerMask whatIsWall;
    private void Awake()
    {
        playerMove = FindObjectOfType<PlayerMoveComponent>() as PlayerMoveComponent;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsValidWall(collision)) return;
        playerMove.OnPlayerCollisionWithWallStayEvent(side);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsValidWall(collision)) return;
        playerMove.OnPlayerCollisionWithWallExitEvent();
    }

    private bool IsValidWall(Collider2D collision)
    {
		if (collision.tag == "Player") return false;
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) return false;
		if (collision.gameObject.GetComponent<Dangerous>()) return false;
		if (collision.gameObject.layer == LayerMask.NameToLayer("Can't Wall Jump")) return false;
		if (collision.gameObject.layer == LayerMask.NameToLayer("HideinCamera")) return false;
		return true;
	}
}
