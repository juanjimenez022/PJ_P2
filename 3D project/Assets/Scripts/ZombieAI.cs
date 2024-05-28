using UnityEngine;
using UnityEngine.AI;
public class ZombieAI : MonoBehaviour
{
    public AudioClip patrulla;
    public AudioClip ataque;
    public AudioClip muere;
    public AudioClip dano;
    public AudioClip persigue;
    public int strong = 25;
    public float maxHealth;
    public float health, timeBetweenAttacks, sightRange, attackRange;

    private float timeAlive;
    private float closestDistanceToPlayer = float.MaxValue;


    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;


    public Animator animator;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    //public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    //public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public void SetGenetic(int strong, float health)
    {
        this.maxHealth = health;
        this.health = health;
        this.strong = strong;
        Debug.Log("Zombie generado con Fuerza: " + this.strong + " y Vida: " + this.health + "(Vida Maxima: "+maxHealth+")");
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetStrong()
    {
        return strong;
    }

    public float GetTimeAlive()
    {
        return timeAlive;
    }

    public float GetClosestDistanceToPlayer()
    {
        return closestDistanceToPlayer;
    }

    private void Awake()
    {
        //Debug.Log("Zombie generado en el awake");
        player = GameObject.Find("FirstPersonController").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        maxHealth = health;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;
        //Check for sight and attack range
        if (health >= 0)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) AttackPlayer();

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer < closestDistanceToPlayer)
            {
                closestDistanceToPlayer = distanceToPlayer;
            }


        }
        else
        {
            animator.SetBool("Looking", false);
            animator.SetBool("Following", false);
            animator.SetBool("Running", false);
            animator.SetBool("Dead", true);
        }
    }

    private void Patroling()
    {
        //reproducirAudio(patrulla);
        animator.SetBool("Looking", true);
        animator.SetBool("Following", false);
        animator.SetBool("Running", false);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            if (agent.isOnNavMesh) // Check if the agent is on the NavMesh
            {
                agent.SetDestination(walkPoint);
            }
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        //reproducirAudio(persigue);
        if (health > 0)
        {
            animator.SetBool("Looking", false);
            animator.SetBool("Following", true);
            animator.SetBool("Running", false);
            agent.SetDestination(player.position);
        }
    }

    private void AttackPlayer()
    {
        reproducirAudio(ataque);
        animator.SetBool("Dead", false);
        animator.SetBool("Looking", false);
        animator.SetBool("Following", false);
        animator.SetBool("Running", false);
        animator.SetTrigger("Attack");
        // Aseg�rate de que el enemigo no se mueva

        if (health > 0 && agent.isOnNavMesh)
        {
            agent.SetDestination(transform.position);
            //transform.LookAt(player);
            LookAtPlayerYAxis();


            if (!alreadyAttacked)
            {
                PlayerController PC = player.GetComponent<PlayerController>();
                if (PC != null)
                {
                    PC.TakeDamage(strong);
                }

                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        


        if (health <= 0)
        {
            //Invoke(nameof(DestroyEnemy), 0.5f);
            DestroyEnemy();
        }
        else
        {
            reproducirAudio(dano);
            animator.SetTrigger("Hurt");
            animator.SetBool("Looking", false);
            animator.SetBool("Following", false);
            animator.SetBool("Running", false);
            health -= damage;
        }

    }
    private void DestroyEnemy()
    {
        reproducirAudio(muere);
        RoundManager.Instance.ZombieMuerto();
        Debug.Log("Destroying Zombie with Health: " + maxHealth + ", Strong: " + strong + ", TimeAlive: " + timeAlive + ", ClosestDistanceToPlayer: " + closestDistanceToPlayer);
        RoundManager.Instance.AddDeadZombieData(new ZombieData(strong, maxHealth, timeAlive, closestDistanceToPlayer));

        Debug.Log("Zombie time alive: " + timeAlive);
        Debug.Log("Closest distance to player: " + closestDistanceToPlayer);

        //Destroy(gameObject);
        animator.SetBool("Dead", true);
        animator.SetBool("Looking", false);
        animator.SetBool("Following", false);
        animator.SetBool("Running", false);
        animator.SetTrigger("Daying");
        if (agent.isActiveAndEnabled)
        {
            agent.enabled = false;
        }

        CapsuleCollider CC = GetComponent<CapsuleCollider>();
        Destroy(CC);
        // Opci�n para destruir el objeto despu�s de un tiempo
        Destroy(gameObject, 5f); // Ajusta el tiempo si es necesario

    }

    private void LookAtPlayerYAxis()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Ignorar los cambios en el eje Y
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void reproducirAudio(AudioClip clip1)
    {
        GameObject tempAudioSource = new GameObject("TempAudio");
        AudioSource audioSource = tempAudioSource.AddComponent<AudioSource>();
        audioSource.clip = clip1;
        audioSource.volume = 0.1f;
        audioSource.Play();

    }
}
