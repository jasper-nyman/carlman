using UnityEngine;

public class Interactable : MonoBehaviour
{
    // State variables
    [Header("States")]
    public bool interactable;
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
            interactable = false;
        }
        else
        {
            // Enable interaction when not grabbed
            interactable = true;
        }
    }
}