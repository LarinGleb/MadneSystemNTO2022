using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;

    [Header("VisualRotations")]
    public float speedRotate;
    public Transform cameraRotation;
    Transform myTransform;
    [Header("Animations")]
    public Animator animator;

    [Header("Bools")]
    public bool isRoll;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();



    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();

        myTransform = transform;

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 );
        
        float targetMovingSpeed = 0;
        
        // Get targetMovingSpeed.
        if(IsRunning)
        {
            targetMovingSpeed = runSpeed;
            animator.SetInteger("State", 2);
        }
        else if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 )
        {   
             targetMovingSpeed = speed;
             animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 0);
        }

        if(Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0 )
        {
            targetMovingSpeed /= 1.4f;  //<-----------------------------------------------------при порте на мобилку замени 1.4F на поиск гипотенузы через пифагора
        }

        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity =new Vector2( Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        
         
        // Apply movement.
        //rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
        if(isRoll == false)
        {
            rigidbody.velocity = cameraRotation.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y) + cameraRotation.forward * Input.GetAxis("Vertical");
        }
       
    }

    private void LateUpdate() 
    {
        if(isRoll == false)
        {
            Vector3 moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal");
            moveVector.z = Input.GetAxis("Vertical");

            Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * moveVector + cameraRotation.forward * Input.GetAxis("Vertical"), speedRotate * Time.deltaTime, 0f);
            //Vector3 direct = Vector3.RotateTowards(myTransform.forward, cameraRotation.rotation * moveVector + cameraRotation.forward * Input.GetAxis("Vertical"), speedRotate, 0f);
            transform.rotation = Quaternion.LookRotation(direct);
        }
    }
}