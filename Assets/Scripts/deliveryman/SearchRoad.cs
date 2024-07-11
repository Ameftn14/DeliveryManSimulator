using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.Properties;


public class searchroad : MonoBehaviour {

    private MapManagerBehaviour mapManager;
    private Property property;
    public List<int> shortestPath = new List<int>();

    public float moveSpeed;

    private int currentPathIndex = 0; // 当前路径索引
    private bool isMoving = false;    //when true, start to move

    private bool firstBegin = true;

    //private Vector3 targetPosition;   // 目标位置
    private int beginPosition; //每段移动的起始点
    private int targetPosition; //每段移动的目标点

    public bool orderFinished = false;


    void Awake() {
        Debug.Log("egg awaked");
    }
    // Start is called before the first frame update
    void Start() {
        Debug.Log("egg started");
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        property = GameObject.Find("deliveryman").GetComponent<Property>();

        // 检查是否找到了正确的GameObject
        if (mapManager == null) {
            Debug.LogError("MapManager not found!");
        } else {
            Debug.Log("mapManager found");
        }
    }

    // Update is called once per frame
    void Update() {
        moveSpeed = property.speed;
        Dictionary<int, Dictionary<int, float>> graph = mapManager.GetEdges();
        Dictionary<int, Vector3> vertices = mapManager.GetVertices();
        Dictionary<int, WayPointBehaviour> wayPoints = mapManager.GetWayPoints();
        DijkstraAlgorithm algo = new DijkstraAlgorithm(graph);

        int targetwaypoint = 8;


        // 检测按键输入
        if (Input.GetKeyDown(KeyCode.S) && !isMoving) {
            Debug.Log("S pressed, new start");


            Debug.Assert(wayPoints.ContainsKey(targetwaypoint));

            //更新当前路径索引
            currentPathIndex = 0;
            orderFinished = false;

            if (wayPoints.ContainsKey(targetwaypoint)) {

                // 目标地点所在边的起点与终点
                int targetStartVid = wayPoints[targetwaypoint].startVid;
                int targetEndVid = wayPoints[targetwaypoint].endVid;
                // 到startVertex的距离与整条边长度的比例
                float ratio = wayPoints[targetwaypoint].ratio;

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
                        shortestPath = pathss;
                        break;
                    case 1:
                        shortestPath = pathse;
                        break;
                    case 2:
                        shortestPath = pathes;
                        break;
                    case 3:
                        shortestPath = pathee;
                        break;
                }

                Debug.Log($"Shortest path is: {string.Join(" -> ", shortestPath)}");
            }

            // 初始化目标位置为第一个路径点的位置
            if (shortestPath.Any())
                targetPosition = shortestPath[0];

            // 开始移动
            if (!isMoving) {
                isMoving = true;
            }
        } else if (Input.GetKeyDown(KeyCode.C) && isMoving) {
            Debug.Log("C pressed");
            // 停止移动
            isMoving = false;
        }


        if (isMoving) {
            Vector3 targetPos;

            // 如果还有路径点未到达
            if (currentPathIndex < shortestPath.Count) {
                targetPosition = shortestPath[currentPathIndex];
                targetPos = vertices[targetPosition];
            } else {
                // 已经到达路径的最后一个点，现在目标是wayPoint
                targetPos = wayPoints[targetwaypoint].transform.position;
            }

            // 向目标位置移动
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPos, moveSpeed * Time.deltaTime);

            // 检查是否到达目标位置
            if ((gameObject.transform.position - targetPos).sqrMagnitude <= 0.1f) {
                if (currentPathIndex < shortestPath.Count) {
                    // 到达当前路径点后更新路径索引和目标位置
                    beginPosition = shortestPath[currentPathIndex];
                    currentPathIndex++;
                } else {
                    // 到达路径末尾和wayPoint后停止移动
                    Debug.Log("Reach the final wayPoint");
                    orderFinished = true;
                    isMoving = false;
                    targetPosition = int.MaxValue;
                }
            }
        }
    }


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
