using UnityEngine;
using System;
using System.Collections.Generic;

public class OrderDB: MonoBehaviour
{
    // PairOrder根据OrderID来索引建立字典
    public Dictionary<int, PairOrder> orderDict = new Dictionary<int, PairOrder>();

    // 声明委托，通过OrderID用于在订单状态发生变化时通知其他对象
    public delegate void OrderStateChangeHandler(int OrderID);
    public event OrderStateChangeHandler OrderStateChanged;

    // 触发事件
    protected virtual void OnOrderStateChanged(int OrderID)
    {
        if (OrderStateChanged != null)
        {
            OrderStateChanged(OrderID);
        }
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