using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    // References
    [Header("References")]
    GameObject parent;
    Interactable intComp;
    public Animation anim;
    public GameObject hinge;
    public GameObject key;

    // State variables
    [Header("States")]
    public bool locked;
    public bool open;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    float rotationYStart;
    public float rotationY;

    void Start()
    {
        // Get references
        parent = transform.parent.gameObject;
        intComp = GetComponent<Interactable>();

        // Store the initial Y rotation of the door
        rotationYStart = hinge.transform.eulerAngles.y;

        // Set initial door state
        if (open)
        {
            rotationY = -90f * parent.transform.lossyScale.x; // Door starts open
        }
        else
        {
            rotationY = 0f; // Door starts closed
        }
    }

    public void InteractionEvent(int eventNumber)
    {
        switch (eventNumber)
        {
            case 0:
                if (!locked)
                {
                    if (open && !anim.IsPlaying("DoorOpen"))
                    {
                        // Close the door
                        anim.Play("DoorClose");
                        open = false;
                        Debug.Log("Door closed.");
                    }
                    
                    if (!open && !anim.IsPlaying("DoorClose"))
                    {
                        // Open the door
                        anim.Play("DoorOpen");
                        open = true;
                        Debug.Log("Door opened.");
                    }
                }
                else
                {
                    if (open && !anim.IsPlaying("DoorOpen"))
                    {
                        // Close the door
                        anim.Play("DoorClose");
                        open = false;
                        Debug.Log("Door closed.");
                    }

                    if (!open && !anim.IsPlaying("DoorClose"))
                    {
                        // Open the door
                        anim.Play("DoorOpen");
                        open = true;
                        Debug.Log("Door opened.");
                    }
                }
            break;

            case 1:
                // if the player has the key to the door
                // lock/unlock the door
            break;
        }
    }

    void Update()
    {
        // Update the door's rotation
        hinge.transform.eulerAngles = new Vector3(hinge.transform.eulerAngles.x, rotationYStart + rotationY * parent.transform.lossyScale.x, hinge.transform.eulerAngles.z);

        // Check for interaction input
        if (intComp.interactable && intComp.touchingFakecast)
        {
            if (Input.GetMouseButtonDown(0))
            {
                InteractionEvent(0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                InteractionEvent(1);
            }
        }
    }
}
