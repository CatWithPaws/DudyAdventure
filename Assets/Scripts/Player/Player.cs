﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerMoveComponent))]
[RequireComponent(typeof(PlayerAnimationComponent))]
public class Player : MonoBehaviour
{
    public enum State
    {
        IDLE,
        WALK,
        JUMP,
        FALL,
        DASH
    }

    [SerializeField] private PlayerMoveComponent playerMove;
    [SerializeField] private PlayerAnimationComponent playerAnimation;
    private State currentState;
    private float directionByX;
    private void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimationComponent>();
        playerMove = GetComponent<PlayerMoveComponent>();
    }

    private void FixedUpdate()
    {
        playerMove.MovePlayer(out currentState,ref directionByX);
    }
    private void LateUpdate()
    {
        playerAnimation.AnimatePlayer(ref currentState,ref directionByX);
    }
}