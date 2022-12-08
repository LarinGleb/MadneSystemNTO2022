using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTimer : MonoBehaviour
{
    public float damage = 5f;
    public bool canDamage = true;

    public float delayDamage = 0.5f;

    private void OnTriggerStay(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHP))
        {
            if(canDamage == true)
            {
                playerHP.GetDamageAsPoison(damage);
                canDamage = false;
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
