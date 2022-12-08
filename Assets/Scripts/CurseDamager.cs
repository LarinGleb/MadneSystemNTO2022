using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseDamager : MonoBehaviour
{
    public float damage = 5f;
    public float stackDamage;

    public bool canDamage = true;

    public float delayDamage = 0.5f;

    
    public Transform sourceCurse;
    public Transform player;

    private int coutStacks = 0;

    private void OnTriggerStay(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHP))
        {
            if(canDamage == true)
            {
                playerHP.GetDamageAsPoison((damage / Vector3.Distance(player.position, sourceCurse.position)) + stackDamage * coutStacks);
                canDamage = false;
                coutStacks++;
                StartCoroutine(DelayDamage());
            }
        }
    }

    IEnumerator DelayDamage()
    {
        yield return new WaitForSeconds(delayDamage);
        
        canDamage = true;

        StopCoroutine(DelayDamage());
        yield break;
    }
}
