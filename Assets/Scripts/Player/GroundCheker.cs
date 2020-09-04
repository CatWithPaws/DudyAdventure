using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheker : MonoBehaviour
{
	[SerializeField] private LayerMask whatIsGround;
	[SerializeField] private PlayerMoveComponent playerMove;
	private void Start()
	{
		playerMove = FindObjectOfType<PlayerMoveComponent>() as PlayerMoveComponent;
	}
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			playerMove.OnCollisionFloorStay();
	}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger) {
			
			playerMove.OnPlayerCollisionWithFloorExitEvent();
		}
	}
}
