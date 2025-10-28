using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public PlayerVariables var;

    float moveSpeed;
    float cameraRotation;

    // Cache input each frame
    float movementX;
    float movementZ;
    bool jumpPressed;

    void Start()
    {
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
                        var.anim["PlayerMove"].speed = moveSpeed / var.walkSpeed;
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

                    var.heldObject.GetComponent<Rigidbody>().AddForce((gameObject.transform.up * 5f) + (var.cam.transform.forward * var.objectThrowForce), ForceMode.Impulse);
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
            Vector3 moveVelocity = move.normalized * moveSpeed;

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

                // Lerp held object to fakecast position
                var.heldObject.transform.position = Vector3.Lerp(var.heldObject.transform.position, var.fakecast.transform.position - (var.fakecast.transform.up / 2f), Time.deltaTime * 10f);
                var.heldObject.transform.rotation = Quaternion.Lerp(var.heldObject.transform.rotation, var.fakecast.transform.rotation, Time.deltaTime * 10f);
            }
        }
    }
}