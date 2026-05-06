using NUnit.Framework.Interfaces;
using UnityEngine;

public class WalkState : IPlayerState
{
    public void EnterState(PlayerController player)
    {
        // Make sure "Walk" matches the exact name in your Animator!
        if (player.animator != null) player.animator.CrossFade("Walk", 0.1f);
    }

    public void UpdateState(PlayerController player)
    {
        if (!player.isGrounded)
        {
            player.ChangeState(new JumpState());
            return;
        }

        if (player.isDashing)
        {
            player.ChangeState(new RunState());
            return;
        }

        Vector2 input = player.moveAction.ReadValue<Vector2>();
        if (input.magnitude <= 0.1f)
        {
            player.ChangeState(new IdleState());
        }
    }
}