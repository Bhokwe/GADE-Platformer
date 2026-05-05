using UnityEngine;

public abstract class AbstractEnemyFactory : MonoBehaviour
{
    //for concrete enemy factories implementation 
    public abstract BaseEnemy CreateEnemy(string enemyType, Vector3 spawnPosition);
}