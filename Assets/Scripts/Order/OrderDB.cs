using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections.Specialized;

public class OrderDB: MonoBehaviour
{
    public static OrderDB Instance { get; set; }

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
    // PairOrder根据OrderID来索引建立字典
    public Dictionary<int, PairOrder> orderDict;

    // 声明委托，通过OrderID用于在订单状态发生变化时通知其他对象
    public delegate void OrderStateChangeHandler(int OrderID);
    public  event OrderStateChangeHandler OrderStateChanged;

    public Property theProperty = null;

    public void Start() {
        // 初始化字典
        orderDict = new Dictionary<int, PairOrder>();
        theProperty = GameObject.Find("Deliveryman").GetComponent<Property>();
    }
    public void Update() {
        if (orderDict.Count - (theProperty.allCapacity - theProperty.nowCapacity) >= 3) {
            if (TutorialManagerBehaviour.timeslow == false) {
                TutorialManagerBehaviour.TimeSlow();
            }
        }
    }

    // 触发事件
    protected virtual void OnOrderStateChanged(int OrderID)
    {
        OrderStateChanged?.Invoke(OrderID);
    }

    // 字典的增删查改操作接口
    public void AddOrder(PairOrder order)
    {
        orderDict.Add(order.OrderID, order);
        // 调用事件
        OnOrderStateChanged(order.OrderID);
    }

    public void RemoveOrder(int OrderID)
    {
        if (orderDict.ContainsKey(OrderID))//TODO:这里应该还要做状态判定
        {
            orderDict.Remove(OrderID);
            // 调用事件
            OnOrderStateChanged(OrderID);
        }
    }

    public PairOrder GetOrder(int OrderID)
    {
        if (orderDict.ContainsKey(OrderID))
        {
            return orderDict[OrderID];
        }
        return null;
    }

    public bool IfOrderExist(int OrderID)
    {
        return orderDict.ContainsKey(OrderID);
    }

    public bool IsClear()
    {
        //判断字典中是否还有活跃订单
        if (orderDict.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateOrder(PairOrder order)
    {
        if (orderDict.ContainsKey(order.OrderID))
        {
            orderDict[order.OrderID] = order;
            // 调用事件
            OnOrderStateChanged(order.OrderID);
        }
    }

    // 示例订阅方法
    public void SubscribeToOrderStateChange(OrderStateChangeHandler handler)
    {
        OrderStateChanged += handler;
    }

    // 示例取消订阅方法
    public void UnsubscribeFromOrderStateChange(OrderStateChangeHandler handler)
    {
        OrderStateChanged -= handler;
    }
}