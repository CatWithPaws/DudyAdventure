using System.Collections;using System.Collections.Generic;using UnityEngine;[RequireComponent(typeof(Rigidbody2D))]public class PlayerMoveComponent : MonoBehaviour{    private const int countOfStates = 5;    private delegate void DMove();    private DMove[] Move = new DMove[countOfStates];        private Rigidbody2D rigidbody;    [SerializeField] private Vector2 velocity = new Vector2();    [SerializeField] private float speed;    [SerializeField] private bool canJump;    [SerializeField] private bool isGrounded;    [SerializeField] private Collider2D collider;    [SerializeField] private LayerMask Detect;    [SerializeField] private float max_jump_height, min_jump_height, jumping_time;    private float max_jump_velocity, min_jump_velocity, gravity;    private bool willJump,willDash,isDashing,canDash = false;
    private float dashTime = .15f,dashPower = 30;

    private int countOfAdditionalJumps, maxCountOfAdditionalJumps = 1;       private void Start()    {        Move[(int)Player.State.IDLE] = MoveInIdle;        Move[(int)Player.State.WALK] = MoveInWalk;        Move[(int)Player.State.JUMP] = MoveInJump;        Move[(int)Player.State.FALL] = MoveInFall;        Move[(int)Player.State.DASH] = MoveInDash;        float BlockValue = 2;        countOfAdditionalJumps = maxCountOfAdditionalJumps;        rigidbody = GetComponent<Rigidbody2D>();        collider = GetComponent<Collider2D>();        velocity = new Vector2();        gravity = 2 * max_jump_height * BlockValue / (jumping_time * jumping_time);        min_jump_velocity = Mathf.Sqrt(gravity * min_jump_height * BlockValue);        max_jump_velocity = Mathf.Sqrt(gravity * max_jump_height * BlockValue);        Player.OnCollisionWithCeilingEnter.AddListener(OnCollisionCeiling);        Player.OnCollisionWithFloorEnter.AddListener(OnCollisionFloorEnter);        Player.OnCollisionWithFloorExit.AddListener(OnCollisionFloorExit);    }    /// <summary>    /// Move Player and take a state in parameters    /// </summary>    /// <param name="state" description="Player's State after moving"></param>    public void MovePlayer(out Player.State state,ref float directionByX)    {        UpdateCheckDependencies(out directionByX);        state = GetState(ref directionByX);        print(state);        Move[(int)state]();                rigidbody.velocity = velocity;    }    public void OnCollisionCeiling() => velocity.y = 0;    public void OnCollisionFloorEnter()
    {
        isGrounded = true;
        countOfAdditionalJumps = maxCountOfAdditionalJumps;
        canDash = true;
    }    public void OnCollisionFloorExit() => isGrounded = false;    private void UpdateCheckDependencies(out float directionByX)
    {        if (Input.GetKeyDown(KeyCode.C) && isGrounded) velocity.y = max_jump_velocity;
        if (Input.GetKeyDown(KeyCode.C) && !isGrounded && countOfAdditionalJumps > 0)
        {
            velocity.y = max_jump_velocity;
            countOfAdditionalJumps--;
        }
        if (Input.GetAxisRaw("Dash") != 0 && canDash) willDash = true;        directionByX = Input.GetAxisRaw("Horizontal");
    }    private Player.State GetState(ref float directionByX)    {
        if (willDash) return Player.State.DASH;        else if (!isGrounded && velocity.y >= 0) return Player.State.JUMP;        else if (!isGrounded && velocity.y < 0) return Player.State.FALL;        else if (directionByX != 0) return Player.State.WALK;        else return Player.State.IDLE;    }    private void ControlGravity()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;        velocity.y = Mathf.Clamp(velocity.y, -dashPower, max_jump_velocity * 4);
    }    private void CheckHorizontalMove()
    {
        velocity.x = Input.GetAxis("Horizontal") * speed;
    }    private void MoveInWalk()
    {
        CheckHorizontalMove();
        ControlGravity();
    }    private void MoveInIdle() { ControlGravity(); CheckHorizontalMove(); }    private void MoveInAir() {        MoveInWalk();                if (Input.GetKeyUp(KeyCode.C) && velocity.y > min_jump_velocity)        {            velocity.y = min_jump_velocity;        }    }    private void MoveInJump()    {        MoveInAir();    }    private void MoveInFall()    {        MoveInAir();    }    private void MoveInDash() {
        if (!isDashing)
        {
            isDashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }    }    IEnumerator Dash()
    {
        Vector2 direction = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
                );
        direction = direction.normalized * dashPower ;
        velocity = direction;
        yield return new WaitForSeconds(dashTime);
        willDash = false;
        isDashing = false;
    }}