using UnityEngine;
using UnityEngine.XR;

public class Grabable : MonoBehaviour
{
    // References
    [Header("References")]
    public Collider objectCollider;
    public Collider objectTrigger;

    // State variables
    [Header("States")]
    public bool grabable;

    // Do not modify these values directly
    [Header("Do Not Modify")]
    public bool grabbed;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Manage gravity based on grab state
        if (grabbed)
        {
            rb.linearVelocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            objectTrigger.enabled = false;
            rb.useGravity = false;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            objectTrigger.enabled = false;
            rb.useGravity = true;
        }
    }
}
