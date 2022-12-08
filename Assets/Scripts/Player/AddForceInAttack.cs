using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceInAttack : MonoBehaviour
{
    public Rigidbody rigidbody;

    private void Start() 
    {
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
    }

    public void AddForce(float force)
    {
        if(TryGetComponent<ThirdPersonMovement>(out ThirdPersonMovement thirdPersonMovement))
        {
            thirdPersonMovement.enabled = false;
        }
        rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        //GetComponent<ThirdPersonMovement>().enabled = true;
    }
}
