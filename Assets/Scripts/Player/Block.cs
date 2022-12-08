using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public KeyCode blockKey;
    public ThirdPersonMovement thirdPersonMovement;
    public Animator animator;

    public float myDefense = 3;

    public PlayerHP playerHP;
    // Start is called before the first frame update
    void Start()
    {
        playerHP.shieldDefense = myDefense;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(blockKey))
        {
            thirdPersonMovement.canRun = false;
            animator.SetBool("isBlock", true);
            playerHP.isBlock = true;
        }
        else
        {
            thirdPersonMovement.canRun = true;
            animator.SetBool("isBlock", false);
            playerHP.isBlock = false;
        }
    }
}
