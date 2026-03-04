using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // reference for the player's inptu action and movement
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    Rigidbody rb;



    // the settings for the players movement mechanics
    [Header("Movement Settings")]
    public float moveSpeed;

    [Header("Jump Settings")]
    public float jumpForce; 


    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    void PlayerMovement()
    {

        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;

    }

    void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); // reset the y velocity to ensure consistent jump height
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
