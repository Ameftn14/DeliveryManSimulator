using UnityEngine;

public class OrderDB
{
    //PairOrder根据OrderID来索引建立字典
    public Dictionary<int, PairOrder> orderDict = new Dictionary<int, PairOrder>();

    //字典的增删查改操作接口
    public void AddOrder(PairOrder order)
    {
        orderDict.Add(order.OrderID, order);
    }

    public void RemoveOrder(int OrderID)
    {
        orderDict.Remove(OrderID);
    }

    public PairOrder GetOrder(int OrderID)
    {
        return orderDict[OrderID];
    }

    public void UpdateOrder(PairOrder order)
    {
        orderDict[order.OrderID] = order;
    }

    //声明委托，通过OrderID用于在订单状态发生变化时通知其他对象
    // TODO: 接口管理
    public delegate void OrderStateChange(int OrderID);
    public OrderStateChange orderStateChange;

    //订单状态发生变化时调用委托
    public void OnOrderStateChange(int OrderID)
    {
        orderStateChange(OrderID);
    }
}