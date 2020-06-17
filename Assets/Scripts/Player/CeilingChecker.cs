using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingChecker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.isTrigger)

        {
            EventHolder.OnPlayerCollisionWithCeilingEnterEvent.Invoke();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.isTrigger)

        {
            EventHolder.OnPlayerCollisionWithCeilingExitEvent.Invoke();
        }
    }
}
