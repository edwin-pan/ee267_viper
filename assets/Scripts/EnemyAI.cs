
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public GameObject muzzleFlash;

    // Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Check sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        // State update
        if (!playerInSightRange && !playerInAttackRange) Patrol();
        if (playerInSightRange && !playerInAttackRange) Chase();
        if (playerInSightRange && playerInAttackRange) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) 
            agent.SetDestination(walkPoint);

        Vector3 delta = transform.position - walkPoint;

        // Within threshold?
        if (delta.magnitude < 1f)
            walkPointSet = false;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        // Enemy stops moving to shoot
        agent.SetDestination(transform.position);

        // Rotate to look at player
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Shoot the player
            GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 32f, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().AddForce(transform.up * 8f, ForceMode.Impulse);

            // Muzzle flash
            if (muzzleFlash != null)
                Instantiate(muzzleFlash, transform.position, Quaternion.identity);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    // Utilities
    private void SearchWalkPoint()
    {
        // Find random point in range to walk to 
        float rZ  = Random.Range(-walkPointRange, walkPointRange);
        float rX  = Random.Range(-walkPointRange, walkPointRange);

        // walkPoint = new Vector3(transform.position.x + rX, transform.position.y, transform.position.z + rZ);
        walkPoint = new Vector3(transform.position.x + rX, 1f, transform.position.z + rZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

