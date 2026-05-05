using UnityEngine;

public class ConcreteEnemyFactory : AbstractEnemyFactory
{
    [Header("Enemy Prefabs")]
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;

    [Header("Enemy looks")]
    public Material fastEnemyMaterial;
    public Material tankEnemyMaterial; 

    public override BaseEnemy CreateEnemy(string enemyType, Vector3 spawnPosition)
    {
        GameObject spawnedEnemy = null;
        BaseEnemy enemyScript = null;

        if (enemyType == "Fast")
        {
            //spawn fast enemy
            spawnedEnemy = Instantiate(fastEnemyPrefab, spawnPosition, Quaternion.identity);

            //get the script and set up the enemy
            enemyScript = spawnedEnemy.GetComponent<FastEnemy>();

            //specifics for the fast enemy type: speed, size, material
            enemyScript.SetupEnemy(6f, new Vector3(0.5f, 0.5f, 0.5f), fastEnemyMaterial);

        }
        else if (enemyType == "Tank")
        {
            spawnedEnemy = Instantiate(tankEnemyPrefab, spawnPosition, Quaternion.identity);
            enemyScript = spawnedEnemy.GetComponent<TankEnemy>();

            //specifics for the tank enemy type: speed, size, material
            enemyScript.SetupEnemy(2f, new Vector3(2f, 2f, 2f), tankEnemyMaterial);
        }
        else
        {
            Debug.LogWarning("Factory can't build this enemy type: " + enemyType);
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
