using UnityEngine;

public class JumpState : IPlayerState
{
    public void EnterState(PlayerMove player)
    {
        player.animator.CrossFade("Jump", 0.1f);

        Rigidbody rb = player.GetComponent<Rigidbody>();
        float jumpForce = 7f;

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public void UpdateState(PlayerMove player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();

        player.ChangeState(new IdleState());

        if (player.isGrounded && rb. linearVelocity.y <= 0.1f)
        {
            player.ChangeState(new IdleState());
        }
    }
}
