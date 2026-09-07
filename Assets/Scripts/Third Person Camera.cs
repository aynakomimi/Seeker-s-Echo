using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate()
    {
        if (Camera.main == null || player == null || orientation == null) return;

        Vector3 camPosition = Camera.main.transform.position;
        Vector3 playerPosition = player.position;

        camPosition.y = playerPosition.y;

        Vector3 viewDir = (playerPosition - camPosition).normalized;

        if (viewDir != Vector3.zero)
        {
            orientation.forward = viewDir;
        }
    }
}