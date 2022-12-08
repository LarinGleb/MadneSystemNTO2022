using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    public Transform myTransform;
    public Rigidbody rb;
    public float force;
    public ThirdPersonMovement thirdPersonMovement;
    public FighterScript fighterScript;
    [SerializeField]
    private bool canRoll = true;

    public Animator animator;
    
    public KeyCode rollKey;
    

    public GroundCheck groundCheck;
    public Transform cameraRotation;

    public PlayerHP playerHp;
    // Start is called before the first frame update
    void Start()
    {   
        myTransform = transform;
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(groundCheck.isGrounded == true)
        {
            if(Input.GetKeyDown(rollKey) && canRoll == true)
            {
                animator.SetBool("isRoll", true);
                fighterScript.enabled = false;
                
                if(playerHp != null)
                {
                    playerHp.isRoll = true;
                }

                Vector3 moveVector = Vector3.zero;
                moveVector.x = Input.GetAxis("Horizontal");
                moveVector.z = Input.GetAxis("Vertical");

                Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * moveVector + cameraRotation.forward * Input.GetAxis("Vertical"), 100f * Time.deltaTime, 0f);
                //Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * moveVector + cameraRotation.forward * Input.GetAxis("Vertical"), speedRotate, 0f);
                transform.rotation = Quaternion.LookRotation(direct);

                thirdPersonMovement.isRoll = true;

                
                rb.velocity = Vector3.zero;
                rb.velocity = myTransform.forward * force;

                StartCoroutine(CoolDownBeforRoll());
            }
        }
    }

    IEnumerator CoolDownBeforRoll()
    {
        canRoll = false;
       
        
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isRoll", false);
        if(playerHp != null)
        {
            playerHp.isRoll = false;
        }
        yield return new WaitForSeconds(1f);
        fighterScript.enabled = true;
        thirdPersonMovement.isRoll = false;
        yield return new WaitForSeconds(0.5f);
        canRoll = true;
        StopCoroutine(CoolDownBeforRoll());
        yield break;
    }
}
