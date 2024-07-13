using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Property : MonoBehaviour {
    //public static Property Instance { get; private set; }
    private float maxSpeed = 20.0f;
    private int maxCapacity = 5;

    public float speed;

    public int allCapacity;

    public int nowCapacity;

    public int money;

    public int finishedcount;
    // Start is called before the first frame update


    void Start() {
        speed = DeliverymanManager.speed;
        allCapacity = DeliverymanManager.allCapacity;
        money = DeliverymanManager.money;
        finishedcount = DeliverymanManager.finishedcount;
        //DontDestroyOnLoad(gameObject);
        nowCapacity = allCapacity;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("In SampleScene money is" + money);
        }
    }

    public void increaseSpeed() {
        if (speed < maxSpeed) {
            Debug.Log("Speed up!");
            speed = speed * (float)1.5;
            money -= 100;
        } else {
            Debug.Log("Reach the maxspeed");
        }

    }

    public void increaseAllCapacity() {
        if (allCapacity < maxCapacity) {
            Debug.Log("allCapacity + 1!");
            allCapacity += 1;
            money -= 200;
        } else {
            Debug.Log("Reach the maxcapacity");
        }
    }

    public void increaseNowCapacity() {
        nowCapacity++;
    }

    public void decreaseNowCapacity() {
        nowCapacity--;
    }

    public void increaseFinishedCount() {
        finishedcount++;
    }
    public void increaseMoney(int earning) {
        money += earning;
    }
}
