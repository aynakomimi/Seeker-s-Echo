using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;

    [Header("Cinemachine FreeLook Reference")]
    public CinemachineOrbitalFollow freeLookCam;

    [Header("Sensitivity Settings")]
    [Range(0.1f, 10f)] public float mouseSensitivityX = 2f;
    [Range(0.1f, 10f)] public float mouseSensitivityY = 2f;

    [Header("Scroll Wheel Zoom Settings")]
    public float zoomSpeed = 3f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 10f;

    private CinemachineInputAxisController axisController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (freeLookCam == null) freeLookCam = GetComponent<CinemachineOrbitalFollow>();
        axisController = GetComponent<CinemachineInputAxisController>();
    }

    private void Update()
    {
        if (axisController != null && axisController.Controllers.Count >= 2)
        {
            axisController.Controllers[0].Input.Gain = mouseSensitivityX;
            axisController.Controllers[1].Input.Gain = mouseSensitivityY;
        }

        HandleZoom();
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

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollInput) < 0.01f || freeLookCam == null) return;

        float targetRadius = freeLookCam.Radius - (scrollInput * zoomSpeed);
        freeLookCam.Radius = Mathf.Clamp(targetRadius, minZoomDistance, maxZoomDistance);
    }
}