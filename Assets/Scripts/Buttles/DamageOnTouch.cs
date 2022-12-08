using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    public float damage;

    private float intervalDamage;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.isTrigger == false)
        {
            if(other.TryGetComponent<EnemyHP>(out EnemyHP enemyHp))
            {
                
                 intervalDamage = damage * Random.Range(0.75f, 1.25f);

                enemyHp.GetDamage(intervalDamage, 100);
            }
        }
    }
}
