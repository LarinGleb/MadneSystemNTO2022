using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHP : MonoBehaviour
{   

    public float maxHp;
    public float curHp;

    public HPBar hpBar;

    public PlayerSetGetDamage playerSetGetDamage;

    public delegate void OnPlayerDeathCallback ();

    public static OnPlayerDeathCallback OnPlayerDeath;

    public bool isBlock;
    public float shieldDefense;
    
    public Transform respawnPoint;

    public bool isRoll;

    void Update () {
        if (transform.position.y < 0) {
            Death ();
        }
    }

    public void GetDamage(float damage, float force)
    {
        if(isRoll == false)
        {
            if(isBlock == true)
            {
                damage /= shieldDefense;
            }


            curHp -= damage;

            hpBar.SetProgress(curHp / maxHp);

            if(playerSetGetDamage != null)
            {
                playerSetGetDamage.GetDamage();
            }
            /*if(enemyAi.isAttack == false)
            {
                enemyAi.GetDamage();
            }*/

            GetComponent<Rigidbody>().AddForce(-transform.forward * force);
        
            //healthBar.fillAmount = curHp / maxHp;
        
            if(curHp <= 0)
            {
                Death();
            }
            if(curHp < maxHp / 2)
            {
                /*if(VFXLowHp != null)
                {
                    VFXLowHp.SetActive(true);
                }*/
            }
        }
    }

    public void GetDamageAsPoison(float damage)
    {
        
            curHp -= damage;

            hpBar.SetProgress (curHp / maxHp);
            
            if(curHp <= 0)
            {
                Death();
            }
    }
    

    public void Death()
    {
        //Destroy(gameObject);
        if (OnPlayerDeath != null) OnPlayerDeath ();
        curHp = maxHp;
        hpBar.SetProgress (1);
        transform.position = respawnPoint.position;
    }
}
