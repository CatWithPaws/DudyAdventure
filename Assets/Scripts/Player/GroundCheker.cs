﻿using System.Collections;
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
		Player.OnCollisionWithFloorExitEvent.Invoke();
	}
}
