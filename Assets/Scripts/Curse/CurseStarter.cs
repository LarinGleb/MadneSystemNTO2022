using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseStarter : MonoBehaviour
{
    public GameObject particleStart;
    public GameObject curse;
    public float delayCurseParticle;
    
    public GameObject curseSource;

    private bool isActive;
    // Start is called before the first frame update
    void Start()
    {
        delayCurseParticle = particleStart.GetComponent<ParticleSystem>().main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player") && isActive == false)
        {
            isActive = true;
            Instantiate(particleStart, new Vector3(transform.position.x, transform.position.y + 15f, transform.position.z), Quaternion.Euler(90, 0, 0));
            StartCoroutine(DelaySpawnCurse(other.transform));
        }    
    }

    IEnumerator DelaySpawnCurse(Transform player)
    {
        yield return new WaitForSeconds(delayCurseParticle - 1);
        GameObject particleCurse = Instantiate(curse, new Vector3(transform.position.x, transform.position.y + 7.5f, transform.position.z), Quaternion.Euler(0, 0, 0));

        

        GameObject instCurse = Instantiate(curseSource, transform.position, transform.rotation);
        instCurse.GetComponent<SourceOfCurse_Movement>().player = player;
        instCurse.GetComponent<SourceOfCurse_Movement>().curseParticles = particleCurse;


        Destroy(gameObject);
        StopCoroutine(DelaySpawnCurse(null));
        yield break;
    }
}
