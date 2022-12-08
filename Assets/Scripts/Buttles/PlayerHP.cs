using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHP : MonoBehaviour
{   

    public float maxHp;
    public float curHp;

    public Image hpBar;

    public PlayerSetGetDamage playerSetGetDamage;


    public bool isBlock;
    public float shieldDefense;
    
    public Transform respawnPoint;

    public bool isRoll;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

            hpBar.fillAmount = curHp / maxHp;

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

            hpBar.fillAmount = curHp / maxHp;
            
            if(curHp <= 0)
            {
                Death();
            }
    }
    

    public void Death()
    {
        //Destroy(gameObject);
        curHp = maxHp;
        transform.position = respawnPoint.position;
    }
}
