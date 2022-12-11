using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckpoinCheckPlayer : MonoBehaviour
{
    public Image pointCanSave;
    public Transform repawnPoint;

    public KeyCode saveKey = KeyCode.E;

    private PlayerHP playerHP;

    void Awake () {
        pointCanSave = GameObject.FindWithTag("InteractionHint").GetComponent<Image>();
    }

    private void Update() 
    {
        if(pointCanSave.enabled == true && Input.GetKeyDown(saveKey))
        {
            playerHP.respawnPoint = repawnPoint;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHp))
        {
            pointCanSave.enabled = true;
            enabled = true;
            playerHP = playerHp;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHp))
        {
            enabled = false;
            pointCanSave.enabled = false;
            playerHP = playerHp;
        }
    }
}
