using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : MonoBehaviour
{
    public ParticleSystem attackParticle;
    public Collider collider;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            collider.enabled = false;
            StartCoroutine(TurnOffCollider());
            animator.SetBool("isMagicAttack", true);
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
        {
            attackParticle.Play();
            collider.enabled = true;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("MagicAttack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            collider.enabled = false;
            animator.SetBool("isMagicAttack", false);
        }
    }

    IEnumerator TurnOffCollider()
    {
        yield return new WaitForSeconds(0.5f);
        
        //collider.enabled = false;
        StopCoroutine(TurnOffCollider());
        yield break;
    }
}
