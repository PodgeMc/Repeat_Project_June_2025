using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TankPlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float rotationSpeed = 180f;
    public float doubleTapTime = 0.3f; // time window for double-tap W

    Rigidbody rb;
    float moveInput;
    float turnInput;
    float lastWTime;
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
        moveInput = Input.GetAxisRaw("Vertical");   // W=1, S=-1
        turnInput = Input.GetAxisRaw("Horizontal"); // A=-1, D=1

        // Double-tap W to run
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastWTime < doubleTapTime)
                isRunning = true;

            lastWTime = Time.time;
        }

        // Stop running when W is released
        if (Input.GetKeyUp(KeyCode.W))
            isRunning = false;
    }

    void FixedUpdate()
    {
        // Choose walk or run speed
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
