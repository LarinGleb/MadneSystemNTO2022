using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [Header("HP")]
    public float maxHp;
    public float curHp;

    private bool isDeath;
    [Header("Visual")]
    public GameObject VFXDeath;
    public GameObject VFXLowHp;

    private Animator animator;

    [Header("ComeBack")]
    public EnemyMovement enemyAi;

    private Vector3 origPosition;

    void Awake () {
        origPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDeath)
        {
            DeathEnemy();
        }
    }

    public void GetDamage(float damage, float force)
    {
        curHp -= damage;

        if(enemyAi.isAttack == false)
        {
            enemyAi.GetDamage();
        }
        GetComponent<Rigidbody>().AddForce(-transform.forward * force);
        //healthBar.fillAmount = curHp / maxHp;
        if(curHp <= 0)
        {
            //DeathEnemy();
            isDeath = true;
            animator.SetBool("isDead", true);
        }
        if(curHp < maxHp / 2)
        {
            if(VFXLowHp != null)
            {
                VFXLowHp.SetActive(true);
            }
        }
    }

    

    public void DeathEnemy()
    {
        //FallAtHit
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("FallAtHit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            gameObject.SetActive(false);
            PlayerHP.OnPlayerDeath += Respawn;
        }
    }

    public void Respawn () {
        transform.position = origPosition;
        curHp = maxHp;
        gameObject.SetActive(true);
        PlayerHP.OnPlayerDeath -= Respawn;
    }
}
