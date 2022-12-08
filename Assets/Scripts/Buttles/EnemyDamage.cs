using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;

    private float intervalDamage;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.isTrigger == false)
        {
            if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHp))
            {
                
                intervalDamage = damage * Random.Range(0.75f, 1.25f);

                playerHp.GetDamage(intervalDamage, 100);
            }
        }
    }
}
