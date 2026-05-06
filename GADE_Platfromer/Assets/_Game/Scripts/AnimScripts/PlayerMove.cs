using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Animator animator;
    public Rigidbody rb;

    private IPlayerState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(new IdleState());
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MovePlayerPhysically();

        if (currentState != null)
        {
            currentState.UpdateState(this);        
        }

        //Debug.Log("Am I grounded? " + isGrounded);
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    /*public Vector3 GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        return new Vector3(x, 0, z);
    }*/

    public Vector2 GetMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        return new Vector2(x, y);
    }
    public void MovePlayerPhysically()
    {
        Vector2 inputDirection = GetMovementInput();

        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * inputDirection.y + camRight * inputDirection.x).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            rb.linearVelocity = new Vector3(moveDirection.x * 5f, rb.linearVelocity.y, moveDirection.z * 5f);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
            /*Vector3 movement = GetMovementInput();

        movement = movement.normalized;

        float moveSpeed = 5f;
        transform.Translate(movement *  moveSpeed * Time.deltaTime, Space.World);*/

        
    }
}
