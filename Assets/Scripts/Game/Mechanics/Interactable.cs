using UnityEngine;

public class Interactable : MonoBehaviour
{
    // State variables
    [Header("States")]
    public bool isInteractable;
    public bool grabable;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public bool touchingRaycast;
    public Grabable grabComp;

    void Start()
    {
        // Get grabable script if applicable
        if (grabable)
        {
            grabComp = GetComponent<Grabable>();
        }
        else
        {
            grabComp = null;
        }
    }

    void Update()
    {
        if (grabable && grabComp.grabbed)
        {
            // Disable interaction while grabbed
            isInteractable = false;
        }
        else
        {
            // Enable interaction when not grabbed
            isInteractable = true;
        }
    }
}