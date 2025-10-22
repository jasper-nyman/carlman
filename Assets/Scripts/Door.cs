using UnityEngine;

public class Door : MonoBehaviour
{
    // References and state variables
    public Animation anim;
    public GameObject hinge;
    public bool locked;
    public bool open;

    // Door rotation variables
    float rotationYStart;
    public float rotationY;
    public float rotationYOffset;

    void Start()
    {
        // Store the initial Y rotation of the door
        rotationYStart = hinge.transform.eulerAngles.y;

        // Set initial door state
        if (open)
        {
            rotationY = -90f; // Door starts open
        }
        else
        {
            rotationY = 0f; // Door starts closed
        }
    }

    void Update()
    {
        // Update the door's rotation
        hinge.transform.eulerAngles = new Vector3(hinge.transform.eulerAngles.x, rotationYStart + rotationY + rotationYOffset, hinge.transform.eulerAngles.z);
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
                    else if (!open && !anim.IsPlaying("DoorClose"))
                    {
                        // Open the door
                        anim.Play("DoorOpen");
                        open = true;
                        Debug.Log("Door opened.");
                    }
                }
                else
                {
                    Debug.Log("Door locked.");
                }
            break;

            case 1:
                // if the player has the key to the door
                // lock/unlock the door
            break;
        }
    }
}
