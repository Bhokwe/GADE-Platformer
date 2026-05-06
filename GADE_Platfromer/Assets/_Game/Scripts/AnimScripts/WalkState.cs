 using UnityEngine;

public class WalkState : IPlayerState
{
    public void EnterState(PlayerMove player)
    {
        
        player.animator.CrossFade("", 0.1f);
    }

    public void UpdateState(PlayerMove player)
    {
        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            player.ChangeState(new JumpState());
            return;
        }
        //Vector3 moveInput = player.GetMovementInput();
        Vector3 horizontalVelocity = new Vector3(player.rb.linearVelocity.x, 0, player.rb.linearVelocity.z);

        if (horizontalVelocity.magnitude <= 0.1f)
        {
            player.ChangeState(new IdleState());
        }
       /* if (moveInput.magnitude <= 0.1f) 
        {
            player.ChangeState(new IdleState());
        }*/
              
    }
}
