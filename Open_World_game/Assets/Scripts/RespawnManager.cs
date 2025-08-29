using UnityEngine;

public class RespawnManager : MonoBehaviour
{

    //Headers used for easy identification in inspector
    [Header("References")]
    public Transform respawnPoint;
    public GameObject player;

    void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the respawn trigger
        if (other.gameObject == player)
        {
            Debug.Log("Player hit respawn trigger.");
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        // Move the player to the respawn point
        player.transform.position = respawnPoint.position;

        if (player.TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = Vector3.zero;

        Debug.Log("Player respawned at " + respawnPoint.position);
    }
}
