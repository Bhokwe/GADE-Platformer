using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{
    [Header("Core Enemy Stats")]
    public float moveSpeed;
    public Vector3 enemySize = Vector3.one;
    public Material enemyMaterial;

    [Header("AI & Pathfinding")]
    protected NavMeshAgent agent;

    private Vector3 baseScale;
    private float baseRadius;
    private float baseHeight;
    private float baseOffset;

    public virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        baseScale = transform.localScale;

        if (agent != null)
        {
            baseRadius = agent.radius;
            baseHeight = agent.height;
            baseOffset = agent.baseOffset;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    public virtual void SetupEnemy(float speed, Vector3 sizeMultiplier, Material mat)
    {
        moveSpeed = speed;
        enemySize = sizeMultiplier;
        enemyMaterial = mat;

        ApplyVisualScale(sizeMultiplier);
        ApplyNavMeshSettings();
        ApplyMaterial();

        EnemyPatrol patrol = GetComponent<EnemyPatrol>();
        if (patrol != null)
        {
            patrol.InitializePatrol();
        }
    }

    protected void ApplyVisualScale(Vector3 sizeMultiplier)
    {
        transform.localScale = Vector3.Scale(baseScale, sizeMultiplier);
    }

    protected void ApplyNavMeshSettings()
    {
        if (agent == null)
        {
            return;
        }

        float scaleFactor = Mathf.Max(enemySize.x, 0.1f);

        agent.enabled = false;
        agent.speed = moveSpeed;
        agent.radius = baseRadius * scaleFactor;
        agent.height = baseHeight * scaleFactor;
        agent.baseOffset = baseOffset * scaleFactor;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.enabled = true;

        SnapToNavMesh();
    }

    protected void SnapToNavMesh()
    {
        if (agent == null)
        {
            return;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }
        else
        {
            Debug.LogWarning("Enemy could not find NavMesh near spawn: " + gameObject.name);
        }
    }

    private void ApplyMaterial()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null && enemyMaterial != null)
        {
            rend.material = enemyMaterial;
        }
    }

    public virtual void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Enemy hit player: " + gameObject.name);

        if (GameManager.instance != null)
        {
            GameManager.instance.RespawnPlayer();
        }
    }

    // Kept as a fallback if a level still uses non-trigger colliders.
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            return;
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.RespawnPlayer();
        }
    }
}
