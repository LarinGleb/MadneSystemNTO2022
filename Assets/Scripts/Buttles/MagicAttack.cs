using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public ParticleSystem attackParticle;
    public Collider coll;
    public Animator animator;
    
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            coll.enabled = false;
            StartCoroutine(TurnOffCollider());
            animator.SetBool("isMagicAttack", true);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
        {
            attackParticle.Play();
            coll.enabled = true;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            coll.enabled = false;
            animator.SetBool("isMagicAttack", false);
        }
    }

    IEnumerator TurnOffCollider()
    {
        yield return new WaitForSeconds(0.5f);
        
        //coll.enabled = false;
        StopCoroutine(TurnOffCollider());
        yield break;
    }
}
