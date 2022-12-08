using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetGetDamage : MonoBehaviour
{
    public bool isAttack;
    public bool isGetHit;

    public Animator animator;

    [Header("ScriptsToOff")]
    public FighterScript fighterScript;
    public ThirdPersonMovement thirdPersonMovement;
    public Roll roll;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGetHit == true)
        {
            
        }
        CheckHitState();
        if(animator.GetBool("hit1") || animator.GetBool("hit2") || animator.GetBool("hit3"))
        {
            isGetHit = false;

            animator.SetBool("getHit1", false);
            animator.SetBool("getHit2", false);
            animator.SetBool("getHit3", false);
            

            if( thirdPersonMovement.enabled == false)
            {
                thirdPersonMovement.enabled = true;
            }
            fighterScript.enabled = true;
            roll.enabled = true;
        }
    }

    public void GetDamage()
    {
        if(isAttack == false && isGetHit == false)
        {
            isGetHit = true;
            animator.SetInteger("State", 0);
            animator.SetBool("getHit" + Random.Range(1, 4), true);

            thirdPersonMovement.enabled = false;
            fighterScript.enabled = false;
            roll.enabled = false;
        }
    }

    public void CheckHitState()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_1"))
        {
            animator.SetBool("getHit1", false);    
            isGetHit = false;

            if( thirdPersonMovement.enabled == false)
            {
                thirdPersonMovement.enabled = true;
            }
            fighterScript.enabled = true;
            roll.enabled = true;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_2"))
        {
            animator.SetBool("getHit2", false); 
            isGetHit = false;

            if( thirdPersonMovement.enabled == false)
            {
                thirdPersonMovement.enabled = true;
            }
            fighterScript.enabled = true;
            roll.enabled = true;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f && animator.GetCurrentAnimatorStateInfo(0).IsName("getHit_3"))
        {
            animator.SetBool("getHit3", false);
            isGetHit = false;
            if( thirdPersonMovement.enabled == false)
            {
                thirdPersonMovement.enabled = true;
            }
            fighterScript.enabled = true;
            roll.enabled = true;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && animator.GetCurrentAnimatorStateInfo(0).IsName("GetHit_Block"))
        {
            animator.SetBool("getHit1", false);
            animator.SetBool("getHit2", false);
            animator.SetBool("getHit3", false);

            isGetHit = false;
            
            if( thirdPersonMovement.enabled == false)
            {
                thirdPersonMovement.enabled = true;
            }
            fighterScript.enabled = true;
            roll.enabled = true;
        }
    }

    
}
