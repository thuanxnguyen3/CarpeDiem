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
    public float wanderWaitTime = 10f;
    public bool canMoveWhileAttacking;
    [Space]
    public float walkSpeed = 2f;
    public float runSpeed = 3.5f;
    public float wanderRange = 5f;

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

        if(target.GetComponent<PlayerStats>().health <= 0)
        {
            Wander();
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
        if (currentWanderTime >= wanderWaitTime)
        {
            Vector3 wanderPos = transform.position;

            wanderPos.x += Random.Range(-wanderRange, wanderRange);
            wanderPos.z += Random.Range(-wanderRange, wanderRange);

            currentWanderTime = 0;

            agent.speed = walkSpeed;
            agent.SetDestination(wanderPos);

            walk = true;
            run = false;
        }
        else
        {
            if (agent.isStopped)    
            {
                currentWanderTime += Time.deltaTime;

                walk = false;
                run = false;
            }
        }
    }

    public void Chase()
    {
        finishAttack = false;
        agent.SetDestination(target.transform.position);

        bearGrowl.Play();

        walk = false;

        run = true;

        agent.speed = runSpeed;

        if (Vector3.Distance(target.transform.position, transform.position) <= minAttackDistance && !isAttacking)
            StartAttack();
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
