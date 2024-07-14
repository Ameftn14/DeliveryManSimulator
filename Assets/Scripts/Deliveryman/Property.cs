using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Property : MonoBehaviour {
    //public static Property Instance { get; private set; }
    public float speed;
    public int allCapacity;
    public int nowCapacity;
    public int money;
    public int finishedcount;

    public float speedUp;
    public float timeSlow;
    // Start is called before the first frame update


    void Start() {
        speed = DeliverymanManager.speed;
        allCapacity = DeliverymanManager.allCapacity;
        money = DeliverymanManager.money;
        finishedcount = DeliverymanManager.finishedcount;
        speedUp = DeliverymanManager.speedUp;
        timeSlow = DeliverymanManager.timeSlow;
        //DontDestroyOnLoad(gameObject);
        nowCapacity = allCapacity;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("In Property: speed:" + speed + "allCapacity:" + allCapacity + "speedup:" + speedUp + "timeSlow:" + timeSlow + "money:" + money);
        }
        BalanceDiplayBehaviour balanceDiplayBehaviour = GameObject.Find("Money").GetComponent<BalanceDiplayBehaviour>();
        balanceDiplayBehaviour.setBalance(money);
    }

}
