using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationComponent : MonoBehaviour
{
	[SerializeField] private Animator playerAnimator;
	private SpriteRenderer spriteRenderer;
	private string[] AnimateNames = new string[10];

	private void Awake()
	{
		playerAnimator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		AnimateNames[(int)Player.State.IDLE] = "Idle";
		AnimateNames[(int)Player.State.WALK] = "Walk";
		AnimateNames[(int)Player.State.JUMP] = "Jump";
		AnimateNames[(int)Player.State.FALL] = "Fall";
		AnimateNames[(int)Player.State.DIALOG] = "Idle";
		AnimateNames[(int)Player.State.WALL_JUMP] = "Jump";
		AnimateNames[(int)Player.State.SLIDE_ON_WALL] = "Fall";
	}

	public void AnimatePlayer(ref Player.State state,ref float directionByX)
	{
		playerAnimator.Play(AnimateNames[(int)state]);
		if (directionByX != 0) spriteRenderer.flipX = directionByX < 0;
	}


}
