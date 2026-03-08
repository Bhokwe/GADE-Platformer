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

    [Header("Camera Ref")]
    public Transform mainCam;

    [Header("Rotation Settings")]
    public float turnSmoothTime; //how fast the player rotates
    float turnSmoothVelocity; // smoothen the rotation 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //getting the components 
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        //the move and jump actions found in the IA asset
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

    }

    // Update is called once per frame
    void Update()
    {
        // checks if the player is on the ground by checking for collisions with the ground layer
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); 
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
        //converting it to a 3D direction vector
        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        //Vector2 direction = moveAction.ReadValue<Vector2>(); switch back to it if something breaks
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;

        //rotate only when the player is inputting that direction
        if (direction.magnitude>=0.1f)
        {
            //eyy this math bro but anyway, this calculates the anle the character needs to face
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCam.eulerAngles.y;

            //eases the current rotation smoothly towards the target angle
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //applies the rotation to the player
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;


            //changes forward to the new input direction
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;


        }

    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // reset the y velocity to ensure consistent jump height
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
