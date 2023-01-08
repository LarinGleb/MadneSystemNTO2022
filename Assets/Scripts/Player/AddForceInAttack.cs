using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceInAttack : MonoBehaviour
{
    public Rigidbody rb;

    private void Start() 
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void AddForce(float force)
    {
        if(TryGetComponent<ThirdPersonMovement>(out ThirdPersonMovement thirdPersonMovement))
        {
            thirdPersonMovement.enabled = false;
        }
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
        //GetComponent<ThirdPersonMovement>().enabled = true;
    }
}
