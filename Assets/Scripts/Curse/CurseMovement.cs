using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseMovement : MonoBehaviour
{
    public float maxSpeed;
    public float minSpeed;


    public float maxR;
    public float minR;
    public Transform source;
    public float targetR;

    private float curSpeed;
    private Transform myTransform;

    // Start is called before the first frame update
    void Start()
    {
        curSpeed = Random.Range(minSpeed, maxSpeed);
        myTransform = transform;

        StartCoroutine(DelayMovement());
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Rotate(0, curSpeed * Time.deltaTime, 0);

    }

    public void ChangeMovement()
    {
        curSpeed = Random.Range(minSpeed, maxSpeed);
        if(Random.Range(0, 101) > 50)
        {
            curSpeed *= -1f;
        }

        source.localPosition = Vector3.Lerp(source.localPosition, new Vector3(targetR, source.localPosition.y, source.localPosition.z), curSpeed * Time.deltaTime);

        StartCoroutine(DelayMovement());
    }

    IEnumerator DelayMovement()
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        
        targetR = Random.Range(minR, maxR);

        ChangeMovement();

        StopCoroutine(DelayMovement());
        yield break;
    }
}
