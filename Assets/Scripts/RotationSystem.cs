using UnityEngine;

public class RotationSystem : MonoBehaviour
{
    public Transform target; // The target object to rotate.
    public float rotationSpeed = 0.2f; // Rotation sensitivity.
    [SerializeField]
    private float momentumDecay = 0.95f; // Controls how quickly the momentum fades.

    private Vector3 virtualPivot; // The calculated centroid of the target.
    private Vector3 rotationVelocity; // The current rotation velocity, used for momentum.
    private Vector3 lastMousePosition;

    void Start()
    {
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
}
