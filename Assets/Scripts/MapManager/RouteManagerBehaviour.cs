using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManagerBehaviour : MonoBehaviour {
    public LineRenderer lineRenderer;
    public int targetwaypoint = -1;
    public int beginStartVid = -1, beginEndVid = -1;
    public Vector3 pos;
    public SearchRoad searchRoad;

    enum TargetFrom {
        none,
        player,
        list
    }

    private TargetFrom targetFrom = TargetFrom.player;

    public void setRouteBegin(int startVid, int endVid, Vector3 position) {
        beginStartVid = startVid;
        beginEndVid = endVid;
        pos = position;
    }


    public void playerSetRouteEnd(int waypoint) {
        if (targetFrom != TargetFrom.list) {
            targetwaypoint = waypoint;
            targetFrom = TargetFrom.player;
        }
    }

    public void listSetRouteEnd(int waypoint) {
        // targetwaypoint = waypoint;
        // targetFrom = TargetFrom.list;
    }


    public void showPath(List<Vector3> nodes, Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++) {
            Vector3 node = nodes[i];
            node.z = -1.9f;
            lineRenderer.SetPosition(i, node);
            // lineRenderer.SetPosition(nodes.Count * 2 - 1 - i, nodes[i]);
        }
    }

    private void hidePath() {
        targetFrom = TargetFrom.none;
        targetwaypoint = -1;
    }

    public void listHidePath() {
        // if (targetFrom == TargetFrom.list) {
        //     hidePath();
        // }
    }

    public void playerHidePath() {
        if (targetFrom == TargetFrom.player) {
            hidePath();
        }
    }

    void checkMouseList(OrderInfo orderinfo) {
        // if (orderinfo == null) {
        //     if (targetFrom == TargetFrom.list) {
        //         listHidePath();
        //     }
        //     return;
        // }
        // listSetRouteEnd(orderinfo.pid);
    }



    // Start is called before the first frame update
    void Start() {
        if (lineRenderer == null) {
            lineRenderer = GameObject.Find("CurrentRoad").GetComponent<LineRenderer>();
            Debug.Assert(lineRenderer != null);
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
        }
        if (searchRoad == null) {
            searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
            Debug.Assert(searchRoad != null);
        }
        // OrderMenuListBehaviour.Instance.OnMouseHoverOrderChanged += checkMouseList;
    }

    // Update is called once per frame
    void Update() {
        List<Vector3> nodes = searchRoad.searchRoadPos(beginStartVid, beginEndVid, pos, targetwaypoint);
        if (nodes.Count > 0) {
            showPath(nodes, Color.yellow);
        } else {
            hidePath();
        }
    }
}
