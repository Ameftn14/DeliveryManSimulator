using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointBehaviour : MonoBehaviour {
    public int pid = -1;
    public int startVid = -1, endVid = -1;
    public bool isBusy = false;
    public int isResturant = 0;
    public MapManagerBehaviour mapManager = null;

    // 到startVertex的距离与整条边长度的比例
    public float ratio = 0.5f;

    void Awake() {
        if (mapManager == null) {
            mapManager = transform.parent.parent.GetComponent<MapManagerBehaviour>();
        }
        Debug.Assert(mapManager != null);
    }

    // Start is called before the first frame update
    void Start() {
    }

    public void SetVertices(GameObject start, GameObject end) {
        startVid = start.GetComponent<VertexBehaviour>().vid;
        endVid = end.GetComponent<VertexBehaviour>().vid;
        Debug.Assert(ratio >= 0 && ratio <= 1);
        transform.position = start.transform.position * (1 - ratio) + end.transform.position * ratio;
        transform.localScale = new Vector3(2f, 0.01f, 1f);
    }

    // Update is called once per frame
    void Update() {

    }

    public void BecomeBusy() {
        isBusy = true;
    }

    public void BecomeFree() {
        isBusy = false;
    }
}
