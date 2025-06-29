using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource menuMusic;  // Background music
    public AudioSource clickSound; // Button click sound

    void Start()
    {
        // Start playing menu music if it's not already playing
        if (menuMusic != null && !menuMusic.isPlaying)
        {
            menuMusic.loop = true; // Ensures it loops
            menuMusic.Play();
        }
    }

    public void PlayClickSound()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
    }

    public void menuNewGame()
    {
        PlayClickSound();
        SceneManager.LoadScene("Open World"); // Replace with actual game scene name
    }

    public void menuSettings()
    {
        PlayClickSound();
        Debug.Log("Opening Settings Menu...");
        // Open settings UI panel here
    }

    public void menuLoad()
    {
        PlayClickSound();
        Debug.Log("Loading game...");
        // Implement save/load logic here
    }

    public void menuExit()
    {
        PlayClickSound();
        Debug.Log("Exiting Game...");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in Unity Editor
#else
            Application.Quit(); // Exits the game
#endif
    }
}
