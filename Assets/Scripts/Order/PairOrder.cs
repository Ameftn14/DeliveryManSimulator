using UnityEngine;
using UnityEngine.UI;
using System;

public class PairOrder : MonoBehaviour
{
    public int OrderID;
    public GameObject OrderFrom;//预制件
    public GameObject OrderTo;//预制件
    public SingleOrder from;//单个订单:起点
    public SingleOrder to;//单个订单:终点

    private int price;
    private float distance;

    private TimeSpan Deadline;
    //private Timespan AcceptTime;
    //private Timespan PickUpTime;
    private float LifeTime = 5f;

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
        state = State.NotAccept;
        distance = Vector2.Distance(from.GetPosition(), to.GetPosition());
        //获取脚本
        from = OrderFrom.GetComponent<SingleOrder>();
        to = OrderTo.GetComponent<SingleOrder>();
        //随机生成价格
        price = UnityEngine.Random.Range(30, 100);
        // TODO: 初始化
    }

    public void Update()
    {

    }

    public void GeneratePrefab()
    {
        //TODO: 生成预制件
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

    public float GetPrice()
    {
        return price;
    }
    public void SetPrice(int price)
    {
        this.price = price;
    }

    public float GetDistance()
    {
        return distance;
    }
    public void SetDistance(float distance)
    {
        this.distance = distance;
    }

    public TimeSpan GetDeadline()
    {
        return Deadline;
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
    public void OrederAccept()
    {
        state = State.Accept;
    }
    public void OrderPickUp()
    {
        state = State.PickUp;
    }
    public void OrderDelivered()
    {
        state = State.Delivered;
    }
    public void OrderFinished()
    {
        state = State.Finished;
    }
}