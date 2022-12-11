using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;

    public float maxUpdateDistance = 30;

    public float speed = 3f;
    public float walkSpeed = 1.5f;

    public float angularSpeed = 60f;
    public float angularWalkSpeed = 90f;

    public Transform target;    

    public float distanceToWalk;
    public float distanceToStop;

    public bool isGetHit = false;
    public bool isAttack = false;
    private Transform myTransform;

    [Header("Attack")]
    public Collider damager;
    public float coolDown = 2f;
    public float timeToNextAnim = 0.7f;
    
    public float maxDelayAttack;
    public float minDelayAttack;

    private bool canAttack = true;



    [Header("Visual")]
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > maxUpdateDistance) return;
        if(isGetHit == false)
        {
            CheckAttackState();
            //CheckHitState();

            navMeshAgent.destination = target.position;
            if(Vector3.Distance(target.position, transform.position) > distanceToWalk)
            {
                navMeshAgent.speed = speed;
                navMeshAgent.angularSpeed = angularSpeed;
                animator.SetInteger("State", 2);
            }
            else if(Vector3.Distance(target.position, transform.position) > distanceToStop)
            {
                navMeshAgent.speed = walkSpeed;
                navMeshAgent.angularSpeed = angularWalkSpeed;
                animator.SetInteger("State", 1);
            }
            else
            {
                navMeshAgent.speed = 0f;
                animator.SetInteger("State", 0);
                if(canAttack == true)
                {
                    Attack();
                    isAttack = true;
                    canAttack = false;
                    StartCoroutine(DelayAttacks(Random.Range(minDelayAttack, maxDelayAttack)));
                }
            }
        }
        else
        {
            navMeshAgent.speed = 0f;
            CheckHitState();
        }

        if(animator.GetBool("hit1") || animator.GetBool("hit2") || animator.GetBool("hit3"))
        {
            isGetHit = false;
            damager.enabled = true;  

            animator.SetBool("getHit1", false);
            animator.SetBool("getHit2", false);
            animator.SetBool("getHit3", false);
        }
    }

    public void CheckAttackState()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);
            damager.enabled = false;    
            isAttack = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false); 
            damager.enabled = false;
            isAttack = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
            damager.enabled = false;
            isAttack = false;
        }

        
    }

    public void CheckHitState()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_1"))
        {
            animator.SetBool("getHit1", false);    
            isGetHit = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_2"))
        {
            animator.SetBool("getHit2", false); 
            isGetHit = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_3"))
        {
            animator.SetBool("getHit3", false);
            isGetHit = false;
        }

        /*if(!animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_1"))
        {
            animator.SetBool("getHit1", false);    
            isGetHit = false;
        }
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_2"))
        {
            animator.SetBool("getHit2", false); 
            isGetHit = false;
        }
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_3"))
        {
            animator.SetBool("getHit3", false);
            isGetHit = false;
        }*/
    }
    public void Attack()
    {
        animator.SetBool("hit" + Random.Range(1, 4), true);
        
        damager.enabled = false;
        damager.enabled = true;
    }



    IEnumerator DelayAttacks(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        canAttack = true;
        StopCoroutine(DelayAttacks(0));
        yield break;
    }

    public void GetDamage()
    {
        if(isAttack == false && isGetHit == false)
        {
            isGetHit = true;
            animator.SetInteger("State", 0);
            animator.SetBool("getHit" + Random.Range(1, 4), true);
        }
    }
}

