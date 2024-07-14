using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.Properties;


public class SearchRoad : MonoBehaviour {

    private MapManagerBehaviour mapManager;
    private Property property;
    private Dictionary<int, Dictionary<int, float>> graph;
    private Dictionary<int, Vector3> vertices;
    private Dictionary<int, WayPointBehaviour> wayPoints;
    public List<int> shortestPath = new List<int>();

    public float moveSpeed;

    private int currentPathIndex = 0; // 当前路径索引
    public bool isMoving = false;    //when true, start to move

    private bool firstBegin = true;

    //private Vector3 targetPosition;   // 目标位置
    private int beginPosition = 0; //每段移动的起始点
    private int targetPosition = 0; //每段移动的目标点

    public int targetOrderID = -1;
    public bool targetIsFrom = false;
    public bool orderFinished = false;

    public int targetwaypoint = -1;

    public RouteManagerBehaviour routeManager;
    private LineRenderer lineRenderer;

    public float realSpeedUp;
    public float realTimeSlow;
    public float speedUpPercentage;
    public float timeSlowPercentage;
    private float decreaseSpeedPerSecond = 5.0f; // 每秒减少速度

    // Start is called before the first frame update
    void Start() {
        if (lineRenderer == null) {
            lineRenderer = gameObject.GetComponent<LineRenderer>();
            Debug.Assert(lineRenderer != null);
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
        }
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
        routeManager = GameObject.Find("RouteManager").GetComponent<RouteManagerBehaviour>();

        // 检查是否找到了正确的GameObject
        if (mapManager == null) {
            Debug.LogError("MapManager not found!");
            Debug.Assert(false);
        } else {
            Debug.Log("mapManager found");
        }
        // 检查是否找到了正确的GameObject
        if (property == null) {
            Debug.LogError("property not found!");
            Debug.Assert(false);
        } else {
            Debug.Log("property found");
        }

        graph = mapManager.GetEdges();
        vertices = mapManager.GetVertices();
        wayPoints = mapManager.GetWayPoints();

        moveSpeed = property.speed;
        realSpeedUp = property.speedUp;
        realTimeSlow = property.timeSlow;
    }

    // Update is called once per frame
    void Update() {

        float realMoveSpeed;
        AudioSource audio = GameObject.Find("Camera").GetComponent<AudioSource>();

        if (Input.GetKey(KeyCode.LeftShift) && realSpeedUp > 0) {
            realMoveSpeed = moveSpeed * 2;
            realSpeedUp = Mathf.Max(0, realSpeedUp - decreaseSpeedPerSecond * Time.deltaTime);
        } else {
            realMoveSpeed = moveSpeed;
        }
        // 控制时间流速
        if (Input.GetKey(KeyCode.LeftControl) && realTimeSlow > 0) {
            if (audio.pitch > 0.747f)
                audio.pitch *= 0.99f;
            else
                audio.pitch = 0.5f;
            Time.timeScale = 0.2f;
            realTimeSlow = Mathf.Max(0, realTimeSlow - 5 * decreaseSpeedPerSecond * Time.deltaTime);
        } else {
            if (audio.pitch < 1)
                audio.pitch *= 1.01f;
            else
                audio.pitch = 1;
            Time.timeScale = 1;
        }

        speedUpPercentage = realSpeedUp / DeliverymanManager.addSpeedUp;
        timeSlowPercentage = realTimeSlow / DeliverymanManager.addTimeSlow;

        int switcher = 0;
        if (wayPoints.ContainsKey(targetwaypoint)) switcher += 4;
        if (orderFinished) switcher += 2;
        if (isMoving) switcher += 1;
        switch (switcher) {
            case 0:
            case 1:
            case 2:
            case 3:// 非法目标：不移动
                orderFinished = false;
                isMoving = false;
                break;
            case 4:// 新目标：重新设计路径
                Debug.Log("New start, redesign the path");

                //更新当前路径索引
                currentPathIndex = 0;
                orderFinished = false;

                //起始地点所在边的起点和终点
                int beginStartVid, beginEndVid;
                if (firstBegin) {
                    //0为每日骑手的出发点
                    beginStartVid = 0;
                    beginEndVid = 0;
                    firstBegin = !firstBegin;
                } else {
                    beginStartVid = beginPosition;
                    beginEndVid = targetPosition;
                }

                Vector3 deliverymanPosition = gameObject.transform.position;

                shortestPath = searchRoad(beginStartVid, beginEndVid, deliverymanPosition, targetwaypoint);
                // 初始化目标位置为第一个路径点的位置
                if (shortestPath.Any())
                    targetPosition = shortestPath[0];

                // 开始移动
                isMoving = true;
                break;
            case 5:// 移动中
                Vector3 targetPos;

                // 如果还有路径点未到达
                if (currentPathIndex < shortestPath.Count) {
                    targetPosition = shortestPath[currentPathIndex];
                    targetPos = vertices[targetPosition];
                } else {
                    // 已经到达路径的最后一个点，现在目标是wayPoint
                    if (beginPosition == wayPoints[targetwaypoint].startVid)
                        targetPosition = wayPoints[targetwaypoint].endVid;
                    else
                        targetPosition = wayPoints[targetwaypoint].startVid;
                    targetPos = wayPoints[targetwaypoint].transform.position;
                }


                // 向目标位置移动
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, realMoveSpeed * Time.deltaTime);

                // 检查是否到达目标位置
                if ((gameObject.transform.position - targetPos).sqrMagnitude <= 0.1f) {
                    if (currentPathIndex < shortestPath.Count) {
                        // 到达当前路径点后更新路径索引和目标位置
                        beginPosition = shortestPath[currentPathIndex];
                        currentPathIndex++;
                    } else {
                        // 到达路径末尾和wayPoint后停止移动
                        Debug.Log("Reach the final wayPoint");
                        //property.increaseFinishedCount();
                        orderFinished = true;
                    }
                    routeManager.playerHidePath();
                } else {
                    // 画出路径
                    routeManager.setRouteBegin(beginPosition, targetPosition, gameObject.transform.position);
                    routeManager.playerSetRouteEnd(targetwaypoint);
                }
                break;
            case 6:// 等待目标设定
            case 7:// 达到目标：停止移动
                isMoving = false;
                break;
            default:
                Debug.Log("Error");
                Debug.Assert(false);
                break;
        }
        if (switcher != 5)
            routeManager.playerHidePath();


        if (Input.GetKeyDown(KeyCode.W)) {
            Debug.Log("In SearchRoad: realMoveSpeed:" + realMoveSpeed + "realSpeedUp:" + realSpeedUp + "realTimeSlow:" + realTimeSlow);
        }
    }

    // public List<int> searchRoad(int beginStartVid, int beginEndVid, Vector3 deliverymanPosition, List<int> targetwaypoints) {
    //     List<int> totalShortestList = new List<int>();
    //     for (int i = 0; i < targetwaypoints.Count; i++) {
    //         targetwaypoint = targetwaypoints[i];
    //         totalShortestList.AddRange(searchRoad(beginStartVid, beginEndVid, deliverymanPosition, targetwaypoint));
    //         beginStartVid = wayPoints[targetwaypoint].startVid;
    //         beginEndVid = wayPoints[targetwaypoint].endVid;
    //         deliverymanPosition = wayPoints[targetwaypoint].transform.position;
    //     }
    //     return totalShortestList;
    // }

    public List<Vector3> searchRoadPos(int beginStartVid, int beginEndVid, Vector3 deliverymanPosition, int targetwaypoint) {
        List<Vector3> totalShortestList = new List<Vector3>();
        if (!wayPoints.ContainsKey(targetwaypoint))
            return totalShortestList;
        totalShortestList.Add(deliverymanPosition);
        List<int> tempShortestPath = searchRoad(beginStartVid, beginEndVid, deliverymanPosition, targetwaypoint);
        for (int j = 0; j < tempShortestPath.Count; j++)
            totalShortestList.Add(vertices[tempShortestPath[j]]);
        totalShortestList.Add(wayPoints[targetwaypoint].transform.position);
        return totalShortestList;
    }

    // public List<Vector3> searchRoadPos(int beginStartVid, int beginEndVid, Vector3 deliverymanPosition, List<int> targetwaypoints) {
    //     List<Vector3> totalShortestList = new List<Vector3>();
    //     totalShortestList.Add(deliverymanPosition);
    //     for (int i = 0; i < targetwaypoints.Count; i++) {
    //         targetwaypoint = targetwaypoints[i];
    //         List<int> tempShortestPath = searchRoad(beginStartVid, beginEndVid, deliverymanPosition, targetwaypoint);
    //         for (int j = 0; j < tempShortestPath.Count; j++)
    //             totalShortestList.Add(vertices[tempShortestPath[j]]);
    //         totalShortestList.Add(wayPoints[targetwaypoint].transform.position);
    //         beginStartVid = wayPoints[targetwaypoint].startVid;
    //         beginEndVid = wayPoints[targetwaypoint].endVid;
    //         deliverymanPosition = wayPoints[targetwaypoint].transform.position;
    //     }
    //     return totalShortestList;
    // }

    public List<int> searchRoad(int beginStartVid, int beginEndVid, Vector3 deliverymanPosition, int targetwaypoint) {
        // 目标地点所在边的起点与终点
        int targetStartVid = wayPoints[targetwaypoint].startVid;
        int targetEndVid = wayPoints[targetwaypoint].endVid;
        // 到startVertex的距离与整条边长度的比例
        float ratio = wayPoints[targetwaypoint].ratio;

        DijkstraAlgorithm algo = new DijkstraAlgorithm(graph);

        List<int> tempShortestPath = new List<int>();

        var (pathss, pathssLength) = algo.ShortestPath(beginStartVid, targetStartVid);
        var (pathse, pathseLength) = algo.ShortestPath(beginStartVid, targetEndVid);
        var (pathes, pathesLength) = algo.ShortestPath(beginEndVid, targetStartVid);
        var (pathee, patheeLength) = algo.ShortestPath(beginEndVid, targetEndVid);

        pathssLength = pathssLength + Vector3.Distance(gameObject.transform.position, vertices[beginStartVid]) + graph[targetStartVid][targetEndVid] * ratio;
        pathseLength = pathseLength + Vector3.Distance(gameObject.transform.position, vertices[beginStartVid]) + graph[targetStartVid][targetEndVid] * (1 - ratio);
        pathesLength = pathesLength + Vector3.Distance(gameObject.transform.position, vertices[beginEndVid]) + graph[targetStartVid][targetEndVid] * ratio;
        patheeLength = patheeLength + Vector3.Distance(gameObject.transform.position, vertices[beginEndVid]) + graph[targetStartVid][targetEndVid] * (1 - ratio);

        float[] myArray = new float[4] { pathssLength, pathseLength, pathesLength, patheeLength };
        int flag = 0;
        for (int i = 1; i < 4; i++) {
            if (myArray[i] < myArray[flag]) {
                flag = i;
            }
        }

        switch (flag) {
            case 0:
                tempShortestPath = pathss;
                break;
            case 1:
                tempShortestPath = pathse;
                break;
            case 2:
                tempShortestPath = pathes;
                break;
            case 3:
                tempShortestPath = pathee;
                break;
        }

        // Debug.Log($"Shortest path is: {string.Join(" -> ", tempShortestPath)}");
        return tempShortestPath;
    }

    //public List<int> designAllPath()


    public class SimplePriorityQueue<T, TPrior> where TPrior : IComparable<TPrior> {
        private List<T> items = new List<T>();
        private List<TPrior> priorities = new List<TPrior>();

        public void Enqueue(T item, TPrior priority) {
            items.Add(item);
            priorities.Add(priority);
        }

        public T Dequeue() {
            int index = 0;
            for (int i = 1; i < priorities.Count; i++) {
                if (priorities[i].CompareTo(priorities[index]) < 0) {
                    index = i;
                }
            }

            T result = items[index];
            items.RemoveAt(index);
            priorities.RemoveAt(index);

            return result;
        }

        public bool IsEmpty() {
            return items.Count == 0;
        }
    }

    public class DijkstraAlgorithm {
        Dictionary<int, Dictionary<int, float>> graph;
        Dictionary<int, float> distances;
        Dictionary<int, int> predecessors;
        Dictionary<int, bool> visited;

        public DijkstraAlgorithm(Dictionary<int, Dictionary<int, float>> graph) {
            this.graph = graph;
            distances = new Dictionary<int, float>();
            predecessors = new Dictionary<int, int>();
            visited = new Dictionary<int, bool>();
        }

        public (List<int>, float) ShortestPath(int start, int end) {
            foreach (var vertex in graph.Keys) {
                distances[vertex] = float.MaxValue;
                visited[vertex] = false;
                predecessors[vertex] = -1;
            }

            distances[start] = 0;

            SimplePriorityQueue<int, float> pq = new SimplePriorityQueue<int, float>();
            pq.Enqueue(start, 0);

            while (!pq.IsEmpty()) {
                int current = pq.Dequeue();

                if (visited[current])
                    continue;
                visited[current] = true;

                foreach (var neighbor in graph[current]) {
                    int neighborVertex = neighbor.Key;
                    float edgeWeight = neighbor.Value;

                    float newDistance = distances[current] + edgeWeight;

                    if (newDistance < distances[neighborVertex]) {
                        distances[neighborVertex] = newDistance;
                        predecessors[neighborVertex] = current;
                        pq.Enqueue(neighborVertex, newDistance);
                    }
                }
            }

            List<int> path = new List<int>();
            int now = end;
            while (now != -1) {
                path.Insert(0, now);
                now = predecessors[now];
            }

            // 返回路径和路径长度的元组
            return (path, distances[end]);
        }
    }
}
