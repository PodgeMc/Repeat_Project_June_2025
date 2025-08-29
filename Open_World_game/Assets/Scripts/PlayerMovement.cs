using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;       // walk speed
    public float runSpeed = 8f;        // run speed (hold Shift)
    public float rotationSpeed = 180f; // turn speed (A/D)

    [Header("Jump Settings")]
    public float jumpForce = 5f;       // jump height
    public float jumpCooldown = 0.5f;  // time between jumps
    public float jumpAnimHold = 0.25f; // how long the jump anim stays on

    Rigidbody rb;
    Animator anim;

    float moveInput;    // W/S
    float turnInput;    // A/D
    bool wantsRun;      // Shift
    float lastJumpTime = -999f;
    float jumpAnimTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        // Freeze X and Z rotation so the player can't tip over.
        // Leave Y free so we can turn left/right with code.
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        // Inputs
        moveInput = Input.GetAxisRaw("Vertical");   // W/S
        turnInput = Input.GetAxisRaw("Horizontal"); // A/D
        if (Mathf.Abs(turnInput) < 0.15f) turnInput = 0f; // ignore tiny drift
        wantsRun = Input.GetKey(KeyCode.LeftShift);       // hold Shift to run

        // Jump with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && (Time.time - lastJumpTime) >= jumpCooldown)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            lastJumpTime = Time.time;
            jumpAnimTimer = jumpAnimHold;
        }

        // Animations
        if (anim)
        {
            bool jumping = false;
            if (jumpAnimTimer > 0f) { jumpAnimTimer -= Time.deltaTime; jumping = true; }
            else { jumping = rb.velocity.y > 0.1f; }

            Vector3 horizVel = rb.velocity; horizVel.y = 0f;
            float speed = horizVel.magnitude;

            bool walking = speed > 0.05f && speed < runSpeed;
            bool running = speed >= runSpeed;
            bool idle = speed <= 0.05f && !jumping;

            anim.SetBool("Jumping", jumping);
            anim.SetBool("Running", running);
            anim.SetBool("Walking", walking);
            anim.SetBool("Idle", idle);
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = wantsRun ? runSpeed : walkSpeed;

        // Move forward/back (only if pressing W/S)
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector3 move = transform.forward * moveInput * targetSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + move);
        }

        // Rotate left/right (always allowed with A/D)
        if (Mathf.Abs(turnInput) > 0.01f)
        {
            float yaw = turnInput * rotationSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, yaw, 0f));
        }
    }
}
