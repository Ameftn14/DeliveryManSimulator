using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointBehaviour : MonoBehaviour
{
    static private int nextPid = 0;
    public int pid = -1;
    public int startVid = -1, endVid = -1;
    public float ratio = 0;

    // Start is called before the first frame update
    void Start()
    {
        pid = nextPid++;
    }

    public void SetVertices(GameObject start, GameObject end)
    {
        startVid = start.GetComponent<VertexBehaviour>().vid;
        endVid = end.GetComponent<VertexBehaviour>().vid;
        Debug.Assert(ratio > 0 && ratio < 1);
        transform.position = start.transform.position * (1 - ratio) + end.transform.position * ratio;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
