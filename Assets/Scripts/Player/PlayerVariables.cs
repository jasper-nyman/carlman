using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    // References
    [Header("References")]
    public Rigidbody rb;
    public GameObject cam;

    // Player state variables
    [Header ("States")]
    public bool active;
    public bool grounded;
    public bool canMove;
    public bool canSprint;
    public bool canJump;
    public bool canCrouch;
    public bool canLook;
    public bool canInteract;
    public bool canHold;

    // Player movement variables
    [Header ("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public float lookSensitivity;
    public float cameraRotation;

    void Start()
    {
        Debug.Log("Player variables initialized");
    }
}