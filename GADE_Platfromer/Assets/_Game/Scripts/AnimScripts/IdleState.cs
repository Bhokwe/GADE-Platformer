using UnityEngine;

public class IdleState : IPlayerState
{
  public void EnterState(PlayerMove player)
  {
        player.animator.CrossFade("Idle", 0.1f);
  }

    public void UpdateState(PlayerMove player)
    {
        if (Input.GetButtonDown("Jump") && player.isGrounded) 
        {
            player.ChangeState(new JumpState());
            return;
        }

        Vector3 horiztonalVelocity = new Vector3(player.rb.linearVelocity.x, 0, player.rb.linearVelocity.z);

        if (horiztonalVelocity.magnitude > 0.1f)
        {
            player.ChangeState(new WalkState());
        }
        /*if (player.GetMovementInput() != Vector3.zero)
        {
            player.ChangeState(new WalkState());
        }*/
    }

}
