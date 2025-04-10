using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public Camera targetCamera; // Assign the camera in the Unity editor
    public float zoomSpeed = 30f; // Speed of zooming
    public float minZoom = 20f; // Minimum field of view or orthographic size
    public float maxZoom = 60f; // Maximum field of view or orthographic size

    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        // Get scroll wheel input
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (targetCamera.orthographic)
        {
            // For orthographic cameras
            targetCamera.orthographicSize -= scrollInput * zoomSpeed;
            targetCamera.orthographicSize = Mathf.Clamp(targetCamera.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            // For perspective cameras
            targetCamera.fieldOfView -= scrollInput * zoomSpeed;
            targetCamera.fieldOfView = Mathf.Clamp(targetCamera.fieldOfView, minZoom, maxZoom);
        }
    }
}