using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationComponent : MonoBehaviour
{
	[SerializeField] private Animator playerAnimator;
	private string[] AnimateNames = new string[7];

	private void Awake()
	{
		playerAnimator = GetComponent<Animator>();
		AnimateNames[(int)Player.State.IDLE] = "Idle";
		AnimateNames[(int)Player.State.WALK] = "Walk";
		AnimateNames[(int)Player.State.JUMP] = "Jump";
		AnimateNames[(int)Player.State.FALL] = "Fall";
		AnimateNames[(int)Player.State.DIALOG] = "Idle";	
	}

	public void AnimatePlayer(ref Player.State state,ref float directionByX)
	{
		playerAnimator.Play(AnimateNames[(int)state]);
		if(directionByX != 0)	transform.localScale = new Vector3(directionByX, transform.localScale.y, transform.localScale.z);
	}


}
