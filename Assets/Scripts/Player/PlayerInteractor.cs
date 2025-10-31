using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractor : MonoBehaviour
{
    // References
    [Header("References")]
    public PlayerVariables var;
    Camera cam;
    Image dotImage;
    RawImage handImage;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public GameObject focusedObject;

    // Raycast Settings
    [Header("Raycast Settings")]
    public float raycastDistance;
    public LayerMask raycastLayers = ~0; // optional: set in Inspector

    void Start()
    {
        var = GetComponent<PlayerVariables>();
        cam = Camera.main;
        dotImage = var.cvar.dot.GetComponent<Image>();
        handImage = var.cvar.hand.GetComponent<RawImage>();
    }

    void ClearFocus(string reason = null)
    {
        if (focusedObject != null)
        {
            var intPrev = focusedObject.GetComponent<Interactable>();
            if (intPrev != null) intPrev.touchingRaycast = false;

            focusedObject = null;
            dotImage.enabled = true;
            handImage.enabled = false;

            if (!string.IsNullOrEmpty(reason))
            {
                Debug.Log($"Focus cleared: {reason}");
            }
        }
    }

    void Update()
    {
        // Rebind UI if needed
        if (dotImage == null || handImage == null)
        {
            dotImage = var.cvar.dot.GetComponent<Image>();
            handImage = var.cvar.hand.GetComponent<RawImage>();
        }

        // Perform raycast
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        // Include triggers, but don’t require that the hit be a trigger
        if (Physics.Raycast(ray, out hit, raycastDistance, raycastLayers, QueryTriggerInteraction.Collide))
        {
            // Try resolve Interactable directly, then via parent (covers child-collider setups)
            Interactable intComp = hit.collider.GetComponent<Interactable>();
            if (intComp == null)
            {
                intComp = hit.collider.GetComponentInParent<Interactable>();
            }

            // Diagnostics
            Debug.Log($"Hit '{hit.collider.name}' (isTrigger={hit.collider.isTrigger}) | Interactable found: {(intComp != null ? intComp.name : "null")}");

            if (intComp != null && intComp.isInteractable)
            {
                // If we switched target, clear previous
                if (focusedObject != null && focusedObject != intComp.gameObject)
                {
                    var prev = focusedObject.GetComponent<Interactable>();
                    if (prev != null) prev.touchingRaycast = false;
                }

                // Set new focus
                focusedObject = intComp.gameObject;
                intComp.touchingRaycast = true;

                dotImage.enabled = false;
                handImage.enabled = true;
            }
            else
            {
                // We hit something, but it's not a valid interactable
                ClearFocus(intComp == null ? "No Interactable on hit" : "Interactable not currently interactable");
            }
        }
        else
        {
            // Nothing hit at all
            ClearFocus("Raycast miss");
        }

        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red);

        // Grab input
        if (focusedObject != null && Input.GetMouseButtonDown(0))
        {
            var intComp = focusedObject.GetComponent<Interactable>();
            if (intComp != null && intComp.isInteractable && intComp.grabable && intComp.grabComp != null && intComp.grabComp.grabable)
            {
                var.heldObject = focusedObject;
                Debug.Log($"Grabbed: {focusedObject.name}");
            }
        }
    }
}
