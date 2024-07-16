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
    public int latecount;
    public int badcount;

    public float speedUp;
    public float timeSlow;
    // Start is called before the first frame update


    void Start() {
        speed = DeliverymanManager.speed;
        allCapacity = DeliverymanManager.allCapacity;
        money = DeliverymanManager.money;
        finishedcount = DeliverymanManager.finishedcount;
        latecount = DeliverymanManager.latecount;
        badcount = DeliverymanManager.badcount;
        speedUp = DeliverymanManager.speedUp;
        timeSlow = DeliverymanManager.timeSlow;
        //DontDestroyOnLoad(gameObject);
        nowCapacity = allCapacity;

    }

    // Update is called once per frame
    void Update() {

        SearchRoad searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
        BalanceDiplayBehaviour balanceDiplayBehaviour = GameObject.Find("balance bar").GetComponent<BalanceDiplayBehaviour>();
        balanceDiplayBehaviour.setBalance(money);
        SkillBarBehaviour speedSkillBar = GameObject.Find("temp speed up skill").GetComponent<SkillBarBehaviour>();
        speedSkillBar.setPercentage(searchRoad.speedUpPercentage);
        SkillBarBehaviour timeSlowSkillBar = GameObject.Find("time slow down skill").GetComponent<SkillBarBehaviour>();
        timeSlowSkillBar.setPercentage(searchRoad.timeSlowPercentage);
    }

}
