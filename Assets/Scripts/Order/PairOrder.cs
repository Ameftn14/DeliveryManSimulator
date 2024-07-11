// using UnityEngine;
// using UnityEngine.UI;
// using System;
// using System.Numerics;

// public class PairOrder : MonoBehaviour
// {
//     public int OrderID;
//     public SingleOrder from;
//     public SingleOrder to;

//     private float price;
//     private float distance;

//     private Timespan Deadline;
//     private Timespan AcceptTime;

//     private int Orderstate;

//     public enum State
//     {
//         NotAccept,//未接单
//         Accept,//已接单
//         PickUp,//已取货
//         Delivered,//已送达
//         Finished//已完成        
//     }

//     public State state;

//     public Start()
//     {
//         state = State.NotAccept;
//         distance = Vector2.Distance(from.GetPosition(), to.GetPosition());
//         // TODO: 初始化
//     }

//     public Update()
//     {

//     }

//     //下面是接口

//     public int GetOrderID()
//     {
//         return OrderID;
//     }
//     public void SetOrderID(int id)
//     {
//         this.OrderID = id;
//     }

//     public float GetPrice()
//     {
//         return price;
//     }
//     public void SetPrice(float price)
//     {
//         this.price = price;
//     }

//     public float GetDistance()
//     {
//         return distance;
//     }
//     public void SetDistance(float distance)
//     {
//         this.distance = distance;
//     }

//     public Timespan GetDeadline()
//     {
//         return Deadline;
//     }
//     public void SetDeadline(Timespan deadline)
//     {
//         this.Deadline = deadline;
//     }

//     public Timespan GetAcceptTime()
//     {
//         return AcceptTime;
//     }
//     public void SetAcceptTime(Timespan acceptTime)
//     {
//         this.AcceptTime = acceptTime;
//     }

//     public State GetState()
//     {
//         return state;
//     }
//     //StateChange
//     public void OrederAccept()
//     {
//         state = State.Accept;
//     }
//     public void OrderPickUp()
//     {
//         state = State.PickUp;
//     }
//     public void OrderDelivered()
//     {
//         state = State.Delivered;
//     }
//     public void OrderFinished()
//     {
//         state = State.Finished;
//     }
// }