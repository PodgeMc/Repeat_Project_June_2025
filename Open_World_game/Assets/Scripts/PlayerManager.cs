using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{

    //Headers used for easy identification in inspector
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Text healthText;
    public Text crystalsText;

    [Header("Audio")]
    public AudioClip damageSound;
    private AudioSource audioSource;

    void Start()
    {
        //starts the player at full health and 0 crystals
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateCrystalUI(0);   // start at 0 crystals

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public void TakeDamage(int amount)
    {
        // Reduces health and makes sure it doesn't go below 0
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        if (damageSound != null && audioSource != null)
            audioSource.PlayOneShot(damageSound);

        UpdateHealthUI();

        if (currentHealth == 0)
        {
            // Handle player death
            Debug.Log("Player died!");
            SceneManager.LoadScene("EndGame");
        }
    }

    public void UpdateHealthUI()
    {
        // Updates the health display
        if (healthText != null)
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void UpdateCrystalUI(int total)
    {
        // Updates the crystal display
        if (crystalsText != null)
            crystalsText.text = "Crystals: " + total;
    }
}
