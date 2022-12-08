using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowToPlayer : MonoBehaviour
{
    public Transform target;
    public float followSpeed;
    
    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, target.position, followSpeed * Time.deltaTime);
    }
}
