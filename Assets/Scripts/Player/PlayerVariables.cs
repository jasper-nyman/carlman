using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    // References
    [Header("References")]
    public Rigidbody rb;
    public GameObject cam;
    public GameObject fakecast;
    

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

    // Player stat variables
    [Header ("Stats")]
    public float health;
    public float stamina;
    public float maxHealth;
    public float maxStamina;
    public float staminaDrainRate;
    public float staminaRegenRate;

    // Player movement variables
    [Header ("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public float lookSensitivity;

    // Do not modify these values directly
    [Header ("Do Not Modify")]
    public float cameraRotation;
    public CanvasVariables cvar;

    void Start()
    {
        cvar = FindFirstObjectByType<Canvas>().GetComponent<CanvasVariables>();
        Debug.Log("Player variables initialized");
    }
}