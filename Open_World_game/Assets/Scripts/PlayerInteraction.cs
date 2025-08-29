using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    //Headers used for easy identification in inspector

    [Header("Interaction")]
    public Camera cam;
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactMask = ~0;

    [Header("Crystals / Goal")]
    public int crystals = 0;
    public int crystalsNeeded = 4;
    public int crystalsForLevel1 = 4;
    public int crystalsForLevel2 = 8;
    public int crystalsForLevel3 = 12;
    public int crystalsForLevel4 = 16;

    public GameObject portal_Training;
    public GameObject portal_ReturnTraining;
    public GameObject portal_Level_1;
    public GameObject portal_Level_2;
    public GameObject portal_Level_3;

    Collider lastCollectedThisPress;
    PlayerManager pm;

    [Header("Audio")]
    public AudioClip pickupSound;
    private AudioSource audioSource;

    void Start()
    {
        if (portal_ReturnTraining) portal_ReturnTraining.SetActive(true); // always active
        if (portal_Training) portal_Training.SetActive(false);
        if (portal_Level_1) portal_Level_1.SetActive(false);
        if (portal_Level_2) portal_Level_2.SetActive(false);
        if (portal_Level_3) portal_Level_3.SetActive(false);

        pm = GetComponent<PlayerManager>();
        pm.UpdateCrystalUI(crystals);  // show starting count

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        // Basic raycast interaction
        if (!cam) return;

        // Create a ray from the camera's position forward
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        // Visualize the ray in the editor for debugging
        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (hit.collider && hit.collider.CompareTag("Collectible"))
                {
                    if (hit.collider != lastCollectedThisPress)
                    {
                        // Collect the crystal
                        lastCollectedThisPress = hit.collider;
                        AddCrystal(1);


                        if (pickupSound != null)
                            audioSource.PlayOneShot(pickupSound, 0.05f);

                        Debug.Log("Picked up " + hit.collider.gameObject.name + "! Total = " + crystals);

                        Destroy(hit.collider.gameObject);
                    }
                }
            }
        }

        if (Input.GetKeyUp(interactKey))
        {
            lastCollectedThisPress = null;
        }
    }

    // Call this to add crystals and check for portal unlocks
    void AddCrystal(int amount)
    {
        crystals += amount;
        pm.UpdateCrystalUI(crystals);  // update HUD

        // Training portal unlock
        if (crystals >= crystalsNeeded && portal_Training && !portal_Training.activeSelf)
        {
            portal_Training.SetActive(true);
            Debug.Log("Training Portal opened!");
        }

        // Return portal always active
        if (crystals >= crystalsForLevel1 && portal_Level_1 && !portal_Level_1.activeSelf)
        {
            portal_Level_1.SetActive(true);
            Debug.Log("Level 1 Portal opened!");
        }

        // Level 2 portal unlock
        if (crystals >= crystalsForLevel2 && portal_Level_2 && !portal_Level_2.activeSelf)
        {
            portal_Level_2.SetActive(true);
            Debug.Log("Level 2 Portal opened!");
        }

        // Level 3 portal unlock
        if (crystals >= crystalsForLevel3 && portal_Level_3 && !portal_Level_3.activeSelf)
        {
            portal_Level_3.SetActive(true);
            Debug.Log("Level 3 Portal opened!");
        }

        if (crystals >= crystalsForLevel4)
        {
            Debug.Log("All portals opened! You have collected all crystals!");
            SceneManager.LoadScene("YouWin");
        }
    }
}
