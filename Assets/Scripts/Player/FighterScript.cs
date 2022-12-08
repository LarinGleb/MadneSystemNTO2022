using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour
{
    public Animator animator;
    public float coolDown = 2f;
    public float timeToNextAnim = 0.7f;
    private float nextFireTime = 0f;
    public static int numberOfClicks = 0;
    float lastClickedTime = 0;
    public float maxComboDelay = 1;

    public ThirdPersonMovement thirdPersonMovement;

    public Collider damager;
    public GameObject runDamager;

    public PlayerSetGetDamage playerSetGetDamage;

    private Transform myTransform;
    private Transform cameraRotation;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        cameraRotation = thirdPersonMovement.cameraRotation;
    }


    // Update is called once per frame
    void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
        {
            animator.SetBool("hit1", false);

            playerSetGetDamage.isAttack = false;
            
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
        {
            animator.SetBool("hit2", false); 

            playerSetGetDamage.isAttack = false;
            
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit3"))
        {
            animator.SetBool("hit3", false);
            damager.enabled = false;
            numberOfClicks = 0;

            playerSetGetDamage.isAttack = false;
        }
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && animator.GetCurrentAnimatorStateInfo(0).IsName("runHit"))
        {
            animator.SetBool("runHit", false); 
            damager.enabled = false;
            numberOfClicks = 0;

            playerSetGetDamage.isAttack = false;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("runHit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
        {
            runDamager.SetActive(false);
            runDamager.SetActive(true);
            damager.enabled = false;
            damager.enabled = true;
        }

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("hit1")  && !animator.GetCurrentAnimatorStateInfo(0).IsName("hit2") && !animator.GetCurrentAnimatorStateInfo(0).IsName("hit3") && !animator.GetCurrentAnimatorStateInfo(0).IsName("runHit"))
        {
            numberOfClicks = 0;
            damager.enabled = false;
            runDamager.SetActive(false);
            thirdPersonMovement.enabled = true;
        }

        if(Time.time - lastClickedTime > maxComboDelay)
        {
            damager.enabled = false;
            numberOfClicks = 0;
        }
        if(Time.time > nextFireTime)
        {
            if(Input.GetMouseButtonDown(0))
            {
                OnClick();
            }
        }
    }
    private void LateUpdate() 
    {
       /* Vector3 moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") + 0.1f;
        moveVector.z = Input.GetAxis("Vertical") + 0.1f;*/
        if(thirdPersonMovement.enabled == false)
        {
            Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * Vector3.forward + cameraRotation.forward * Input.GetAxis("Vertical"), 5 * Time.deltaTime, 0f);
            //Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * moveVector + cameraRotation.forward * Input.GetAxis("Vertical"), speedRotate, 0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
    }

    void OnClick()
    {
        thirdPersonMovement.enabled = false;
        lastClickedTime = Time.time;
        numberOfClicks++;
        if(Input.GetKey(KeyCode.LeftShift) == false)
        {
            if(numberOfClicks == 1)
            {
                animator.SetBool("hit1", true);

                playerSetGetDamage.isAttack = true;
                
                damager.enabled = false;
                damager.enabled = true;
            }
            numberOfClicks = Mathf.Clamp(numberOfClicks, 0, 3);

            if(numberOfClicks >= 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit1"))
            {
                animator.SetBool("hit1", false);
                animator.SetBool("hit2", true);

                playerSetGetDamage.isAttack = true;

                damager.enabled = false;
                damager.enabled = true;
  
            }

            if(numberOfClicks >= 3 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > timeToNextAnim && animator.GetCurrentAnimatorStateInfo(0).IsName("hit2"))
            {
                animator.SetBool("hit2", false);
                animator.SetBool("hit3", true);

                playerSetGetDamage.isAttack = true;

                damager.enabled = false;
                damager.enabled = true;

            }
        }
        else
        {
            animator.SetBool("runHit", true);
            playerSetGetDamage.isAttack = true;

        }
    }
}
