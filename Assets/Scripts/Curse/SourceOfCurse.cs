using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceOfCurse : MonoBehaviour
{
    public Transform player;

    public SourceOfCurse_Movement sourceOfCurse_Movement;

    public float distanceToTp = 2;
    public float timerToTP;

    private Transform myTransform;

    private bool isCanTP;
    private bool isCanTPByTimer = true;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            if(Vector3.Distance(myTransform.position, player.position) <= distanceToTp)
            {
                if(isCanTP == true && isCanTPByTimer)
                {
                    sourceOfCurse_Movement.ChangePosition();
                    isCanTP = false;
                    isCanTPByTimer = false;

                    StartCoroutine(DelayTPCurse());
                }
            }
            else
            {
                isCanTP = true;
            }
        }
    }

    IEnumerator DelayTPCurse()
    {
        yield return new WaitForSeconds(timerToTP);
        
        isCanTPByTimer = true;

        StopCoroutine(DelayTPCurse());
        yield break;
    }
}
