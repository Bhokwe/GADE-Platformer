using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    // reference for the player's inptu action and movement
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction lookAction;
    InputAction dashAction;
    Rigidbody rb;



    // the settings for the players movement mechanics
    [Header("Movement Settings")]
    public float moveSpeed; //player movement speed

    [Header("Jump Settings")]
    public float jumpForce; //how much force is applied when the player jumps
    bool canDoubleJump; // can the player double jump?

    [Header("Ground Check Settings")]
    public Transform groundCheck; // to see if the player is on the ground
    public float groundDistance = 0.4f; //the radius of the sphere used to check for the ground
    public LayerMask groundMask;  //the layer for the ground objects
    public bool isGrounded; // this will be used to check if the player is on the ground before allowing them to jump

    [Header("Dash Settings")]
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    bool isDashing;
    float dashTimeTracker;
    float dashCooldownTracker;

    [Header("Look Settings")]
    public float mouseSens = 20f; //mouse sensitivity for looking around
    public Transform cameraTarget; //slot for the cameraTarget empty in the player
    float xRotation = 0f; //up&down tilt of the camera



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //getting the components 
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        //the move, jump and look actions found in the IA asset
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        lookAction = playerInput.actions.FindAction("Look");
        dashAction = playerInput.actions.FindAction("Dash");

        //locking of cursor
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;

    }

    // Update is called once per frame
    void Update()
    {
        // checks if the player is on the ground by checking for collisions with the ground layer
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 
        PlayerLook();
        //PlayerMovement();


        if (isGrounded && !isDashing)
        {
            canDoubleJump = true; //reset double jump when the player is grounded again
        }
        //start counting down the dash timer
        if (dashCooldownTracker > 0) dashCooldownTracker -= Time.deltaTime;

        //Tick down the active dash timer and end the dash after  the time is up
        if (isDashing) 
        {
            dashTimeTracker -= Time.deltaTime;
            if (dashTimeTracker <= 0)
            {
                isDashing = false;//dash is over
            }
        
        }
        if (!isDashing) 
        {
            PlayerMovement();
        }
        //Dash trigger
        if (dashAction != null && dashAction.WasPressedThisFrame() && dashCooldownTracker <= 0&& !isDashing) 
        {
            StartDash();
        }

        

        //when jump is pressed and the player is grounded, the player wil jump
        if (jumpAction.WasPressedThisFrame())
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump) 
            { 
                Jump();
                canDoubleJump = false; //prevents the player from jumping into oblivion
            }

            
        }
    }

    void PlayerMovement()
    {
        //reading the raw input
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
     
        Vector3 moveDirection = (transform.forward*inputDirection.y + transform.right * inputDirection.x).normalized;


        //rotate only when the player is inputting that direction
        if (inputDirection.magnitude>=0.1f)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);


        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }

    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // reset the y velocity to ensure consistent jump height
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void PlayerLook()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * lookInput.x * mouseSens * Time.deltaTime);

        xRotation -= lookInput.y * mouseSens * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -20f, 20f);
        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void StartDash()
    {
        isDashing = true;
        dashTimeTracker = dashDuration;
        dashCooldownTracker = dashCooldown;

        // Figure out which way the player is currently trying to move
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 dashDirection = (transform.forward * inputDirection.y + transform.right * inputDirection.x).normalized;

        // If they aren't pressing any keys, default to dashing straight forward
        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        // Force a massive burst of speed, zeroing out the Y velocity so the dash shoots perfectly straight
        rb.linearVelocity = new Vector3(dashDirection.x * dashForce, 0f, dashDirection.z * dashForce);
    }
}
