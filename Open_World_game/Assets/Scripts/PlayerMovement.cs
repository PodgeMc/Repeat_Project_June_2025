using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    // Input System Callback for Movement
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void HandleMovement()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * movementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + transform.TransformDirection(movement));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Respawn"))
        {
            Debug.Log("Player entered respawn trigger.");
            //RespawnPlayer(gameObject);
        }
    }

    //void RespawnPlayer(GameObject player)
    //{
    //    RespawnManager respawnManager = FindObjectOfType<RespawnManager>();

    //    if (respawnManager != null)
    //    {
    //        Debug.Log("Respawning player...");
    //        player.transform.position = respawnManager.respawnPoint.position;
    //        Debug.Log("Player respawned at: " + respawnManager.respawnPoint.position);
    //    }
    //    else
    //    {
    //        Debug.LogError("RespawnManager not found in the scene. Cannot respawn player.");
    //    }
    //}
}
