using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 180f;
    public float doubleTapTime = 0.3f;

    [Header("Jump")]
    public float jumpForce = 7f;
    public float jumpCooldown = 2f; // time (seconds) between jumps

    Rigidbody rb;
    float moveInput;
    float turnInput;
    float lastWTime;
    float lastJumpTime = -999f; // start far in the past
    bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Forward/back (W/S) and rotate (A/D)
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");

        // Double-tap W to run
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastWTime < doubleTapTime)
                isRunning = true;

            lastWTime = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.W))
            isRunning = false;

        // Jump with cooldown
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastJumpTime >= jumpCooldown)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                lastJumpTime = Time.time;
            }
        }
    }

    void FixedUpdate()
    {
        float speed = isRunning ? runSpeed : walkSpeed;

        // Move forward/back
        Vector3 move = transform.forward * moveInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        // Rotate left/right
        float turn = turnInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRot = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRot);
    }
}
