using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Core Enemy Stats")]
    public float moveSpeed;
    public Vector3 enemySize;
    public Material enemyMaterial; //this handles the enemy's texture/colour

    [Header("AI & Pathfinding")]
    protected NavMeshAgent agent; 

    //reference to my customLinkedList
    //protected CustomLinkedList pathfindingList;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    public virtual void SetupEnemy(float speed, Vector3 size, Material mat)
    {
        moveSpeed = speed;
        enemySize = size;
        enemyMaterial = mat;

        // applying the stats to the physical unity object
        if (agent != null) agent.speed = moveSpeed;
        transform.localScale = enemySize;

        Renderer rend = GetComponent<Renderer>();
        if (rend != null) rend.material = enemyMaterial;


    }

    public virtual void Update()
    {
        // Base enemy behavior can be defined here, or left empty for derived classes to implement
    }
    public virtual void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with player, 
            Debug.Log("Enemy collided with player!");

            if(GameManager.instance != null)
            {
                GameManager.instance.RespawnPlayer();
            }
            else
            {
                Debug.LogWarning("GameManager instance not found. Cannot respawn player.");
            }

        }
    }

}
