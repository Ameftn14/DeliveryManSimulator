using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBehaviour : MonoBehaviour
{
    public int startVid = -1;
    public int endVid = -1;
    public GameObject startVertex = null;
    public GameObject endVertex = null;
    // Start is called before the first frame update
    public MapManagerBehaviour mapManager = null;
    void Awake()
    {
        if (mapManager == null)
        {
            mapManager = transform.parent.GetComponent<MapManagerBehaviour>();
        }
        Debug.Assert(mapManager != null);
    }

    public void SetVertices(GameObject start, GameObject end)
    {
        startVertex = start;
        endVertex = end;
        startVid = startVertex.GetComponent<VertexBehaviour>().vid;
        endVid = endVertex.GetComponent<VertexBehaviour>().vid;
        Debug.Log("SetVertices Road: " + startVid + " -> " + endVid);
        Vector3 startPosition = startVertex.transform.position;
        Vector3 endPosition = endVertex.transform.position;
        Vector3 direction = endPosition - startPosition;
        Vector3 center = startPosition + direction / 2;
        center.z = transform.position.z;
        transform.position = center;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        transform.localScale = new Vector3(1, direction.magnitude, 1);
        mapManager.AddEdge(startVid, endVid, gameObject);
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "WayPoint")
            {
                child.GetComponent<WayPointBehaviour>().SetVertices(startVertex, endVertex);
                mapManager.AddWayPoint(child.GetComponent<WayPointBehaviour>().pid, child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
