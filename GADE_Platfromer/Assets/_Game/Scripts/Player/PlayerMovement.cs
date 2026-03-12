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
    Rigidbody rb;



    // the settings for the players movement mechanics
    [Header("Movement Settings")]
    public float moveSpeed; //player movement speed

    [Header("Jump Settings")]
    public float jumpForce; //how much force is applied when the player jumps

    [Header("Ground Check Settings")]
    public Transform groundCheck; // to see if the player is on the ground
    public float groundDistance = 0.4f; //the radius of the sphere used to check for the ground
    public LayerMask groundMask;  //the layer for the ground objects
    public bool isGrounded; // this will be used to check if the player is on the ground before allowing them to jump

    //because we changed to a TP camera, we do not need this for now. 
    //[Header("Camera Ref")]
    //public Transform mainCam;

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
        PlayerMovement();


        //when jump is pressed and the player is grounded, the player wil jump
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            Jump();
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

        xRotation = Mathf.Clamp(xRotation, -70f, 70f);
        cameraTarget.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
