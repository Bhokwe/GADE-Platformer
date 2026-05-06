using UnityEngine;

public interface IPlayerState
{
    void EnterState(PlayerMove player);

    void UpdateState(PlayerMove player);
}
