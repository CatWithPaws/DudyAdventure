using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : MonoBehaviour
{

    [SerializeField] private LayerMask whatIsDangerous;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.layer == whatIsDangerous)
        {
            Player.OnPlayerDeadEvent.Invoke();
            print("COollll");
        }
    }
}
