using System.Collections;using System.Collections.Generic;using UnityEngine;[RequireComponent(typeof(Rigidbody2D))]public class PlayerMoveComponent : MonoBehaviour{    private const int countOfStates = 5;    private delegate void DMove();    private DMove[] Move = new DMove[countOfStates];            private Rigidbody2D rigidbody;    [SerializeField] private Vector2 velocity = new Vector2();    [SerializeField] private float speed;    [SerializeField] private bool canJump;    [SerializeField] private bool isGrounded;    [SerializeField] private Collider2D collider;    [SerializeField] private LayerMask Detect;    [SerializeField] private float max_jump_height, min_jump_height, jumping_time;    [SerializeField] private KeyCode jumpKey, dashKey, teleportKey;    private float max_jump_velocity, min_jump_velocity, gravity;    private bool willJump,willDash,isDashing,canDash = false;
    private float dashTime = .15f,dashPower = 30;
    private Vector3 lastPos;
    private float teleportRange = 4f;
    [SerializeField] private bool canTeleport = false, isTeleporting = false;

    private int countOfAdditionalJumps, maxCountOfAdditionalJumps = 1;
    float BlockValue = 2;    bool isJumpJustPressed,isJumpJustReleased, isDashJustPressed, isTeleportJustPressed;    private void Start()    {        Move[(int)Player.State.IDLE] = MoveInIdle;        Move[(int)Player.State.WALK] = MoveInWalk;        Move[(int)Player.State.JUMP] = MoveInJump;        Move[(int)Player.State.FALL] = MoveInFall;        Move[(int)Player.State.DASH] = MoveInDash;

        jumpKey = KeyCode.C;        dashKey = KeyCode.Z;        teleportKey = KeyCode.X;        countOfAdditionalJumps = maxCountOfAdditionalJumps;        rigidbody = GetComponent<Rigidbody2D>();        collider = GetComponent<Collider2D>();        velocity = new Vector2();        Player.OnCollisionWithCeilingEnterEvent.AddListener(OnCollisionCeiling);        Player.OnCollisionWithFloorStayEvent.AddListener(OnCollisionFloorStay);        Player.OnCollisionWithFloorExitEvent.AddListener(OnCollisionFloorExit);    }
    private void CalculatePhysicsVariables()
    {
        gravity = 2 * max_jump_height * BlockValue / (jumping_time * jumping_time);        min_jump_velocity = Mathf.Sqrt(gravity * min_jump_height * BlockValue);        max_jump_velocity = Mathf.Sqrt(gravity * max_jump_height * BlockValue);
    }    /// <summary>    /// Move Player and take a state in parameters    /// </summary>    /// <param name="state" description="Player's State after moving"></param>    public void MovePlayer(out Player.State state,ref float directionByX)    {
        isJumpJustPressed = Input.GetKeyDown(jumpKey);
        isJumpJustReleased = Input.GetKeyUp(jumpKey);
        isDashJustPressed = Input.GetKeyDown(dashKey);        CalculatePhysicsVariables();        UpdateCheckDependencies(out directionByX);        state = GetState(ref directionByX);        Move[(int)state]();                rigidbody.velocity = velocity;        lastPos = transform.position;    }    public void OnCollisionCeiling() => velocity.y = 0;    public void OnCollisionFloorStay()
    {
        isGrounded = true;
        countOfAdditionalJumps = maxCountOfAdditionalJumps;
        canDash = true;
        canTeleport = true;
    }    public void OnCollisionFloorExit() => isGrounded = false;    private void UpdateCheckDependencies(out float directionByX)
    {        if (isJumpJustPressed && isGrounded) velocity.y = max_jump_velocity;
        if (isJumpJustPressed && !isGrounded && countOfAdditionalJumps > 0)
        {
            velocity.y = max_jump_velocity;
            countOfAdditionalJumps--;
        }
        if (isDashJustPressed && canDash) willDash = true;        if (isTeleportJustPressed && !isTeleporting && canTeleport) {canTeleport = false; StartCoroutine(TeleportPlayer()); }        directionByX = Input.GetAxisRaw("Horizontal");
    }    private Player.State GetState(ref float directionByX)    {
        if (willDash) return Player.State.DASH;        else if (!isGrounded && velocity.y >= 0) return Player.State.JUMP;        else if (!isGrounded && velocity.y < 0) return Player.State.FALL;        else if (directionByX != 0) return Player.State.WALK;        else return Player.State.IDLE;    }    private IEnumerator TeleportPlayer()
    {
        isTeleporting = true;
        Vector3 direction = GetDirectionByArrows();
        direction.Normalize();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + direction * teleportRange, collider.bounds.size * 0.9f, 0);
        if (colliders.Length > 0) print("You can't teleport there");
        else
        {
            transform.position += (direction * teleportRange);
            print(direction * teleportRange);
        }
        yield return new WaitForEndOfFrame();
        isTeleporting = false;
    }    private void ControlGravity()
    {
        if (lastPos.y == transform.position.y && velocity.y < 0) { velocity.y = 0; return; }
        velocity.y -= gravity * Time.fixedDeltaTime;        velocity.y = Mathf.Clamp(velocity.y, -dashPower, max_jump_velocity);
    }    private void CheckHorizontalMove()
    {
        velocity.x = Input.GetAxis("Horizontal") * speed;
    }    private void MoveInWalk()
    {
        CheckHorizontalMove();
        ControlGravity();
    }    private void MoveInIdle() { ControlGravity(); CheckHorizontalMove(); }    private void MoveInAir() {        MoveInWalk();                if (isJumpJustReleased && velocity.y > min_jump_velocity)        {            velocity.y = min_jump_velocity;        }    }    private void MoveInJump()    {        MoveInAir();    }    private void MoveInFall()    {        MoveInAir();    }    private void MoveInDash() {
        if (!isDashing)
        {
            isDashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }    }    IEnumerator Dash()
    {
        Vector2 direction = GetDirectionByArrows();
        direction = direction.normalized * dashPower ;
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
    }    public void CheckInput()
    {
        
        isTeleportJustPressed = Input.GetKeyDown(teleportKey);
    }}