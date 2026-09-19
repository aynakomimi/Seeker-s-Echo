using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float groundCheckDelay = 0.15f; 
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Internal State")]
    public bool readyToJump = true;
    public bool isJumping = false; 
    [Header("References")]
    public PlayerWalk walkScript;

    private void Start()
    {
        if (walkScript == null)
        {
            walkScript = GetComponent<PlayerWalk>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(jumpKey) && readyToJump && walkScript.grounded && !isJumping)
        {
            PerformJump();
        }
    }

    public void PerformJump()
    {
        if (walkScript == null || walkScript.rb == null) return;

        readyToJump = false;
        isJumping = true;
        walkScript.grounded = false;

        Vector3 currentVel = walkScript.rb.linearVelocity;
        walkScript.rb.linearVelocity = new Vector3(currentVel.x, 0f, currentVel.z);

        walkScript.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(EnableGroundCheck), groundCheckDelay);
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void EnableGroundCheck()
    {
        isJumping = false;
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}