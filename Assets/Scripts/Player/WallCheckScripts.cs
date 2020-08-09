using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheckScripts : MonoBehaviour
{
    [SerializeField] private int side;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsValidWall(collision)) return;
        EventHolder.OnPlayerCollisionWithWallStayEvent.Invoke(side);
        print("Wall!");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!IsValidWall(collision)) return;
        EventHolder.OnPlayerCollisionWithWallExitEvent.Invoke();
    }

    private bool IsValidWall(Collider2D collision)
    {
        if (collision.tag == "Player") return false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Raycast")) return false;
        if (collision.gameObject.GetComponent<Dangerous>()) return false;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Can't Wall Jump")) return false;
        return true;
    }
}
