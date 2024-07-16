using UnityEngine;
using System.Collections;
using System;

public class SingleOrder : MonoBehaviour {
    public ColorDictionary colorDictionary;
    public Vector3 originalScale;
    public VirtualClockUI virtualClockUI;
    public GeneralManagerBehaviour generalManager;
    public MapManagerBehaviour mapManager;
    public RingProgress ringProgress;
    public GameObject[] TheStar;
    public PairOrder parentPairOrder;
    public SingleOrder brotherSingleOrder;
    public TimeSpan acceptTime;
    public TimeSpan TimeToDeadline;
    public bool visible;
    public int OrderID;
    public int ColorIndex;
    // private bool isLate;
    public int level;
    private bool isBig;
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
        TheStar = new GameObject[3];

        originalScale = transform.localScale;

        if (mapManager == null) {
            Debug.LogError("MapManager is not assigned!");
            return;
        }
        state = PairOrder.State.NotAccept;

        ringProgress.ddl = Deadline;
        acceptTime = new TimeSpan(0, 0, 0);

        visible = true;
        isBig = false;
        ringProgress.state = PairOrder.State.NotAccept;
        ringProgress.isFrom = isFrom;

        AddStar();
        
        //改颜色
        foreach (Transform child in transform) {
            // 获取子对象的 SpriteRenderer 组件
            SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();

            // 如果子对象有 SpriteRenderer 组件，则设置其颜色
            if (spriteRenderer != null) {
                Color TargetColor = ColorDictionary.PeekColor(ColorIndex);
                spriteRenderer.color = TargetColor;
            } else {
                Debug.LogError("SpriteRenderer is not assigned!");
            }
        }
    }

    public void Update() {
        if (state > PairOrder.State.NotAccept) {
            //隐藏所有star
            foreach (GameObject star in TheStar) {
                if (star != null) {
                    star.SetActive(false);
                }
            }
        }
    }

    public void OnMouseDown() {
        if (state == PairOrder.State.NotAccept) {
            state = PairOrder.State.Accept;
            Debug.Log("Order " + OrderID + " is accepted");
            
            parentPairOrder.OrderAccept();
            if(ringProgress == null){
                ringProgress = transform.Find("Ring").GetComponent<RingProgress>();
            }
            ringProgress.state = PairOrder.State.Accept;
            SetAcceptTime(virtualClockUI.GetTime());
            brotherSingleOrder.SetAcceptTime(virtualClockUI.GetTime());
            generalManager.DBConfirmOrder(OrderID);
        }
        //StartCoroutine(SizeUpAndDown());
    }

    public void OnMouseEnter(){
        if(parentPairOrder == null){
            parentPairOrder = transform.parent.GetComponent<PairOrder>();
        }
        parentPairOrder.MouseEnter();
    }

    public void OnMouseExit(){
        if(parentPairOrder == null){
            parentPairOrder = transform.parent.GetComponent<PairOrder>();
        }
        parentPairOrder.MouseExit();
    }
    // pid operation
    public int Getpid() {
        return pid;
    }
    public void SetPid(int pid) {
        this.pid = pid;
        //Debug.Log("pid is set to " + pid);
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
    public void OrderNotAccept() {
        state = PairOrder.State.NotAccept;
        ringProgress.state = PairOrder.State.NotAccept;
    }
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

    // public void OrderLated() {
    //     isLate = true;
    // }

    public void OrderFinished() {
        state = PairOrder.State.Finished;
        ringProgress.state = PairOrder.State.Finished;
    }

    public IEnumerator SizeUp() {
        float duration = 0.1f; // 增大和消失的时间
        float elapsed = 0f;
        while(isBig){
            yield return null;
        }
        Vector3 targetScale = originalScale * 1.3f;
        if(isFrom){
            targetScale = originalScale * 1.7f;
        }

        while (elapsed < duration)
        {
            if(transform.localScale == targetScale){
                break;
            }
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isBig = true;
    }

    public IEnumerator SizeDown() {
        Vector3 targetScale = originalScale / 2f;

        float duration = 0.3f; // 增大和消失的时间
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if(transform.localScale == targetScale){
                break;
            }
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator SizeUpAndDown() {
        Vector3 targetScale = originalScale * 2f;

        float duration = 0.3f; // 增大和消失的时间
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        duration = 0.3f; // 增大和消失的时间
        elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator BackToOriginalSize() {
        while(!isBig){
            yield return null;
        }
        Vector3 currentScale = transform.localScale;
        float duration = 0.1f; // 增大和消失的时间
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(currentScale, originalScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isBig = false;
    }

    public void SetTimeToDeadline(TimeSpan time) {
        if(ringProgress == null){
            ringProgress = transform.Find("Ring").GetComponent<RingProgress>();
        }
        TimeToDeadline = time;
        ringProgress.TimeToDeadline = time;
    }

    private void AddStar(){
        if(isFrom){
            if(level == 1){
                TheStar[0] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[0].transform.parent = transform;
                TheStar[0].transform.localPosition = new Vector3(-0.8f, 7.6f, -1);
                TheStar[0].transform.localScale = new Vector3(6f, 6f, 1f);
            }
            else if(level == 2){
                //-5.6 5.1   3.9 5.1
                TheStar[0] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[0].transform.parent = transform;
                TheStar[0].transform.localPosition = new Vector3(-5.6f, 4.3f, -1);
                TheStar[0].transform.localScale = new Vector3(6f, 6f, 1f);

                TheStar[1] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[1].transform.parent = transform;
                TheStar[1].transform.localPosition = new Vector3(4f, 4.3f, -1);
                TheStar[1].transform.localScale = new Vector3(6f, 6f, 1f);
            }
            else if(level == 3){
                TheStar[0] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[0].transform.parent = transform;
                TheStar[0].transform.localPosition = new Vector3(-0.8f, 7.6f, -1);
                TheStar[0].transform.localScale = new Vector3(6f, 6f, 1f);

                TheStar[1] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[1].transform.parent = transform;
                TheStar[1].transform.localPosition = new Vector3(-5f, -1.2f, -1);
                TheStar[1].transform.localScale = new Vector3(6f, 6f, 1f);

                TheStar[2] = Instantiate(Resources.Load("PreFabs/1star")) as GameObject;
                TheStar[2].transform.parent = transform;
                TheStar[2].transform.localPosition = new Vector3(3.4f, -1.2f, -1);
                TheStar[2].transform.localScale = new Vector3(6f, 6f, 1f);
            }
        }
    }
}