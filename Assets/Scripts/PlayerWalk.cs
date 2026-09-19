using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    // Movement Settings
    public float moveSpeed = 7f;
    public float rotationSpeed = 10f; 
    public float groundDrag = 5f;

    // Object References
    public Transform orientation;
    public Transform playerObj; 
    public Transform groundCheck; 

    // Ground Check Settings
    public LayerMask whatIsGround;  
    public float groundDistance = 0.4f;

    public bool grounded;
    public float horizontalInput;
    public float verticalInput;
    public Vector3 moveDir;
    public Rigidbody rb;

   

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (rb != null)
            {
                rb.freezeRotation = true;
            }

        if (orientation == null) orientation = transform;
        
        if (playerObj == null) playerObj = transform;
    }

    public void CheckGround()
    {
        if (groundCheck != null)
            {
                grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
            }

        PlayerJump jumpScript = GetComponent<PlayerJump>();

        if (jumpScript != null && !jumpScript.isJumping)
            {
                grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround); 
            }
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        verticalInput = Input.GetAxisRaw("Vertical");
    }

    public void MovePlayer()
    {
        if (orientation == null || rb == null) return;

        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    public void RotatePlayer()
    {
        if (moveDir != Vector3.zero && playerObj != null)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, moveDir.normalized, Time.fixedDeltaTime * rotationSpeed);
        }
    }

    public void SpeedControl()
    {
        if (rb == null) return;

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}