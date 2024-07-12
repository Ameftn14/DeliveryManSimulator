using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SingleOrder : MonoBehaviour {
    public ColorDictionary colorDictionary;
    public VirtualClockUI virtualClockUI;
    public GeneralManagerBehaviour generalManager;
    public MapManagerBehaviour mapManager;

    public TimeSpan acceptTime;
    public bool visible;
    public RingProgress ringProgress;
    //public RingProgress ringProgress; 
    public PairOrder parentPairOrder;
    public SingleOrder brotherSingleOrder;
    public int OrderID;
    public TimeSpan Deadline;
    public PairOrder.State state;
    private int pid;
    private Vector2 position;
    // private float price;
    // private float distance;
    private bool isFrom;//true for from, false for to
    public float LifeTime = 5f;

    public void Start() {
        ringProgress = transform.Find("Ring").GetComponent<RingProgress>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        virtualClockUI = GameObject.Find("Time").GetComponent<VirtualClockUI>();
        parentPairOrder = transform.parent.GetComponent<PairOrder>();
        colorDictionary = new ColorDictionary();
        generalManager = GameObject.Find("GeneralManager").GetComponent<GeneralManagerBehaviour>();

        if (mapManager == null) {
            Debug.LogError("MapManager is not assigned!");
            return;
        }
        Debug.Log("mapManager found");
        state = PairOrder.State.NotAccept;

        //改颜色
        foreach (Transform child in transform) {
            // 获取子对象的 SpriteRenderer 组件
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            // 如果子对象有 SpriteRenderer 组件，则设置其颜色
            if (spriteRenderer != null) {
                Color TargetColor = ColorDictionary.GetColor(OrderID);
                spriteRenderer.color = TargetColor;
            } else {
                Debug.LogError("SpriteRenderer is not assigned!");
            }
        }
        ringProgress.ddl = Deadline;
        acceptTime = new TimeSpan(0, 0, 0);

        visible = true;
        ringProgress.state = PairOrder.State.NotAccept;
        ringProgress.isFrom = isFrom;
    }

    public void Update() {

    }

    public void OnMouseDown() {
        if (state == PairOrder.State.NotAccept) {
            state = PairOrder.State.Accept;
            Debug.Log("Order " + OrderID + " is accepted");
            
            parentPairOrder.OrderAccept();

            ringProgress = transform.Find("Ring").GetComponent<RingProgress>();
            ringProgress.state = PairOrder.State.Accept;
            SetAcceptTime(virtualClockUI.GetTime());
            brotherSingleOrder.SetAcceptTime(virtualClockUI.GetTime());
            generalManager.DBConfirmOrder(OrderID);
        }
    }
    // pid operation
    public int Getpid() {
        return pid;
    }
    public void SetPid(int pid) {
        this.pid = pid;
        Debug.Log("pid is set to " + pid);
        if (mapManager == null) {
            mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        }
        //Debug.Log("mapManager.GetWayPoints().Count is " + mapManager.GetWayPoints().Count);
        position = mapManager.GetWayPoints()[pid].transform.position;
    }

    // isFrom operation
    public bool GetIsFrom() {
        return isFrom;
    }
    public void SetIsFrom(bool isFrom) {
        this.isFrom = isFrom;
    }

    // OrderID operation
    public int GetOrderID() {
        return OrderID;
    }
    public void SetOrderID(int id) {
        this.OrderID = id;
    }

    public void SetAcceptTime(TimeSpan time) {
        acceptTime = time;
        ringProgress.acceptTime = time;
    }

    // position operation
    public Vector2 GetPosition() {
        return position;
    }

    public void SetUnvisible() {
        visible = false;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }

    public void SetVisible() {
        visible = true;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    //statechange
    public void OrderAccept() {
        state = PairOrder.State.Accept;
        ringProgress.state = PairOrder.State.Accept;
    }

    public void OrderPickUp() {
        state = PairOrder.State.PickUp;
        ringProgress.state = PairOrder.State.PickUp;
        if(isFrom){
            SetUnvisible();
        }
    }

    public void OrderLated() {
        state = PairOrder.State.Lated;
        ringProgress.state = PairOrder.State.Lated;
    }

    public void OrderFinished() {
        state = PairOrder.State.Finished;
        ringProgress.state = PairOrder.State.Finished;
    }
}