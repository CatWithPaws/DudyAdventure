using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheker : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		Player.OnCollisionWithFloorStay.Invoke();
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		Player.OnCollisionWithFloorExit.Invoke();
	}
}
