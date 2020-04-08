using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheker : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		Player.OnCollisionWithFloorStayEvent.Invoke();
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		Collider2D[] col = new Collider2D[1];
		col[0] = collision;
		Player.OnCollisionWithFloorExitEvent.Invoke();
	}
}
