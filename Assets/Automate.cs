using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    public static List<string> moveList = new List<string>() { "U", "U" };
    private CubeState cubeState;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveList.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            // Do the move at the first index;
            DoMove(moveList[0]);

            // Remove the move at the first index
            moveList.Remove(moveList[0]);
        }
    }

    void DoMove(string move)
    {
        CubeState.autoRotating = true;
        if(move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        // automatically rotate the side by the angle
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);
    }
}
