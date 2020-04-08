using System;using System.Collections;using System.Collections.Generic;using UnityEngine;[RequireComponent(typeof(Rigidbody2D))]public class PlayerMoveComponent : MonoBehaviour{    private const int countOfStates = 7;    private delegate void DMove();    private DMove[] Move = new DMove[countOfStates];

    [SerializeField] private Vector2 velocity = new Vector2();    [SerializeField] private float speed;        [SerializeField] private LayerMask Detect;    [SerializeField] private float max_jump_height, min_jump_height, jumping_time;     private Rigidbody2D rigidbody;    private KeyCode jumpKey, dashKey, teleportKey;    private Vector3 lastPos;    private Collider2D collider;    private bool isGrounded;
    private bool willDash, isDashing, canDash = false;    private bool canTeleport = false, isTeleporting = false;    private bool isJumpJustPressed, isJumpJustReleased, isDashJustPressed, isTeleportJustPressed, isRightPressed, isLeftPressed;    private bool isInDialogue = false;    private float max_jump_velocity, min_jump_velocity, gravity;
    private float dashTime = .15f,dashPower = 30;
    private float teleportRange = 4f;
    private float BlockValue = 2;    private float lastDirectionByX = 1;
    private int countOfAdditionalJumps, maxCountOfAdditionalJumps = 1;    private void Start()    {
        rigidbody = GetComponent<Rigidbody2D>();        collider = GetComponent<Collider2D>();        Move[(int)Player.State.IDLE] = MoveInIdle;        Move[(int)Player.State.WALK] = MoveInWalk;        Move[(int)Player.State.JUMP] = MoveInJump;        Move[(int)Player.State.FALL] = MoveInFall;        Move[(int)Player.State.DASH] = MoveInDash;
        Move[(int)Player.State.DIALOG] = MoveInDialog;

        jumpKey = KeyCode.C;        dashKey = KeyCode.X;        teleportKey = KeyCode.Z;        countOfAdditionalJumps = maxCountOfAdditionalJumps;        velocity = new Vector2();        Player.OnCollisionWithCeilingEnterEvent.AddListener(OnCollisionCeiling);        Player.OnCollisionWithFloorStayEvent.AddListener(OnCollisionFloorStay);        Player.OnCollisionWithFloorExitEvent.AddListener(OnCollisionFloorExit);        Player.OnDialogStarted.AddListener(OnDialogStarted);        Player.OnDialogEnded.AddListener(OnDialogEnded);        CalculatePhysicsVariables();    }

    

    private void CalculatePhysicsVariables()
    {
        gravity = 2 * max_jump_height * BlockValue / (jumping_time * jumping_time);        min_jump_velocity = Mathf.Sqrt(gravity * min_jump_height * BlockValue);        max_jump_velocity = Mathf.Sqrt(gravity * max_jump_height * BlockValue);
    }    private Player.State GetState(ref float directionByX)    {
        if (isInDialogue) return Player.State.DIALOG;
        else if (willDash) return Player.State.DASH;        else if (!isGrounded && velocity.y >= 0) return Player.State.JUMP;        else if (!isGrounded && velocity.y < 0) return Player.State.FALL;        else if (directionByX != 0) return Player.State.WALK;        else return Player.State.IDLE;    }

    /// <summary>    /// Move Player and take a state in parameters    /// </summary>    /// <param name="state" description="Player's State after moving"></param>    public void MovePlayer(out Player.State state,ref float directionByX)    {        UpdateCheckDependencies(ref directionByX);        state = GetState(ref directionByX);
        print(state);        Move[(int)state]();               rigidbody.velocity = velocity;        lastPos = transform.position;
        lastDirectionByX = directionByX != 0 ? directionByX : lastDirectionByX;    }
    private void MoveInWalk()
    {
        
        CheckHorizontalMove();
        ControlGravity();
    }    private void MoveInIdle()    {
        ControlGravity();        CheckHorizontalMove();

    }    private void MoveInAir()
    {        MoveInWalk();        if (isJumpJustReleased && velocity.y > min_jump_velocity) velocity.y = min_jump_velocity;    }    private void MoveInJump()    {        MoveInAir();    }    private void MoveInFall()    {        MoveInAir();    }

    private void MoveInDialog()
    {
        velocity.x = 0;
        ControlGravity();
    }
    private void UpdateCheckDependencies(ref float directionByX)
    {        directionByX = Input.GetAxisRaw("Horizontal");
        
    }        private IEnumerator TeleportPlayer()
    {
        yield return new WaitForFixedUpdate();
        canTeleport = false;
        isTeleporting = true;
        Vector3 direction = GetDirectionByArrows();
        direction = direction == Vector3.zero ? new Vector3(lastDirectionByX, 0) : direction;
        direction.Normalize();

        if (IsCollidersInArea(ref direction))
            canTeleport = true;
        else
            transform.position += (direction * teleportRange);
        yield return new WaitForEndOfFrame();
        isTeleporting = false;
    }    private bool IsCollidersInArea(ref Vector3 direction)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + direction * teleportRange, collider.bounds.size * 0.9f, 0);
        return colliders.Length > 0;    }    private void ControlGravity()
    {
        if (lastPos.y == transform.position.y && velocity.y < 0) { velocity.y = 0; return; }
        velocity.y -= gravity * Time.fixedDeltaTime;        velocity.y = Mathf.Clamp(velocity.y, -dashPower, max_jump_velocity);
        
    }    private void CheckHorizontalMove()
    {
        int horizontalDirection = Convert.ToInt32(isRightPressed) - Convert.ToInt32(isLeftPressed);
        velocity.x = Mathf.Lerp(velocity.x, horizontalDirection * speed, 0.2f);
    }       private IEnumerator StartDash() {
        yield return new WaitForFixedUpdate();
        willDash = true;    }    private void MoveInDash() {
        if (!isDashing)
        {
            isDashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }    }    private IEnumerator Dash()
    {   
        yield return new WaitForFixedUpdate();
        Vector2 direction = GetDirectionByArrows();
        direction = direction == Vector2.zero ? new Vector2(lastDirectionByX, 0) : direction;
        direction = direction.normalized * dashPower;
        velocity = direction;
        yield return new WaitForSeconds(dashTime);
        willDash = false;
        isDashing = false;
    }    private Vector3 GetDirectionByArrows()
    {
        return new Vector3(
               Input.GetAxisRaw("Horizontal"),
               Input.GetAxisRaw("Vertical")
               );
    }    private IEnumerator Jump()
    {
        yield return new WaitForFixedUpdate();
        velocity.y = max_jump_velocity;
    }    private void OnDialogStarted()
    {
        isInDialogue = true;
        isDashing = false;
        willDash = false;
    }

    private void OnDialogEnded()
    {
        isInDialogue = false;
        isDashing = false;
        willDash = false;
    }    public void CatchMove()
    {
        if (isInDialogue) return;
        if (isJumpJustPressed && isGrounded) StartCoroutine(Jump());
        if (isJumpJustPressed && !isGrounded && countOfAdditionalJumps > 0 && !isInDialogue)
        {
            StartCoroutine(Jump());
            countOfAdditionalJumps--;
        }
        if (isDashJustPressed && canDash) StartCoroutine(StartDash());
        if (isTeleportJustPressed && !isTeleporting && canTeleport ) StartCoroutine(TeleportPlayer());
    }    public void CheckInput()
    {
        isTeleportJustPressed = Input.GetKeyDown(teleportKey);
        isJumpJustPressed = Input.GetKeyDown(jumpKey);
        isJumpJustReleased = Input.GetKeyUp(jumpKey);
        isDashJustPressed = Input.GetKeyDown(dashKey);
        isRightPressed = Input.GetKey(KeyCode.RightArrow);        isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
    }    public void OnCollisionCeiling() => velocity.y = 0;    public void OnCollisionFloorStay()
    {
        isGrounded = true;
        countOfAdditionalJumps = maxCountOfAdditionalJumps;
        canDash = true;
        canTeleport = true;
    }    public void OnCollisionFloorExit() => isGrounded = false;}