using UnityEngine;

public class ConcreteEnemyFactory : AbstractEnemyFactory
{
    [Header("Enemy Prefabs")]
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    [Header("Enemy looks")]
    public Material fastEnemyMaterial;
    public Material tankEnemyMaterial;

    [Header("Level Data")]
    public Transform[] waypointObjects;

    public override BaseEnemy CreateEnemy(string enemyType, Vector3 spawnPosition)
    {
        GameObject spawnedEnemy = null;
        BaseEnemy enemyScript = null;

        if (enemyType == "Fast")
        {
            spawnedEnemy = Instantiate(fastEnemyPrefab, spawnPosition, Quaternion.identity);
            enemyScript = spawnedEnemy.GetComponent<FastEnemy>();
        }
        else if (enemyType == "Tank")
        {
            spawnedEnemy = Instantiate(tankEnemyPrefab, spawnPosition, Quaternion.identity);
            enemyScript = spawnedEnemy.GetComponent<TankEnemy>();
        }
        else
        {
            Debug.LogWarning("Factory can't build this enemy type: " + enemyType);
            return null;
        }

        EnemyPatrol patrolScript = spawnedEnemy.GetComponent<EnemyPatrol>();
        if (patrolScript != null)
        {
            patrolScript.waypointObjects = waypointObjects;
        }

        if (enemyType == "Fast")
        {
            enemyScript.SetupEnemy(6f, new Vector3(0.75f, 0.75f, 0.75f), fastEnemyMaterial);
        }
        else
        {
            enemyScript.SetupEnemy(2f, new Vector3(1.5f, 1.5f, 1.5f), tankEnemyMaterial);
        }

        return enemyScript;
    }

    private void Start()
    {
        Vector3 spawnPos1 = transform.position + new Vector3(2f, 1f, 0f);
        Vector3 spawnPos2 = transform.position + new Vector3(-2f, 1f, 0f);

        CreateEnemy("Fast", spawnPos1);
        CreateEnemy("Tank", spawnPos2);
    }

}
