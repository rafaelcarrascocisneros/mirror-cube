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

    private int layerMask = 1 << 8; // this layerMask is for the faces of the cube only
    CubeState cubeState;
    CubeMap cubeMap;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        List<GameObject> facesHit = new List<GameObject>();
        Vector3 ray = tFront.transform.position;
        RaycastHit hit;

        // Does the ray intersect any objects in the layerMask?
        if (Physics.Raycast(ray, tFront.forward, out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(ray, tFront.forward * hit.distance, Color.yellow);
            facesHit.Add(hit.collider.gameObject);
            print(hit.collider.gameObject.name);
        }
        else
        {
            Debug.DrawRay(ray, tFront.forward * 1000, Color.green);
        }

        cubeState.front = facesHit;
        cubeMap.Set();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
