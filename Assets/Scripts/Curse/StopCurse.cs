using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCurse : MonoBehaviour
{
    public GameObject curse;
    public GameObject curseObjects;
    public GameObject stopParticle;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            if(stopParticle != null)
            {
                Instantiate(stopParticle, transform.position, transform.rotation);
            }
            Destroy(curse);
            Destroy(curseObjects);
            Destroy(gameObject);
        }
    }
}
