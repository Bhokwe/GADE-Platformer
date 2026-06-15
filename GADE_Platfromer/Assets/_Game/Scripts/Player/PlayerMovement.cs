using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    public PlayerInput playerInput; 
    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction lookAction;
    public InputAction dashAction;
    public Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed;

    [Header("Jump Settings")]
    public float jumpForce;
    public bool canDoubleJump; 

    [Header("Ground Check Settings")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public bool isGrounded;

    [Header("Dash Settings")]
    public float dashForce;
    public float dashDuration;
    public float dashCooldown;
    public bool isDashing; 
    float dashTimeTracker;
    float dashCooldownTracker;

    [Header("Look Settings")]
    public float mouseSens = 20f;
    public Transform cameraTarget;
    float xRotation = 0f;

    //NEW STATE MACHINE VARIABLES 
    [Header("Animation")]
    public Animator animator;
    private IPlayerState currentState;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        lookAction = playerInput.actions.FindAction("Look");
        dashAction = playerInput.actions.FindAction("Dash");

        // Start the State Machine
        ChangeState(new IdleState());
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (animator != null) animator.SetBool ("isGrounded", isGrounded);

        PlayerLook();

        if (isGrounded && !isDashing)
        {
            canDoubleJump = true;
        }

        if (dashCooldownTracker > 0) dashCooldownTracker -= Time.deltaTime;

        if (isDashing)
        {
            dashTimeTracker -= Time.deltaTime;
            if (dashTimeTracker <= 0)
            {
                isDashing = false;
            }
        }

        if (!isDashing)
        {
            PlayerMovement();
        }

        if (dashAction != null && dashAction.WasPressedThisFrame() && dashCooldownTracker <= 0 && !isDashing)
        {
            StartDash();
        }

        if (jumpAction.WasPressedThisFrame())
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

        // --- RUN THE STATE MACHINE ---
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    void PlayerMovement()
    {
        Vector2 inputDirection = moveAction.ReadValue<Vector2>();

        if (animator != null)
        {
            animator.SetFloat("Speed", inputDirection.magnitude);
        }

        Vector3 moveDirection = (transform.forward * inputDirection.y + transform.right * inputDirection.x).normalized;

        if (inputDirection.magnitude >= 0.1f)
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
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
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

        Vector2 inputDirection = moveAction.ReadValue<Vector2>();
        Vector3 dashDirection = (transform.forward * inputDirection.y + transform.right * inputDirection.x).normalized;

        if (dashDirection == Vector3.zero)
        {
            dashDirection = transform.forward;
        }

        rb.linearVelocity = new Vector3(dashDirection.x * dashForce, 0f, dashDirection.z * dashForce);
    }

    // --- STATE MACHINE HELPER ---
    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }
}