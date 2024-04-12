using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;
    private bool dragging = false;
    private bool autoRotating = false;
    private float sensitivity = 0.4f;
    private float speed = 300f;
    private Vector3 rotation;

    private Quaternion targetQuaternion;

    private ReadCube readCube;
    private CubeState cubeState;
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }
        if (autoRotating)
        {
            AutoRotate();
        }
    }

    private void SpinSide(List<GameObject> side)
    {
        // reset the rotation
        rotation = Vector3.zero;

        // current mouse position minus the last mouse position
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        

        // rotate
        transform.Rotate(rotation, Space.Self);

        // store mouse
        mouseRef = Input.mousePosition;
    }

    public void Rotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;
        dragging = true;
        // Create a vector to rotate around
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }

    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 rotationAxis;

        // Determine the rotation axis based on the side
        if (side == cubeState.up || side == cubeState.down)
        {
            rotationAxis = new Vector3(0, 1, 0); // Y-axis
        }
        else if (side == cubeState.left || side == cubeState.right)
        {
            rotationAxis = new Vector3(0, 0, 1); // Z-axis
        }
        else // if (side == cubeState.front || side == cubeState.back)
        {
            rotationAxis = new Vector3(1, 0, 0); // X-axis
        }

        targetQuaternion = Quaternion.AngleAxis(angle, rotationAxis) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    public void RotateToRightAngle()
    {
        // get the current rotation
        Vector3 currentRotation = transform.localEulerAngles;

        // round the rotation to the nearest 90 degrees
        currentRotation.x = Mathf.Round(currentRotation.x / 90) * 90;
        currentRotation.y = Mathf.Round(currentRotation.y / 90) * 90;
        currentRotation.z = Mathf.Round(currentRotation.z / 90) * 90;

        // set the rotation
        targetQuaternion.eulerAngles = currentRotation;
        autoRotating = true;
    }

    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // if within one degree, set angle to target angle and end the rotation
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            // unparent little cubes
            cubeState.PutDown(activeSide, transform.parent);
            CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;
        }
    }

}
