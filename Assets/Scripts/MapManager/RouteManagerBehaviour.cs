using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManagerBehaviour : MonoBehaviour {
    public LineRenderer lineRenderer;
    public int targetwaypoint = -1;
    public int beginStartVid = -1, beginEndVid = -1;
    public Vector3 pos;
    public SearchRoad searchRoad;

    public void setRouteBegin(int startVid, int endVid, Vector3 position) {
        beginStartVid = startVid;
        beginEndVid = endVid;
        pos = position;
    }

    public void setRouteEnd(int waypoint) {
        if (targetwaypoint == -1)
            targetwaypoint = waypoint;
    }


    public void showPath(List<Vector3> nodes, Color color) {
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.positionCount = nodes.Count;
        for (int i = 0; i < nodes.Count; i++) {
            lineRenderer.SetPosition(i, nodes[i]);
            // lineRenderer.SetPosition(nodes.Count * 2 - 1 - i, nodes[i]);
        }
    }

    public void hidePath() {
        targetwaypoint = -1;
    }

    void eventHandler(OrderInfo orderinfo) {
        //...
    }



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
        if (searchRoad == null) {
            searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
            Debug.Assert(searchRoad != null);
        }
        // OrderMenuListBehaviour.Instance.OnMouseHoverOrderChanged += eventHandler;
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
