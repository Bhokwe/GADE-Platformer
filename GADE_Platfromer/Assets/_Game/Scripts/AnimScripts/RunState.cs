using UnityEngine;

public class RunState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // Triggered when dashing. Make sure "Run" matches your Animator!
        if (player.animator != null) player.animator.CrossFade("Run", 0.1f);
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.isGrounded)
        {
            player.ChangeState(new JumpState());
            return;
        }

        // Go back to walk when dash is over
        if (!player.isDashing)
        {
            player.ChangeState(new WalkState());
        }
    }
}