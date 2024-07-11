using UnityEngine;
using UnityEngine.UI;
using System

public class PairOrder : MonoBehaviour
{
    public int OrderID;
    public SingleOrder from;
    public SingleOrder to;

    private float price;
    private float distance;

    private Timespan Deadline;

    public Start()
    {

    }

    public Update()
    {

    }

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
    public void SetPrice(float price)
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
}