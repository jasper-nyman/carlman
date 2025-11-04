using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerVariables var;
    CapsuleCollider capCol;

    float moveSpeed;
    float cameraRotation;

    // Cache input each frame
    float movementX;
    float movementZ;
    bool jumpPressed;

    void Start()
    {
        capCol = GetComponent<CapsuleCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        cameraRotation = 0f;
    }

    void Update()
    {
        if (var.active && var.canMove)
        {
            // --- INPUT ONLY ---

            // Determine movement speed
            if (var.canSprint && Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = var.sprintSpeed;
            }
            else
            {
                moveSpeed = var.walkSpeed;
            }

            // Cache movement input
            movementX = Input.GetAxis("Horizontal");
            movementZ = Input.GetAxis("Vertical");

            // Play movement animation
            if (var.grounded)
            {
                if (movementX != 0f || movementZ != 0f)
                {
                    if (!var.anim.IsPlaying("PlayerMove"))
                    {
                        var.anim.Play("PlayerMove");
                    }
                    else
                    {
                        var.anim["PlayerMove"].speed = moveSpeed / var.walkSpeed / var.moveSpeedDivider;
                    }
                }
                else
                {
                    if (var.anim.IsPlaying("PlayerMove"))
                    {
                        var.anim.Stop("PlayerMove");
                    }

                    var.cameraOffsetY += (0f - var.cameraOffsetY) * (5f * Time.deltaTime);
                }
            }
            else
            {
                if (var.anim.IsPlaying("PlayerMove"))
                {
                    var.anim.Stop("PlayerMove");
                }

                var.cameraOffsetY += (0f - var.cameraOffsetY) * (5f * Time.deltaTime);
            }

            // Cache jump input
            if (Input.GetKeyDown(KeyCode.Space) && var.grounded)
            {
                jumpPressed = true;
            }

            // Crouching
            if (var.canCrouch)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    var.crouching = true;
                }
                else
                {
                    var.crouching = false;
                }
            }
            else
            {
                var.crouching = false;
            }

            if (var.crouching)
            {
                var.moveSpeedDivider = 2;
                capCol.height += (2f - capCol.height) * (10f * Time.deltaTime);
            }
            else
            {
                var.moveSpeedDivider = 1;
                capCol.height += (3f - capCol.height) * (10f * Time.deltaTime);
            }

            var.cameraPositionY = capCol.height - 0.5f;

            // Mouse look (stays in Update)
            if (var.canLook)
            {
                float mouseX = Input.GetAxis("Mouse X") * var.lookSensitivity;
                transform.Rotate(Vector3.up * mouseX);

                float mouseY = Input.GetAxis("Mouse Y") * var.lookSensitivity;
                cameraRotation -= mouseY;
                cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);

                var.cam.transform.localPosition = new Vector3(0f, var.cameraPositionY + var.cameraOffsetY, 0f);
                var.cam.transform.localEulerAngles = new Vector3(cameraRotation, 0f, 0f);
            }

            // Holding objects
            if (var.heldObject != null)
            {
                Grabable grabComp = var.heldObject.GetComponent<Grabable>();
                grabComp.grabbed = true;

                // Throw held object
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    grabComp.grabbed = false;
                    grabComp.rb.constraints = RigidbodyConstraints.None;
                    grabComp.objectCollider.enabled = true;
                    grabComp.rb.useGravity = true;

                    Collider[] playerColliders = GetComponentsInChildren<Collider>();
                    Collider[] objectColliders = var.heldObject.GetComponentsInChildren<Collider>();

                    foreach (var pc in playerColliders)
                    {
                        foreach (var oc in objectColliders)
                        {
                            // Re‑enable collisions between the player and the object
                            Physics.IgnoreCollision(pc, oc, false);
                        }
                    }

                    var.heldObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.up * 7f) + (var.cam.transform.forward * var.objectThrowForce), ForceMode.Impulse);
                    var.heldObject = null;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (var.active && var.canMove)
        {
            // --- PHYSICS ONLY ---

            // Current velocity
            Vector3 velocity = var.rb.linearVelocity;

            // Calculate movement
            Vector3 move = (transform.right * movementX) + (transform.forward * movementZ);
            Vector3 moveVelocity = move.normalized * (moveSpeed / var.moveSpeedDivider);

            // Apply horizontal velocity
            velocity.x = moveVelocity.x;
            velocity.z = moveVelocity.z;
            var.rb.linearVelocity = velocity;

            // Jumping
            if (jumpPressed && var.grounded && var.canJump)
            {
                var.rb.AddForce(Vector3.up * var.jumpForce, ForceMode.Impulse);
                jumpPressed = false;
            }

            // Preserve vertical velocity
            moveVelocity.y = var.rb.linearVelocity.y;
            var.rb.linearVelocity = moveVelocity;

            // Holding objects
            if (var.heldObject != null)
            {
                Rigidbody rb = var.heldObject.GetComponent<Rigidbody>();
                Vector3 holdPoint = var.holdPointObject.transform.position;

                Vector3 direction = holdPoint - rb.position;
                float moveSpeed = 10f;

                rb.linearVelocity = direction * moveSpeed; // physics-driven movement
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, var.cam.transform.rotation, Time.fixedDeltaTime * 10f));

                // Get all colliders on the player (could be capsule, feet, hands, etc.)
                Collider[] playerColliders = GetComponentsInChildren<Collider>();

                // Get all colliders on the object being held (in case it has multiple parts)
                Collider[] objectColliders = var.heldObject.GetComponentsInChildren<Collider>();

                // Loop through every combination of player collider and object collider
                foreach (var pc in playerColliders)
                {
                    foreach (var oc in objectColliders)
                    {
                        // Tell Unity’s physics engine to ignore collisions between this pair
                        Physics.IgnoreCollision(pc, oc, true);
                    }
                }
            }
        }
    }
}