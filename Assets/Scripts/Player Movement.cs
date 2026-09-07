using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 7f;
    public float rotationSpeed = 15f; 

    [Header("References")]
    public Transform orientation;
    public Transform playerObj; 

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDir;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    private void Update()
    {
        myInput();

        if (orientation != null)
        {
            moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
        }

        if (moveDir != Vector3.zero && playerObj != null)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, moveDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        if (moveDir != Vector3.zero && rb != null)
        {
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }
    }
}