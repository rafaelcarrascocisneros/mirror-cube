using UnityEngine;
using UnityEngine.UI;

public class FreeRotationSystem : MonoBehaviour
{
    public Transform target; // The target object to rotate.
    public float rotationSpeed = 0.2f; // Rotation sensitivity.
    [SerializeField]
    private float momentumDecay = 0.95f; // Controls how quickly the momentum fades.

    private Vector3 virtualPivot; // The calculated centroid of the target.
    private Vector3 rotationVelocity; // The current rotation velocity, used for momentum.
    private Vector3 lastMousePosition;
    private Vector3 originalCubePosition;
    private Quaternion originalCubeRotation;

    // Toggle variables for the button
    public FixedRotationSystem fixedRotationSystem; // Assign this in the Unity editor
    public CubeMap cubeMap; // Assign this in the Unity editor
    public Button cubeMapButton;

    void Start()
    {
        originalCubePosition = target.position;
        originalCubeRotation = target.rotation;
        CalculateVirtualPivot();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button just pressed.
        {
            lastMousePosition = Input.mousePosition;
            rotationVelocity = Vector3.zero; // Reset rotation velocity on new drag start.
        }

        if (Input.GetMouseButton(1)) // Right mouse button held.
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // Calculate the rotation velocity based on current mouse movement.
            rotationVelocity = mouseDelta * rotationSpeed;

            RotateAroundVirtualPivot(rotationVelocity);
        }
        else if (rotationVelocity.magnitude > 0.01f) // Apply momentum when the mouse button is released.
        {
            RotateAroundVirtualPivot(rotationVelocity);
            rotationVelocity *= momentumDecay; // Decay the rotation velocity over time.
        }
        else
        {
            rotationVelocity = Vector3.zero; // Reset rotation velocity when it's near zero.
        }
    }

    void CalculateVirtualPivot()
    {
        Bounds bounds = new Bounds(target.position, Vector3.zero);
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
        virtualPivot = bounds.center;
    }

    void RotateAroundVirtualPivot(Vector3 velocity)
    {
        target.RotateAround(virtualPivot, Vector3.up, -velocity.x);
        target.RotateAround(virtualPivot, Vector3.right, velocity.y);
    }

    public void ToggleFreeRotationSystem()
    {
        bool isActive = this.enabled;

        // Toggle FreeRotationSystem
        this.enabled = !isActive;

        // Toggle FixedRotationSystem
        fixedRotationSystem.enabled = isActive;

        // If FreeRotationSystem is being disabled, turn on CubeMap and its button
        if (!this.enabled)
        {
            cubeMap.ToggleVisibility();
            cubeMapButton.interactable = true;
        }
        // If FreeRotationSystem is being enabled, turn off CubeMap and its button
        else
        {
            if (cubeMap.isVisible)
            {
                cubeMap.ToggleVisibility();
            }
            cubeMapButton.interactable = false;
        }

        // Reset cube to original position only if it's not already there
        if (!this.enabled && (target.position != originalCubePosition || target.rotation != originalCubeRotation))
        {
            target.position = originalCubePosition;
            target.rotation = originalCubeRotation;
        }
    }
}
