using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && teleportTarget)
        {
            // Teleport the player to the target position
            other.transform.position = teleportTarget.position;

            if (other.TryGetComponent<Rigidbody>(out var rb))
                rb.velocity = Vector3.zero;
            
            Debug.Log("Player teleported to " + teleportTarget.position);
        }
    }
}