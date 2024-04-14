using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tFront;
    public Transform tBack;

    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    private int layerMask = 1 << 8; // this layerMask is for the faces of the cube only
    CubeState cubeState;
    CubeMap cubeMap;
    public GameObject emptyGO;

    // Start is called before the first frame update
    void Start()
    {
        SetRayTransforms();

        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();
        ReadState();
        CubeState.started = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReadState()
    {
        cubeState = FindObjectOfType<CubeState>();
        
        if(cubeMap.gameObject.activeSelf)
        {
            cubeMap = FindObjectOfType<CubeMap>();
        }

        // set the state of each position in the list of sides so we know
        // what color is in what position
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        // check if cubeMap is toggled on
        if(cubeMap.gameObject.activeSelf)
        {
            // update the map with the found positions
            cubeMap.Set();
        }

    }

    void SetRayTransforms()
    {
        // populate the ray lists with raycasts eminating from the transform, angled towards the cube.
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0), 0.55f, 0.5f, new List<int> { 0, 3, 6 }, new List<int> { 6, 7, 8 });
        backRays = BuildRays(tBack, new Vector3(0, 270, 0), -0.55f, 0.5f, new List<int> { 2, 5, 8 }, new List<int> { 6, 7, 8 });
        upRays = BuildRays(tUp, new Vector3(90, 90, 0), 0.55f, 0.5f, new List<int> { 0, 3, 6 }, new List<int> { 6, 7, 8 });
        downRays = BuildRays(tDown, new Vector3(270, 90, 0), 0.55f, -0.5f, new List<int> { 0, 3, 6 }, new List<int> { 0, 1, 2 });
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0), -0.55f, 0.5f, new List<int> { 2, 5, 8 }, new List<int> { 6, 7, 8 });
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0), 0.55f, 0.5f, new List<int> { 0, 3, 6 }, new List<int> { 6, 7, 8 });
    }

    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction, float xOffset, float yOffset, List<int> raysToOffsetx, List<int> raysToOffsety)
    {
        // The ray count is used to name the rays so we can be sure they are in the right order.
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                float offsetX = raysToOffsetx.Contains(rayCount) ? xOffset : 0;
                float offsetY = raysToOffsety.Contains(rayCount) ? yOffset : 0;

                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x + offsetX,
                                            rayTransform.localPosition.y + y + offsetY,
                                            rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            // Does the ray intersect any objects in the layerMask?
            if (Physics.Raycast(ray, rayTransform.forward, out hit, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                // print(hit.collider.gameObject.name);
            }
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }
}   
