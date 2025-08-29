using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
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

    public GameObject portal_Training;
    public GameObject portal_ReturnTraining;
    public GameObject portal_Level_1;
    public GameObject portal_Level_2;
    public GameObject portal_Level_3;

    Collider lastCollectedThisPress;
    PlayerManager pm;

    void Start()
    {
        if (portal_ReturnTraining) portal_ReturnTraining.SetActive(true); // always active
        if (portal_Training) portal_Training.SetActive(false);
        if (portal_Level_1) portal_Level_1.SetActive(false);
        if (portal_Level_2) portal_Level_2.SetActive(false);
        if (portal_Level_3) portal_Level_3.SetActive(false);

        pm = GetComponent<PlayerManager>();
        pm.UpdateCrystalUI(crystals);  // show starting count
    }

    void Update()
    {
        if (!cam) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactMask, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetKeyDown(interactKey))
            {
                if (hit.collider && hit.collider.CompareTag("Collectible"))
                {
                    if (hit.collider != lastCollectedThisPress)
                    {
                        lastCollectedThisPress = hit.collider;
                        AddCrystal(1);

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

    void AddCrystal(int amount)
    {
        crystals += amount;
        pm.UpdateCrystalUI(crystals);  // update HUD

        if (crystals >= crystalsNeeded && portal_Training && !portal_Training.activeSelf)
        {
            portal_Training.SetActive(true);
            Debug.Log("Training Portal opened!");
        }

        if (crystals >= crystalsForLevel1 && portal_Level_1 && !portal_Level_1.activeSelf)
        {
            portal_Level_1.SetActive(true);
            Debug.Log("Level 1 Portal opened!");
        }

        if (crystals >= crystalsForLevel2 && portal_Level_2 && !portal_Level_2.activeSelf)
        {
            portal_Level_2.SetActive(true);
            Debug.Log("Level 2 Portal opened!");
        }

        if (crystals >= crystalsForLevel3 && portal_Level_3 && !portal_Level_3.activeSelf)
        {
            portal_Level_3.SetActive(true);
            Debug.Log("Level 3 Portal opened!");
        }
    }
}
