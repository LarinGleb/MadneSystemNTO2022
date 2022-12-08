using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceOfCurse_Movement : MonoBehaviour
{
    public float minR = 0;
    public float maxR = 25f;

    public Transform player;

    public Transform source;
    
    public Transform stopCenter;
    public Transform stopPoint; 
    
    public StopCurse stopCurse;
    public GameObject curseParticles;

    public CurseDamager curseDamager;

    private Transform myTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;

        stopCurse.curse = curseParticles;
        
        curseDamager = GameObject.Find("DamagerCurse").GetComponent<CurseDamager>();

        curseDamager.player = player;
        curseDamager.sourceCurse = source;

        source.gameObject.GetComponent<SourceOfCurse>().player = player;

        /*source.position = player.position;
        myTransform.rotation = Quaternion.Euler(0, 180, 0);*/


        source.localPosition = new Vector3(Random.Range(minR, maxR), 0, 0);
        myTransform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        
        stopPoint.localPosition = new Vector3(source.localPosition.x, source.localPosition.y, 0);
        stopPoint.transform.SetParent(null);

        //stopCenter.rotation = Quaternion.Euler(0, myTransform.rotation.eulerAngles.y + 90, 0);


        
    }

    public void ChangePosition()
    {
        source.position = player.position;
        myTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
        
        //stopCenter.localPosition = new Vector3(source.localPosition.x, 0, 0);
        
        //stopPoint.localPosition = new Vector3(source.localPosition.x, source.localPosition.y, 0);
        //stopCenter.rotation = Quaternion.Euler(0, myTransform.rotation.eulerAngles.y + 90, 0);
    }
}
