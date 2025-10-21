using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    [Header("References")]
    public PlayerVariables var;

    float verticalRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        verticalRotation = 0f;
    }

    void OnTriggerStay(Collider other)
    {
        // Check if the collision is not with a trigger collider
        if (!other.isTrigger)
        {
            var.grounded = true;
            Debug.Log("Player grounded");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collision is not with a trigger collider
        if (!other.isTrigger)
        {
            var.grounded = false;
            Debug.Log("Player no longer grounded");
        }
    }

    void Update()
    {
        if (var.active)
        {
            if (var.canMove)
            {
                // Determine movement speed
                float moveSpeed;

                if (var.canSprint)
                {
                    if (Input.GetKey(KeyCode.LeftShift) && var.canSprint)
                    {
                        moveSpeed = var.sprintSpeed;
                    }
                    else
                    {
                        moveSpeed = var.walkSpeed;
                    }
                }
                else
                {
                    moveSpeed = var.walkSpeed;
                }

                // Get input
                var movementX = Input.GetAxis("Horizontal");
                var movementZ = Input.GetAxis("Vertical");

                // Calculate movement vector
                Vector3 move = (transform.right * movementX) + (transform.forward * movementZ);
                Vector3 moveVelocity = move.normalized * moveSpeed;

                // Jumping
                if (Input.GetKeyDown(KeyCode.Space) && var.grounded && var.canJump)
                {
                    var.rb.AddForce(Vector3.up * var.jumpForce, ForceMode.Impulse);
                    Debug.Log("Player jumped");
                }

                // Preserve vertical velocity and apply movement
                moveVelocity.y = var.rb.linearVelocity.y;
                var.rb.linearVelocity = moveVelocity;

                // Mouse look
                if (var.canLook)
                {
                    // Rotate player horizontally
                    float mouseX = Input.GetAxis("Mouse X") * var.lookSensitivity;
                    transform.Rotate(Vector3.up * mouseX);

                    float mouseY = Input.GetAxis("Mouse Y") * var.lookSensitivity;

                    // Update and clamp vertical rotation
                    verticalRotation -= mouseY;
                    verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

                    // Apply rotation to camera
                    var.cam.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
                }
            }
        }
    }
}
