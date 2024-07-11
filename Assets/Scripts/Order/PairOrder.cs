using UnityEngine;
using UnityEngine.UI;
using System;

public class PairOrder : MonoBehaviour
{
    public OrderDB orderDB;
    public MapManagerBehaviour mapManager;
    public VirtualClockUI virtualClock;
    public int OrderID;
    public SingleOrder fromScript;//单个订单:起点
    public SingleOrder toScript;//单个订单:终点
    public int from_pid;
    public int to_pid;
    private int price;
    private float distance;

    private TimeSpan Deadline;
    //private Timespan AcceptTime;
    //private Timespan PickUpTime;
    private float LifeTime = 5f;//TODO:以下所有liftime都没有提供做修改的接口，待优化
    private float timer = 5f;

    private int Orderstate;

    public enum State
    {
        NotAccept,//未接单
        Accept,//已接单
        PickUp,//已取货
        Delivered,//已送达
        Finished//已完成        
    }

    public State state;

    public void Start()
    {
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        mapManager = GameObject.Find("MapManager").GetComponent<MapManagerBehaviour>();
        virtualClock = GameObject.Find("Time").GetComponent<VirtualClockUI>();
        // OrderFromPre = Resources.Load<GameObject>("Prefabs/OrderFrom");
        // if(OrderFromPre == null)
        // {
        //     Debug.Log("OrderFromPre is null");
        // }
        // OrderToPre = Resources.Load<GameObject>("Prefabs/OrderTo");
        state = State.NotAccept;
        //随机获取两个pid
        from_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
        to_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
        while (from_pid == to_pid)
        {
            to_pid = UnityEngine.Random.Range(0, mapManager.GetWayPoints().Count);
        }
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
        fromScript.brotherSingleOrder = toScript;

        Transform childto = transform.Find("OrderTo");
        //位置
        childto.position = to_position3;
        toScript = childto.gameObject.GetComponent<SingleOrder>();
        toScript.SetIsFrom(false);
        toScript.SetPid(to_pid);
        toScript.SetOrderID(OrderID);
        toScript.LifeTime = LifeTime;
        toScript.parentPairOrder = this;
        toScript.brotherSingleOrder = fromScript;

        distance = Vector2.Distance(fromScript.GetPosition(), toScript.GetPosition());
        //随机生成价格
        price = UnityEngine.Random.Range(30, 100);

        //截止时间是当前时间+1h
        //TODO:这个逻辑待优化
        Deadline = virtualClock.GetTime().Add(new TimeSpan(2, 0, 0));
        timer = LifeTime;
        state = State.NotAccept;
    }

    public void Update()
    {
        if(state == State.NotAccept)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                OrderFinished();
                DistroyEverything();
            }
        }
        else{
            TimeSpan currentTime = virtualClock.GetTime();
            if(currentTime > Deadline)
            {
                DistroyEverything();
            }
        }
    }

    public void DistroyEverything()
    {
        Destroy(transform.Find("OrderFrom").gameObject);
        Destroy(transform.Find("OrderTo").gameObject);
        Destroy(gameObject);
    }

    //下面是接口

    public int GetOrderID()
    {
        return OrderID;
    }
    public void SetOrderID(int id)
    {
        this.OrderID = id;
    }

    public int GetPrice()
    {
        return price;
    }
    //获取订单利润
    public void SetPrice(int price)
    {
        this.price = price;
    }
    //获取订单距离
    public TimeSpan GetDeadline()
    {
        return Deadline;
    }
    //获取订单截止时间
    public float GetDistance()
    {
        return distance;
    }
    public void SetDistance(float distance)
    {
        this.distance = distance;
    }

    public void SetDeadline(TimeSpan deadline)
    {
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

    public State GetState()
    {
        return state;
    }
    //StateChange
    public void OrderAccept()
    {
        state = State.Accept;
        //更新子对象状态
        fromScript.state = State.Accept;
        toScript.state = State.Accept;
        orderDB.UpdateOrder(this);
    }
    public void OrderPickUp()
    {
        state = State.PickUp;
        //更新子对象状态
        fromScript.state = State.PickUp;
        toScript.state = State.PickUp;
        orderDB.UpdateOrder(this);
    }
    public void OrderDelivered()
    {
        state = State.Delivered;
        //更新子对象状态
        fromScript.state = State.Delivered;
        toScript.state = State.Delivered;
        orderDB.UpdateOrder(this);
    }
    public void OrderFinished()
    {
        state = State.Finished;
        //更新子对象状态
        fromScript.state = State.Finished;
        toScript.state = State.Finished;
        orderDB.UpdateOrder(this);
    }
}