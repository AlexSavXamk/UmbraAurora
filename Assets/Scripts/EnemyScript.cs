using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform target;
    public float detectionDistance;
    public float attackDistance;
    public GameObject player;
    public GameObject attackHitbox;
    bool isEnumerating;
    public bool isDead;
    public bool hasDetectedPlayer;

    public bool isRangedEnemy;

    public bool isRangedAttacking;
    public bool attackPlayer;
    Animator anim;
    public AnimationClip attackAnimation;
    Rigidbody rb;

    float waitingTime;

    //public ParticleSystem magicParticle;
    public GameObject enemyParticle;
    public Transform particlePoint;
    UnityEngine.AI.NavMeshAgent agent;

    public bool isPartOfGroups = false;

    //public AudioSource enemyDeath;

    float originalSpeed;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        player = FindObjectOfType<PlayerController>().gameObject;

        isEnumerating = false;
        originalSpeed = agent.speed;

        if(attackHitbox != null)
            attackHitbox.SetActive(false);

        if(target == null)
            target = player.transform;
    }  
    void Update()
    {
        Animations();

        if(isDead)
        {
            attackHitbox.SetActive(false);
            agent.isStopped = true;
            return;
        }
        
        //agent.destination = target.position;

        float playerDistance = Vector3.Distance(player.transform.position, transform.position);

        if(playerDistance <= detectionDistance && !hasDetectedPlayer){
            hasDetectedPlayer = true;
            agent.destination = target.position;
        }

        if(!hasDetectedPlayer){
            agent.destination = agent.transform.position;
        }
            

        if(hasDetectedPlayer)
        {
            target = player.transform;
            agent.destination = target.position;
        }

        if(target == player.transform)
        {
            if(isRangedEnemy)
                return;
            
            //attackDistance = 4;

            if(playerDistance <= attackDistance)
                attackPlayer = true;
            else
                attackPlayer = false;
        }

        if(isRangedEnemy)
        {
            if(!isRangedAttacking)
            {
                StartCoroutine(ShouldDoRanged());
            }
        }
        
    }

    public void Death()
    {
        if(isDead)
            return;
            
        isDead = true;
        
        //enemyDeath.PlayOneShot(enemyDeath.clip);
        gameObject.GetComponent<Collider>().enabled = false;

        //Invoke("DisableEnemy", 4f);
    }

    /*void FixedUpdate()
    {
        rb.velocity = Vector3.zero;
    }*/

    void Animations()
    {
        if(isDead)
        {
            print("HEWWO??");
            anim.SetInteger("AnimState", 2); //die
        }
            

        if ((attackPlayer) && !isDead) 
        {
            if(!isEnumerating)
                StartCoroutine(CurrentlyAttacking());

            anim.SetInteger("AnimState", 1);
            this.transform.LookAt(target);
        	transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            print("yay");
        }else if(!isDead)
        {
            float playerDistance = Vector3.Distance(player.transform.position, transform.position);

            if(hasDetectedPlayer)
                anim.SetInteger("AnimState", 0);  //walk
                else
                anim.SetInteger("AnimState", 5); //idle
        }
    }

    void AttackCollider()
    {
        attackHitbox.SetActive(true);
    }

    IEnumerator ShouldDoRanged()
    {
        print("Hewo");
        isRangedAttacking = true;
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        if(!attackPlayer)
        StartCoroutine(RangedAttacking());

        isRangedAttacking = false;
    }

    IEnumerator CurrentlyAttacking()
    {
        isEnumerating = true;
        //agent.isStopped = true;
        agent.speed = originalSpeed / 2.5f;
        agent.acceleration = 9999;
        Invoke("AttackCollider", 0.2f);
        //yield return new WaitForSeconds(attackAnimation.length  / 1.4f);
        yield return new WaitForSeconds(1.2f);
        agent.acceleration = 20;
        agent.speed = originalSpeed;
        isEnumerating = false;
        attackHitbox.SetActive(false);
        //agent.isStopped = true;
    }

    IEnumerator RangedAttacking()
    {
        isEnumerating = true;
        //agent.isStopped = true;
        anim.SetInteger("AnimState", 1);
        
        Instantiate(enemyParticle, particlePoint.position, particlePoint.rotation);
        //magicParticle.Play();
        yield return new WaitForSeconds(attackAnimation.length / 1.4f);
        isEnumerating = false;
        //magicParticle.Stop();
        //agent.isStopped = false;
    }

    public void TargetPlayer()
    {
        hasDetectedPlayer = true;
        agent.destination = target.position;
    }

    public void DisableEnemy()
    {
        gameObject.SetActive(false);
    }
}
