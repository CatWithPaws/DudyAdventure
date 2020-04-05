using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheker : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Player.OnCollisionWithFloorEnter.Invoke();
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		Player.OnCollisionWithFloorExit.Invoke();
	}
}
