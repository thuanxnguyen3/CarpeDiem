using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;

public class BasicAI : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;
    private Animator anim;
    public LayerMask whatIsGround;

    private bool isWander;
    private bool isAttacking;
    public bool finishAttack;
    public float health = 100f;

    [Header("Attack Settings")]
    public float damage;
    public float maxChaseDistance;
    public float minAttackDistance = 1.5f;
    public float maxAttackDistance = 2.5f;

    [Header("Movement")]
    private float currentWanderTime;
    public float wanderWaitTime = 2f;
    public bool canMoveWhileAttacking;
    [Space]
    public float walkSpeed = 2f;
    public float runSpeed = 3.5f;
    public float wanderRange = 5f;
    public Vector3 walkPoint;
    public ScoreSystem score;


    public bool walk;
    public bool run;

    public float intensity;
    PostProcessVolume volume;
    Vignette vignette;

    public AudioSource bearGrowl;
    public AudioSource bearDead;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        volume = GetComponent<PostProcessVolume>();
        bearGrowl = GetComponent<AudioSource>();
        bearDead = GetComponent<AudioSource>();

        currentWanderTime = wanderWaitTime;
        /*
        volume.profile.TryGetSettings<Vignette>(out vignette);

        if (!vignette)
        {
            print("error, vignette empty");
        } else
        {
            vignette.enabled.Override(false);
        }
        */
    }

    bool hasDied;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(agent);
            anim.SetTrigger("Die");
            bearDead.Play();
            score.scoreVal += 100;



            GetComponent<GatherableObject>().enabled = true;
            Destroy(this);
            return;
        }

        if (target != null)
        {
            if (target.GetComponent<PlayerStats>().health <= 0)
            {
                Wander();
            }
        }


        UpdateAnimations();

        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) > maxChaseDistance)
                target = null;

            if (!isAttacking)
                Chase();
        }
        else
            Wander();
    }

    public void UpdateAnimations()
    {
        anim.SetBool("Walk", walk);
        anim.SetBool("Run", run);
    }

    public void Wander()
    {

        walk = true;
        run = false;
        if (!isWander) SearchWalkPoint();

        if (isWander)
        {
            agent.SetDestination(walkPoint);

        }


        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            isWander = false;

    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-wanderRange, wanderRange);
        float randomX = Random.Range(-wanderRange, wanderRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2.5f, whatIsGround))
        {
            isWander = true;
        }

    }

    public void Chase()
    {
        finishAttack = false;
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            bearGrowl.Play();
        }

        

        walk = false;

        run = true;

        isWander = false;

        agent.speed = runSpeed;
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= minAttackDistance && !isAttacking)
                StartAttack();
        }
        
    }

    public void StartAttack()
    {
        bearGrowl.Play();
        finishAttack = false;
        isAttacking = true;

        if (!canMoveWhileAttacking)
            agent.SetDestination(transform.position);

        anim.SetTrigger("Attack");
    }

    public void FinishAttack()
    {
        if (Vector3.Distance(target.transform.position, transform.position) > maxAttackDistance)
        {
            finishAttack = false;
            return;
        }
        
        target.GetComponent<PlayerStats>().health -= damage;
        finishAttack = true;


        isAttacking = false;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
            target = other.transform;
    }
}
