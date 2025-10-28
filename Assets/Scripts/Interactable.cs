using UnityEngine;

public class Interactable : MonoBehaviour
{
    // State variables
    [Header("States")]
    public bool interactable;
    public bool grabable;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public bool touchingFakecast;
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

    void OnTriggerStay(Collider other)
    {
        // Check for fakecast contact
        if (other.CompareTag("Fakecast"))
        {
            touchingFakecast = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check for absence of fakecast contact
        if (other.CompareTag("Fakecast"))
        {
            touchingFakecast = false;
        }
    }
}