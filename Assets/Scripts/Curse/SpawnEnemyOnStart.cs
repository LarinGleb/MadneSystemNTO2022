using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyOnStart : MonoBehaviour
{
    public GameObject enemy;

    public int chanceSpawn = 50;
    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0, 101) < chanceSpawn)
        {
            Instantiate(enemy, transform.position, enemy.transform.rotation);
        }
    }


}
