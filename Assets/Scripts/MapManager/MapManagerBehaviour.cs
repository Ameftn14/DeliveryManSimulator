using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManagerBehaviour : MonoBehaviour
{

    private int nextVid = 0;
    private int nextPid = 0;
    private int vNum = 0;
    private int eNum = 0;

    private int pNum = 0;
    private Dictionary<int, Vector3> vertices = new Dictionary<int, Vector3>();
    private Dictionary<int, Dictionary<int, float>> edges = new Dictionary<int, Dictionary<int, float>>();
    private Dictionary<int, WayPointBehaviour> wayPoints = new Dictionary<int, WayPointBehaviour>();

    private Dictionary<int, GameObject> vertexObjects = new Dictionary<int, GameObject>();
    private Dictionary<int, Dictionary<int, GameObject>> edgeObjects = new Dictionary<int, Dictionary<int, GameObject>>();
    private int backgroundId = 1;

    // public MapManagerBehaviour()
    // {
    //     vertices = new Dictionary<int, Vector3>();
    //     vertexObjects = new Dictionary<int, GameObject>();
    //     edges = new Dictionary<int, Dictionary<int, float>>();
    //     edgeObjects = new Dictionary<int, Dictionary<int, GameObject>>();
    //     wayPoints = new Dictionary<int, WayPointBehaviour>();
    // }

    // for Outside

    // 获取顶点数量
    public int GetVNum() => vNum;

    // 获取边数量
    public int GetENum() => eNum;

    // 获取顶点的度
    public int GetEnum(int startVid) => edges[startVid].Count;

    // 获取路标数量
    public int GetPNum() => pNum;

    // 获取顶点集合
    public Dictionary<int, Vector3> GetVertices() => vertices;

    // 获取边集合
    public Dictionary<int, Dictionary<int, float>> GetEdges() => edges;
    // 获取路标集合
    public Dictionary<int, WayPointBehaviour> GetWayPoints() => wayPoints;

    // // 在现存的边上插入一个中间节点，返回其vid
    // public int CreateInternalVertex(int vid1, int vid2, float ratio)
    // {
    //     Debug.Assert(ratio > 0 && ratio < 1);
    //     Debug.Assert(vertices.ContainsKey(vid1) && vertices.ContainsKey(vid2));
    //     Debug.Assert(vid1 != vid2);
    //     Debug.Assert(!edges.ContainsKey(vid1) || !edges[vid1].ContainsKey(vid2));
    //     Vector3 p1 = vertices[vid1];
    //     Vector3 p2 = vertices[vid2];
    //     Vector3 p = p1 + (p2 - p1) * ratio;
    //     GameObject vertexObject = Instantiate(Resources.Load("PreFabs/SubCross")) as GameObject;
    //     int vid = AddVertex(p, vertexObject);
    //     vertexObject.transform.position = p;
    //     RemoveEdge(vid1, vid2);
    //     GameObject edgeObject1 = Instantiate(Resources.Load("PreFabs/Road")) as GameObject;
    //     GameObject edgeObject2 = Instantiate(Resources.Load("PreFabs/Road")) as GameObject;
    //     edgeObject1.GetComponent<RoadBehaviour>().SetVertices(vertexObjects[vid1], vertexObject);
    //     edgeObject2.GetComponent<RoadBehaviour>().SetVertices(vertexObject, vertexObjects[vid2]);
    //     AddEdge(vid1, vid, edgeObject1);
    //     AddEdge(vid, vid2, edgeObject2);
    //     return vid;
    // }

    // // 删除中间节点，返回新建的边
    // public GameObject RemoveInternalVertex(int vid)
    // {
    //     Debug.Assert(vertices.ContainsKey(vid));
    //     Debug.Assert(edges.ContainsKey(vid));
    //     Debug.Assert(edges[vid].Count == 2);
    //     Dictionary<int, float> myEdges = edges[vid];
    //     // get the two keys
    //     int vid1 = -1;
    //     int vid2 = -1;
    //     foreach (var key in myEdges.Keys)
    //         if (vid1 == -1)
    //             vid1 = key;
    //         else
    //             vid2 = key;
    //     Debug.Assert(vertices.ContainsKey(vid1) && vertices.ContainsKey(vid2));
    //     RemoveVertex(vid);
    //     GameObject edgeObject = Instantiate(Resources.Load("PreFabs/Road")) as GameObject;
    //     edgeObject.GetComponent<RoadBehaviour>().SetVertices(vertexObjects[vid1], vertexObjects[vid2]);
    //     AddEdge(vid1, vid2, edgeObject);
    //     return edgeObject;
    // }

    // about vertices


    public int AddVertex(Vector3 p, GameObject vertexObject)
    {
        int vid = nextVid++;
        vertices[vid] = p;
        vertexObjects[vid] = vertexObject;
        vNum++;
        return vid;
    }

    public void RemoveVertex(int vid)
    {
        vertices.Remove(vid);
        vertexObjects[vid].SetActive(false);
        vertexObjects.Remove(vid);
        foreach (var endVid in edges[vid].Keys)
            RemoveEdge(vid, endVid);
        edges.Remove(vid);
        vNum--;
    }

    public int PrintVertices()
    {
        foreach (var vid in vertices.Keys)
        {
            Debug.Log("Vertex: " + vid + " : " + vertices[vid]);
        }
        return vertices.Count;
    }


    // about edges


    public int AddEdge(int startVid, int endVid, GameObject edgeObject)
    {
        Debug.Log("AddEdge: Road: " + startVid + " -> " + endVid);
        if (!edges.ContainsKey(startVid))
        {
            edges[startVid] = new Dictionary<int, float>();
            edgeObjects[startVid] = new Dictionary<int, GameObject>();
        }
        edges[startVid][endVid] = Vector3.Distance(vertices[startVid], vertices[endVid]);
        edgeObjects[startVid][endVid] = edgeObject;
        if (!edges.ContainsKey(endVid))
        {
            edges[endVid] = new Dictionary<int, float>();
            edgeObjects[endVid] = new Dictionary<int, GameObject>();
        }
        edges[endVid][startVid] = Vector3.Distance(vertices[endVid], vertices[startVid]);
        edgeObjects[endVid][startVid] = edgeObject;
        eNum++;
        return edges[startVid].Count;
    }

    public void RemoveEdge(int startVid, int endVid)
    {
        edges[startVid].Remove(endVid);
        edgeObjects[startVid][endVid].SetActive(false);
        edgeObjects[startVid].Remove(endVid);
        edges[endVid].Remove(startVid);
        edgeObjects[endVid][startVid].SetActive(false);
        edgeObjects[endVid].Remove(startVid);
        eNum--;
    }

    public int PrintEdges()
    {
        foreach (var startVid in edges.Keys)
        {
            foreach (var endVid in edges[startVid].Keys)
            {
                Debug.Log("Edge: " + startVid + " -> " + endVid + " : " + edges[startVid][endVid]);
            }
        }
        return edges.Count;
    }

    // about waypoints

    public int AddWayPoint(WayPointBehaviour wayPoint)
    {
        wayPoint.pid = nextPid++;
        Debug.Assert(wayPoint != null);
        Debug.Log("AddWayPoint: " + wayPoint.pid);
        wayPoints[wayPoint.pid] = wayPoint;
        return 0;
    }

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in transform)
        {
            VertexBehaviour vertexBehaviour = child.GetComponent<VertexBehaviour>();
            if (vertexBehaviour != null)
            {
                vertexBehaviour.mapManager = this;
                vertexBehaviour.vid = AddVertex(child.position, child.gameObject);
            }
        }
        foreach (Transform child in transform)
        {
            RoadBehaviour roadBehaviour = child.GetComponent<RoadBehaviour>();
            if (roadBehaviour != null)
            {
                roadBehaviour.mapManager = this;
                Debug.Assert(roadBehaviour.startVertex != null && roadBehaviour.endVertex != null);
                roadBehaviour.SetVertices(roadBehaviour.startVertex, roadBehaviour.endVertex);
                AddEdge(roadBehaviour.startVid, roadBehaviour.endVid, roadBehaviour.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     foreach (var pid in wayPoints.Keys)
        //     {
        //         Debug.Log("WayPoint: " + pid + " : " + wayPoints[pid].startVid + " -> " + wayPoints[pid].endVid);
        //     }
        // }
        if (Input.GetKeyDown(KeyCode.M)) {
            int backgroundCnt = 0;
            foreach (Transform child in transform) {
                string name = child.name;
                if (name.Contains("Background")) {
                    backgroundCnt++;
                    if ("Background" + backgroundId == child.name)
                        child.gameObject.SetActive(true);
                    else
                        child.gameObject.SetActive(false);
                }
            }
            backgroundId = (backgroundId + 1) % (backgroundCnt + 1);
        }
    }
    public void LogtoDebug(int index)
    {
        Debug.Log("MapManagerBehaviour: " + wayPoints[index].pid);
    }
}


