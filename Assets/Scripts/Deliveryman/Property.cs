using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Property : MonoBehaviour
{
    public static Property Instance { get; private set; }
    private float maxSpeed = 20.0f;
    private int maxCapacity = 5;

    public static float speed = 10.0f;

    public static int allCapacity = 3;

    public static int nowCapacity;

    public static int money = 100;

    public static int finishedcount = 0;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
        nowCapacity = allCapacity;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.P)){
            Debug.Log("In SampleScene money is" + money);
        }
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
