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
    public int crystalsNeeded = 10;
    public GameObject portal;

    Collider lastCollectedThisPress;

    void Start()
    {
        if (portal) portal.SetActive(false);
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

                        // Debug output with object name
                        Debug.Log($"Picked up {hit.collider.gameObject.name}! Total = {crystals}/{crystalsNeeded}");

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

        if (crystals >= crystalsNeeded && portal && !portal.activeSelf)
        {
            portal.SetActive(true);
            Debug.Log("Portal opened!");
        }
    }
}
