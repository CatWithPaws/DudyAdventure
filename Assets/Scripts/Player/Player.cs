using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(PlayerMoveComponent))]
[RequireComponent(typeof(PlayerAnimationComponent))]
public class Player : MonoBehaviour
{
    public static UnityEngine.Events.UnityEvent OnCollisionWithCeilingEnterEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnCollisionWithFloorStayEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnCollisionWithFloorExitEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnPlayerDeadEvent = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnDialogStarted = new UnityEngine.Events.UnityEvent();
    public static UnityEngine.Events.UnityEvent OnDialogEnded = new UnityEngine.Events.UnityEvent();

    public enum State
    {
        IDLE,
        WALK,
        JUMP,
        FALL,
        DASH,
        DIALOG
    }

    [SerializeField] private PlayerMoveComponent playerMove;
    [SerializeField] private PlayerAnimationComponent playerAnimation;
    private State currentState;
    private float directionByX;
    private void Awake()
    {
        OnPlayerDeadEvent.AddListener(OnPlayerDead);
        playerAnimation = GetComponent<PlayerAnimationComponent>();
        playerMove = GetComponent<PlayerMoveComponent>();

    }
    private void Update()
    {
        playerMove.CheckInput();
        playerMove.CatchMove();
    }
    private void FixedUpdate()
    {
        playerMove.MovePlayer(out currentState,ref directionByX);
    }
    private void LateUpdate()
    {
        playerAnimation.AnimatePlayer(ref currentState,ref directionByX);
    }

    private void OnPlayerDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
