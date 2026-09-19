using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerWalk walkScript;

    private void Start()
    {
        if (walkScript == null)
        {
            walkScript = GetComponent<PlayerWalk>();
        }

        if (walkScript == null)
        {
            Debug.LogError("PlayerWalk script missing! Attach PlayerWalk to the same GameObject.");
        }
    }

    private void Update()
    {
        if (walkScript == null) return;

        walkScript.CheckGround();

        walkScript.GetInput();

        walkScript.SpeedControl();

        if (walkScript.rb != null)
        {
            walkScript.rb.linearDamping = walkScript.grounded ? walkScript.groundDrag : 0f;
        }
    }

    private void FixedUpdate()
    {
        if (walkScript == null) return;

        walkScript.MovePlayer();

        walkScript.RotatePlayer();
    }
}