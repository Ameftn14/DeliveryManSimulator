using UnityEngine;
using UnityEngine.UI;
using System;

public class PairOrder : MonoBehaviour {
    public OrderDB orderDB;
    public MapManagerBehaviour mapManager;
    public VirtualClockUI virtualClock;
    public GeneralManagerBehaviour generalManager;
    public int OrderID;
    public SingleOrder fromScript;//单个订单:起点
    public SingleOrder toScript;//单个订单:终点
    public int from_pid;
    public int to_pid;
    private int price;
    private float distance;

    public TimeSpan Deadline;
    //public Timespan AcceptTime;
    //private Timespan PickUpTime;
    private float LifeTime = 5f;//TODO:以下所有liftime都没有提供做修改的接口，待优化
    private float timer = 5f;

    public enum State {
        NotAccept,//未接单
        Accept,//已接单
        PickUp,//已取货
        Lated,//送迟了
        Finished,//已送达      
    }

    public State state;

    public void Start() {
        generalManager = GameObject.Find("GeneralManager").GetComponent<GeneralManagerBehaviour>();
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        virtualClock = GameObject.Find("Time").GetComponent<VirtualClockUI>();

        state = State.NotAccept;


        //TODO:这个逻辑待优化
        Deadline = virtualClock.GetTime().Add(new TimeSpan(2, 0, 0));
        //随机获取两个pid
        do {
            from_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
            Debug.Log("from_pid is"+from_pid);
        } while (mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>().isBusy);
        do {
            to_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
        } while (from_pid == to_pid||mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>().isBusy);

        mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>().BecomeBusy();
        mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>().BecomeBusy();
        //获取两个位置
        Vector2 from_position = mapManager.GetWayPoints()[from_pid].transform.position;
        Vector2 to_position = mapManager.GetWayPoints()[to_pid].transform.position;

        Vector3 from_position3 = new Vector3(from_position.x, from_position.y, -5);
        Vector3 to_position3 = new Vector3(to_position.x, to_position.y, -5);

        //修改两个子对象
        Transform childfrom = transform.Find("OrderFrom");
        //位置
        childfrom.position = from_position3;
        fromScript = childfrom.gameObject.GetComponent<SingleOrder>();
        fromScript.SetIsFrom(true);
        fromScript.SetPid(from_pid);
        fromScript.SetOrderID(OrderID);
        fromScript.LifeTime = LifeTime;
        fromScript.parentPairOrder = this;
        fromScript.Deadline = Deadline;

        Transform childto = transform.Find("OrderTo");
        //位置
        childto.position = to_position3;
        toScript = childto.gameObject.GetComponent<SingleOrder>();
        toScript.SetIsFrom(false);
        toScript.SetPid(to_pid);
        toScript.SetOrderID(OrderID);
        toScript.LifeTime = LifeTime;
        toScript.parentPairOrder = this;
        toScript.Deadline = Deadline;

        toScript.brotherSingleOrder = fromScript;
        fromScript.brotherSingleOrder = toScript;

        distance = Vector2.Distance(fromScript.GetPosition(), toScript.GetPosition());
        //随机生成价格
        price = UnityEngine.Random.Range(30, 100);

        timer = LifeTime;
        state = State.NotAccept;
    }

    public void Update() {
        if (state == State.NotAccept) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                OrderFinished();
                DistroyEverything();
            }
        } 
        else {
            if (state == State.Finished)
            {
                OrderFinished();
                orderDB.RemoveOrder(OrderID);
                DistroyEverything();
            }
            TimeSpan currentTime = virtualClock.GetTime();
            if (currentTime > Deadline) {
                generalManager.LateOrder(OrderID);
                OrderLated();
                state = State.Lated;
                DistroyEverything();
            }
        }
    }

    public void DistroyEverything() {
        Destroy(transform.Find("OrderFrom").gameObject);
        Destroy(transform.Find("OrderTo").gameObject);
        Destroy(gameObject);
        //两个pid变成free
        mapManager.GetWayPoints()[from_pid].GetComponent<WayPointBehaviour>().BecomeFree();
        mapManager.GetWayPoints()[to_pid].GetComponent<WayPointBehaviour>().BecomeFree();
    }

    //下面是接口

    public int GetOrderID() {
        return OrderID;
    }
    public void SetOrderID(int id) {
        this.OrderID = id;
    }

    public int GetPrice() {
        return price;
    }
    //获取订单利润
    public void SetPrice(int price) {
        this.price = price;
    }
    //获取订单距离
    public TimeSpan GetDeadline() {
        return Deadline;
    }
    //获取订单截止时间
    public float GetDistance() {
        return distance;
    }
    public void SetDistance(float distance) {
        this.distance = distance;
    }

    public void SetDeadline(TimeSpan deadline) {
        this.Deadline = deadline;
    }

    // public TimeSpan GetAcceptTime()
    // {
    //     return AcceptTime;
    // }
    // public void SetAcceptTime(TimeSpan acceptTime)
    // {
    //     this.AcceptTime = acceptTime;
    // }

    public State GetState() {
        return state;
    }
    //StateChange
    public void OrderAccept() {
        state = State.Accept;
        //更新子对象状态
        fromScript.OrderAccept();
        toScript.OrderAccept();
        orderDB.UpdateOrder(this);
    }
    public void OrderPickUp() {
        state = State.PickUp;
        //更新子对象状态
        fromScript.OrderPickUp();
        toScript.OrderPickUp();
        orderDB.UpdateOrder(this);
    }
    public void OrderFinished() {
        state = State.Finished;
        //更新子对象状态
        fromScript.OrderFinished();
        toScript.OrderFinished();
        orderDB.UpdateOrder(this);
    }

    public void OrderLated() {
        state = State.Lated;
        //更新子对象状态
        fromScript.OrderLated();
        toScript.OrderLated();
        orderDB.UpdateOrder(this);
    }

    //提供一个接口，调用这个接口时，两个singleoreder对象的大小逐渐变大成原来的两倍
    public void OrderSizeUp() {
        StartCoroutine(fromScript.SizeUp());
        StartCoroutine(toScript.SizeUp());
    }

    public void OrderSizeDown() {
        StartCoroutine(fromScript.SizeDown());
        StartCoroutine(toScript.SizeDown());
    }
}