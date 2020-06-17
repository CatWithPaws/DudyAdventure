using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheker : MonoBehaviour
{
	[SerializeField] private LayerMask whatIsGround;
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			EventHolder.OnPlayerCollisionWithFloorStayEvent.Invoke();
	}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger) {
			EventHolder.OnPlayerCollisionWithFloorExitEvent.Invoke();	
		}
	}
}
