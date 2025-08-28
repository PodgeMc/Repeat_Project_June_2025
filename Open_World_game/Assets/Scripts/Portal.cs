using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform teleportTarget; // where the player will be moved

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && teleportTarget)
        {
            other.transform.position = teleportTarget.position;

            // reset velocity if player has a Rigidbody
            if (other.TryGetComponent<Rigidbody>(out var rb))
                rb.velocity = Vector3.zero;

            Debug.Log("Player teleported to " + teleportTarget.position);
        }
    }
}