﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.OnCollisionWithCeilingEnter.Invoke();
    }
}
