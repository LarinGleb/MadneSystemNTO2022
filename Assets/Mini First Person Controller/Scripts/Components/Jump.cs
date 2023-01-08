﻿using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    [Header("Animations")]
    public Animator animator;
    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        rb = GetComponent<Rigidbody>();

        if(animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void LateUpdate()
    {
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            animator.SetBool("isJump", true);
            rb.AddForce(Vector3.up * 100 * jumpStrength);
            
            Jumped?.Invoke();
            
        }
    }
}
