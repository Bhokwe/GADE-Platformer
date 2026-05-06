using UnityEngine;

public class JumpState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        if (player.animator != null) player.animator.CrossFade("Jump", 0.1f);
    }

    public void UpdateState(PlayerController player)
    {
        // If we land, decide whether to walk or idle
        if (player.isGrounded && player.rb.linearVelocity.y <= 0.1f)
        {
            Vector2 input = player.moveAction.ReadValue<Vector2>();
            if (input.magnitude > 0.1f)
            {
                player.ChangeState(new WalkState());
            }
            else
            {
                player.ChangeState(new IdleState());
            }
        }
    }
}