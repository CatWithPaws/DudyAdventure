﻿using System.Collections;
    private float dashTime = .15f,dashPower = 30;

    private int countOfAdditionalJumps, maxCountOfAdditionalJumps = 1;
    {
        if (Input.GetKeyDown(KeyCode.C) && !isGrounded && countOfAdditionalJumps > 0)
        {
            velocity.y = max_jump_velocity;
            countOfAdditionalJumps--;
        }
        if (Input.GetAxisRaw("Dash") != 0 && canDash) willDash = true;
    }
        if (willDash) return Player.State.DASH;
    {
        Gizmos.DrawCube(transform.position - new Vector3(0, collider.bounds.extents.y + collider.bounds.size.y /20, 0), new Vector3(collider.bounds.size.x * 0.8f, collider.bounds.size.y / 10, 0));
    }
    private bool IsGrounded()
        return false;
    {
        velocity.y -= gravity * Time.fixedDeltaTime;
    }
    {
        velocity.x = Input.GetAxis("Horizontal") * speed;
    }
    {
        CheckHorizontalMove();
        ControlGravity();
    }
        if (!isDashing)
        {
            isDashing = true;
            canDash = false;
            StartCoroutine(Dash());
        }
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
    }