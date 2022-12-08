using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float damage;
    public float chanceCritical;
    public float force = 5;

    private float intervalDamage;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<EnemyHP>(out EnemyHP enemyHp))
        {


            if(Random.Range(0, 100) < chanceCritical)
            {
                intervalDamage = 2 * damage * Random.Range(0.75f, 1.25f);
            }
            else
            {
                intervalDamage = damage * Random.Range(0.75f, 1.25f);
            }
            print(intervalDamage);
            enemyHp.GetDamage(intervalDamage, force);
        }
    }
}
