using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private Animator anim;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public float minAttackDistance = 1.5f;
    public float maxAttackDistance = 2.5f;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    //Enemy explosion and sound
    public AudioSource bearDeathSFX;

    public bool walk;
    public bool run;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //winState = FindObjectOfType<Level01Controller>();
        //damagePlayer = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

    }

    public void UpdateAnimations()
    {
        anim.SetBool("Walk", walk);
        anim.SetBool("Run", run);
    }

    private void Patroling()
    {
        walk = true;
        run = false;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            
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

        if (Physics.Raycast(walkPoint, -transform.up, 2.5f, whatIsGround))
        {
            walkPointSet = true;
        }

    }

    private void ChasePlayer()
    {

        agent.SetDestination(player.position);
        walk = false;

        run = true;


    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        anim.SetTrigger("Attack");
        if (!alreadyAttacked)
        {
            //Attack code here
            if (Vector3.Distance(player.transform.position, transform.position) > maxAttackDistance)
            {
                alreadyAttacked = false;
                return;
            }

            player.GetComponent<PlayerStats>().health -= 10f;

            //damagePlayer.DamagePlayer(10);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
