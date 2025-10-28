using UnityEngine;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    // References
    [Header("References")]
    public GameObject key;
    GameObject parent;
    Interactable intComp;

    // State variables
    [Header("States")]
    public bool locked;
    public bool open;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public Animation anim;
    public GameObject hinge;
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
            rotationY = -90f; //* parent.transform.lossyScale.x; // Door starts open
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
                    // Close unlocked door
                    if (open && !anim.IsPlaying("DoorOpen"))
                    {
                        // Close the door
                        anim.Play("DoorClose");
                        open = false;
                        Debug.Log("Door closed. Is unlocked.");
                    }

                    // Open unlocked door
                    if (!open && !anim.IsPlaying("DoorClose"))
                    {
                        // Open the door
                        anim.Play("DoorOpen");
                        open = true;
                        Debug.Log("Door opened. Is unlocked.");
                    }
                }
                else
                {
                    // Close locked door
                    if (open && !anim.IsPlaying("DoorOpen"))
                    {
                        // Close the door
                        anim.Play("DoorClose");
                        open = false;
                        Debug.Log("Door closed. Is locked.");
                    }

                    // Attempt to open locked door
                    if (!open && !anim.IsPlaying("DoorClose"))
                    {
                        Debug.Log("Door not opened. Is locked.");
                    }
                }
            break;

            case 1:
                PlayerVariables var = GameObject.Find("Player").GetComponent<PlayerVariables>();

                if (var.heldObject == key)
                {
                    // Toggle lock state
                    locked = !locked;

                    if (locked)
                    {
                        Debug.Log("Door locked.");
                    }
                    else
                    {
                        Debug.Log("Door unlocked.");
                    }
                }
                else
                {
                    Debug.Log("Cannot toggle lock. Key not held.");
                }
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
            // Left mouse button for opening/closing
            if (Input.GetMouseButtonDown(0))
            {
                InteractionEvent(0);
            }

            // Right mouse button for locking/unlocking
            if (Input.GetMouseButtonDown(1))
            {
                InteractionEvent(1);
            }
        }
    }
}
