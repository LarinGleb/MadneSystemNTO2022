using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpoinCheckPlayer : MonoBehaviour
{
    public GameObject pointCanSave;
    public Transform repawnPoint;

    public KeyCode saveKey = KeyCode.E;

    private PlayerHP playerHP;
    private void Update() 
    {
        if(pointCanSave.activeSelf == true && Input.GetKeyDown(saveKey))
        {
            playerHP.respawnPoint = repawnPoint;
        }
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHp))
        {
            pointCanSave.SetActive(true);
            playerHP = playerHp;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.TryGetComponent<PlayerHP>(out PlayerHP playerHp))
        {
            pointCanSave.SetActive(false);
            playerHP = playerHp;
        }
    }
}
