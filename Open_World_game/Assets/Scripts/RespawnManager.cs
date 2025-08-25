using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [Header("References")]
    public Transform respawnPoint;   // where to put the player back
    public GameObject player;        // drag your Player here in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player hit respawn trigger.");
            RespawnPlayer();
        }
    }

    void RespawnPlayer()
    {
        player.transform.position = respawnPoint.position;

        if (player.TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = Vector3.zero; // stop any leftover movement

        Debug.Log("Player respawned at " + respawnPoint.position);
    }
}
