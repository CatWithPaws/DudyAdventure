using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{

    

    [SerializeField] private BlackScreen blackScreen;
    [SerializeField] private Animator animator;


	public void StartExiting()
    {
        animator.Play("CampfireDisappear");
    }

    public void MakeDarkScreen()
    {
        blackScreen.animation.Play("BlackScreenAppear");
    }
}
