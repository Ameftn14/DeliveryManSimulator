using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Property : MonoBehaviour
{
    private float maxSpeed = 20.0f;
    private int maxCapacity = 5;

    public float speed = 10.0f;

    public int allCapacity = 3;

    public int nowCapacity;

    public int money = 100;

    public int finishedcount = 0;
    // Start is called before the first frame update
    void Start()
    {
        nowCapacity = allCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     increaseSpeed();
        // }
    }

    public void increaseSpeed()
    {
        if (speed < maxSpeed)
        {
            Debug.Log("Speed up!");
            speed = speed * (float)1.5;
            money -= 100;
        }
        else
        {
            Debug.Log("Reach the maxspeed");
        }

    }

    public void increaseAllCapacity()
    {
        if (allCapacity < maxCapacity)
        {
            Debug.Log("allCapacity + 1!");
            allCapacity += 1;
            money -= 200;
        }
        else
        {
            Debug.Log("Reach the maxcapacity");
        }
    }

    public void increaseNowCapacity(){
        nowCapacity ++;
    }

    public void decreaseNowCapacity(){
        nowCapacity --;
    }

    public void increaseFinishedCount()
    {
        finishedcount++;
    }
    public void increaseMoney(int earning)
    {
        money += earning;
    }
}
