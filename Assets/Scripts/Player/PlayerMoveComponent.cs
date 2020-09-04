using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMoveComponent : MonoBehaviour
{
    private const int countOfStates = 10;
    //Move Delegate  DMove
    private delegate void DMove();
    private DMove[] Move = new DMove[countOfStates];

    //Delegate for movings
    public delegate void CustomEvent();
    public CustomEvent OnPlayerCollisionWithCeilingEnterEvent;
    public CustomEvent OnPlayerCollisionWithCeilingExitEvent;
    public CustomEvent OnPlayerCollisionWithFloorStayEvent;
    public CustomEvent OnPlayerCollisionWithFloorExitEvent;
    public CustomEvent OnPlayerCollisionWithWallExitEvent;
    public delegate void CustomIntEvent(int data);

    public  CustomIntEvent OnPlayerCollisionWithWallStayEvent;

    private Player player;
    [SerializeField] private Vector2 velocity = new Vector2();
    [SerializeField] private float speed;

    [SerializeField] private LayerMask Detect;
    [SerializeField] private float max_jump_height, min_jump_height, jumping_time;
    private Rigidbody2D rigidbody;
    private KeyCode jumpKey, dashKey, teleportKey, runKey;
    private Vector3 lastPos;
    private Collider2D collider;
    [SerializeField]
    private bool isGrounded;
    private bool willDash, isDashing, canDash = true;
    private bool isTeleporting = false;
    private bool isJumpJustPressed, isJumpJustReleased, isDashJustPressed, isTeleportJustPressed, isRightPressed, isLeftPressed;
    private bool isInDialogue = false;
    public bool canTeleport { get; private set; } = true;
    private float max_jump_velocity, min_jump_velocity, gravity;
    private float dashTime = .15f, dashPower = 30;
    private float teleportRange = 8f;
    private float BlockValue = 2;
    private float lastDirectionByX = 1;
    private float directionByX;
    private int countOfAdditionalJumps, maxCountOfAdditionalJumps = 0;
    private bool isHorizontalMoveDelayActive;
    private float dashCallDown = .2f, teleportCallDown = .2f;
    private bool isDashInCallDown = false, isTeleportInCallDown = false;

    private bool isCollidingWithWall;
    private bool isCollidingWithCeiling = false;
    [SerializeField]private int WallPositionInX;
    private float speedMultiplier = 1f;

    [SerializeField]
    private bool isDashUnlocked;
    [SerializeField]
    private bool isTeleportUnlocked;
    [SerializeField] private GameObject TeleportPreview;
    bool isWallJumping = false;

    



    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        Move[(int)Player.State.IDLE] = MoveInIdle;
        Move[(int)Player.State.WALK] = MoveInWalk;
        Move[(int)Player.State.JUMP] = MoveInJump;
        Move[(int)Player.State.FALL] = MoveInFall;
        Move[(int)Player.State.DASH] = MoveInDash;
        Move[(int)Player.State.DIALOG] = MoveInDialog;
        Move[(int)Player.State.WALL_JUMP] = MoveInWallJump;
        Move[(int)Player.State.SLIDE_ON_WALL] = MoveInSlideOnWall;

        jumpKey = KeyCode.C;
        dashKey = KeyCode.X;
        teleportKey = KeyCode.Z;
        runKey = KeyCode.LeftShift;
        player = GetComponent<Player>();

        countOfAdditionalJumps = maxCountOfAdditionalJumps;
        velocity = new Vector2();
        OnPlayerCollisionWithCeilingEnterEvent += OnCollisionCeilingEnter;
        OnPlayerCollisionWithCeilingExitEvent += OnCollisionCeilingExit;
        OnPlayerCollisionWithFloorStayEvent += OnCollisionFloorStay;
        OnPlayerCollisionWithFloorExitEvent += OnCollisionFloorExit;
        EventHolder.OnGUIDialogStarted.AddListener(OnDialogStarted);
        EventHolder.OnGUIDialogEnded.AddListener(OnDialogEnded);
        OnPlayerCollisionWithWallStayEvent += OnCollideWithWall;
        OnPlayerCollisionWithWallExitEvent += OnExitCollideWithWall;

        CalculatePhysicsVariables();

        dashPower = max_jump_velocity;
    }



    private void CalculatePhysicsVariables()
    {
        gravity = 2 * max_jump_height * BlockValue / (jumping_time * jumping_time);
        min_jump_velocity = Mathf.Sqrt(gravity * min_jump_height * BlockValue);
        max_jump_velocity = Mathf.Sqrt(gravity * max_jump_height * BlockValue);
    }

    private Player.State GetState(ref float directionByX)
    {
		if (isInDialogue) return Player.State.DIALOG;
		else if (willDash) return Player.State.DASH;
		else if (isWallJumping) return Player.State.WALL_JUMP;
		else if (isCollidingWithWall && !isGrounded) return Player.State.SLIDE_ON_WALL;
		else if (!isGrounded && velocity.y >= 0) return Player.State.JUMP;
		else if (!isGrounded && velocity.y < 0) return Player.State.FALL;
		else if (directionByX != 0) return Player.State.WALK;
		else return Player.State.IDLE;
	}

	public bool CanBGMove()
	{
        return directionByX == lastDirectionByX && isCollidingWithWall;
	}

    /// <summary>
    /// Move Player and take a state in parameters
    /// </summary>
    /// <param name="state" description="Player's State after moving"></param>
    public void MovePlayer(out Player.State state, ref float directionByX)
    {
       
        //speedMultiplier = Input.GetKey(runKey) ? 1 + 1f / 3f : 1;
        state = GetState(ref directionByX);
        Move[(int)state]();
        TeleportPreview.transform.position = transform.position + GetTeleportPosition();
        rigidbody.velocity = velocity;
        lastPos = transform.position;
        lastDirectionByX = directionByX != 0 ? directionByX : lastDirectionByX;
    }
    private void MoveInWalk()
    {

        CheckHorizontalMove();
        ControlGravity();
    }
    private void MoveInIdle()
    {
        ControlGravity();
        CheckHorizontalMove();

    }
    private void MoveInAir()
    {
        AirControll();
        ControlGravity();
        if (isJumpJustReleased && velocity.y > min_jump_velocity) velocity.y = min_jump_velocity;
    }
    private void MoveInJump()
    {
        MoveInAir();
    }
    private void MoveInFall()
    {
        MoveInAir();
    }

    private void MoveInDialog()
    {
        velocity.x = 0;
        ControlGravity();
    }
    private void MoveInWallJump()
    {
    }

    private void AirControll()
    {
        velocity.x = Mathf.Lerp(velocity.x, GetHorizontalDirection() * speed * speedMultiplier, 0.9f);
    }
    public  void UpdateCheckDependencies(ref float directionByX)
    {
        directionByX = Input.GetAxisRaw("Horizontal");

    }
    public void OnWallStay(int direction)
    {

    }
    private IEnumerator TeleportPlayer()
    {
        yield return new WaitForFixedUpdate();
        canTeleport = false;
        isTeleporting = true;
        Vector3 direction = GetTeleportPosition();

        if (IsCollidersInArea(ref direction))
            canTeleport = true;
        else
            transform.position += (direction);
        isTeleporting = false;
        isTeleportInCallDown = true;
        yield return new WaitForSeconds(teleportCallDown);
        isTeleportInCallDown = false;
    }
    private Vector3 GetTeleportPosition()
    {
        Vector3 teleportPosition = GetDirectionByArrows();
        teleportPosition = teleportPosition == Vector3.zero ? new Vector3(lastDirectionByX, 0) : teleportPosition;
        teleportPosition.Normalize();
        teleportPosition *= teleportRange;
        teleportPosition.x = Mathf.Ceil(teleportPosition.x);
        teleportPosition.y = Mathf.Ceil(teleportPosition.y);
        return teleportPosition;
    }

    private bool IsCollidersInArea(ref Vector3 direction)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + direction, collider.bounds.size * 0.9f, 0);
        return colliders.Length > 0;
    }

    private void ControlGravity()
    {
        if (lastPos.y == transform.position.y && velocity.y < 0) { velocity.y = 0; return; }
        velocity.y -= gravity * Time.fixedDeltaTime;
        velocity.y = Mathf.Clamp(velocity.y, -max_jump_velocity, max_jump_velocity);
    }

    private void CheckHorizontalMove()
    {
        int horizontalDirection = GetHorizontalDirection();
        velocity.x = Mathf.Lerp(velocity.x, horizontalDirection * speed * speedMultiplier, 0.5f);
    }

    private int GetHorizontalDirection()
    {
        return Convert.ToInt32(isRightPressed) - Convert.ToInt32(isLeftPressed);
    }


    private IEnumerator StartDash() {
        yield return new WaitForFixedUpdate();
        willDash = true;
    }

    private void MoveInDash() {
        if (!isDashing)
        {
            isDashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }
    }
    private void MoveInSlideOnWall()
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
        if (velocity.y <= -gravity / 7f)
        {
            velocity.y = -gravity / 7f;
        }
        if (!isHorizontalMoveDelayActive && Input.GetAxisRaw("Horizontal") != 0)
        {
            StartCoroutine(HorizontalMoveDelay());
        }
        
    }

    private IEnumerator HorizontalMoveDelay()
    {
        float timer = 0;
        if (!isGrounded)
        {
            while (timer < 0.25f)
            {
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) != 1) break;
                
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
            }
        }
        isCollidingWithWall = false;
        
    }

    private IEnumerator WallJump()
    {
        yield return new WaitForFixedUpdate();
        isWallJumping = true;
        canDash = true;
        velocity.y = max_jump_velocity;
        if (!isGrounded) velocity.x = speed * 1.1f * 1.5f * -WallPositionInX;
        StartCoroutine(CheckWallJump());
        isCollidingWithWall = false;
    }

    private IEnumerator CheckWallJump()
    {

        yield return new WaitForSeconds(.1f);
        velocity.y = max_jump_velocity;
        isWallJumping = false;

    }
    private IEnumerator Dash()
    {
        yield return new WaitForFixedUpdate();
        Vector2 direction = new Vector2(GetHorizontalDirection(), 0);
        direction = direction == Vector2.zero ? new Vector2(lastDirectionByX, 0) : direction;
        direction = direction.normalized * dashPower;
        velocity = direction;
        yield return new WaitForSeconds(dashTime * 1.5f);
        willDash = false;
        isDashing = false;
        canDash = false;
        isDashInCallDown = true;
        yield return new WaitForSeconds(dashCallDown);
        isDashInCallDown = false;
    }
    private Vector3 GetDirectionByArrows()
    {
        return new Vector3(
               Input.GetAxisRaw("Horizontal"),
               Input.GetAxisRaw("Vertical")
               );
    }
    private IEnumerator JumpCoroutine()
    {
        yield return new WaitForFixedUpdate();
        Jump();
    }
    private void Jump()
    {
        velocity.y = max_jump_velocity;
    }

    private void OnDialogStarted()
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
    }

    public void CatchMove()
    {
        if (isInDialogue) return;
        if (isJumpJustPressed && isGrounded) StartCoroutine(JumpCoroutine());
        if (isJumpJustPressed && isCollidingWithWall && !isGrounded && !isCollidingWithCeiling) StartCoroutine(WallJump());
        if (isJumpJustPressed && !isGrounded && countOfAdditionalJumps > 0 && !isInDialogue)
        {
            StartCoroutine(JumpCoroutine());
            countOfAdditionalJumps--;
        }
        if (isDashJustPressed && canDash && !isDashInCallDown && isDashUnlocked) StartCoroutine(StartDash());
        if (isTeleportJustPressed && !isTeleporting && canTeleport && !isTeleportInCallDown && isTeleportUnlocked) StartCoroutine(TeleportPlayer());
    }
    public void CheckInput()
    {
        isTeleportJustPressed = Input.GetKeyDown(teleportKey);

        isJumpJustPressed = Input.GetKeyDown(jumpKey);
        isJumpJustReleased = Input.GetKeyUp(jumpKey);
        isDashJustPressed = Input.GetKeyDown(dashKey);
        isRightPressed = Input.GetKey(KeyCode.RightArrow);
        isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
    }
    public void OnCollisionCeilingEnter() {
        print("ceiling enter");
        velocity.y = 0;
        isCollidingWithCeiling = true;
    }
    public void OnCollisionCeilingExit()
    {
        print("ceiling exit");
        isCollidingWithCeiling = false;
    }
    public void OnCollisionFloorStay()
    {
        isGrounded = true;
        countOfAdditionalJumps = maxCountOfAdditionalJumps;
        canDash = true;
        canTeleport = true;
    }
    void OnCollideWithWall(int direction)
    {
        isCollidingWithWall = true;
        WallPositionInX = direction;
    }
    void OnExitCollideWithWall()
    {
        isCollidingWithWall = false;
    }
    public void OnCollisionFloorExit() => isGrounded = false;
    public void RestoreTeleport() => canTeleport = true;
}
