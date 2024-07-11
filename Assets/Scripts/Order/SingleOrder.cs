using UnityEngine;
using System.Collections;

public class SingleOrder : MonoBehaviour
{

    public int OrderID;
    private int pid;
    private Vector2 position;
    // private float price;
    // private float distance;
    private bool isFrom;//true for from, false for to
    public float LifeTime = 5f;

    public void Start()
    {
        pid = -1;
        // price = 0;
        // distance = 0;
        isFrom = true;
    }

    public void Update()
    {
        
    }
    // pid operation
    public int Getpid()
    {
        return pid;
    }
    public void SetPid(int pid)
    {
        this.pid = pid;
        //TODO: get position from pid
    }

    // isFrom operation
    public bool GetIsFrom()
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

    // position operation
    public Vector2 GetPosition()
    {
        return position;
    }
}