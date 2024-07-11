using UnityEngine;
using System.Collections;

public class SigelOrder : MonoBehaviour
{

    public int OrderID;
    private int pid;
    // private float price;
    // private float distance;
    private bool isFrom;//true for from, false for to

    // pid operation
    public Getpid()
    {
        return pid;
    }
    public void SetPid(int pid)
    {
        this.pid = pid;
    }

    // price operation
    // public GetPrice()
    // {
    //     return price;
    // }
    // public void SetPrice(float price)
    // {
    //     this.price = price;
    // }

    // // distance operation
    // public GetDistance()
    // {
    //     return distance;
    // }
    // public void SetDistance(float distance)
    // {
    //     this.distance = distance;
    // }

    // isFrom operation
    public GetIsFrom()
    {
        return isFrom;
    }
    public void SetIsFrom(bool isFrom)
    {
        this.isFrom = isFrom;
    }

    // OrderID operation
    public int GetOrderID()
    {
        return OrderID;
    }
    public void SetOrderID(int id)
    {
        this.OrderID = id;
    }
}