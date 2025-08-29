using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI References")]
    public Text healthText;
    public Text crystalsText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
        UpdateCrystalUI(0);   // start at 0 crystals
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth == 0)
        {
            Debug.Log("Player died!");
            SceneManager.LoadScene("EndGame");
        }
    }

    public void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
    }

    public void UpdateCrystalUI(int total)
    {
        if (crystalsText != null)
            crystalsText.text = "Crystals: " + total;
    }
}
