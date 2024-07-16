using UnityEngine;
using System;

public class RandomEventManager : MonoBehaviour{
    public static RandomEventManager Instance { get; set; }
    public  Property property = null;
    public SearchRoad searchRoad = null;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
    //需要接口：pickup, finish状态变化
    //骑手停一会
    //改变钱

    void Start() {
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
        searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
    }

    void Update() {
    }

    private void FromNotPrepared() {
        //停一会
        searchRoad.FallintoStop(new TimeSpan(0, 10, 0));
        Debug.Log("Fall into stop");
    }

    private void LateArriveTo(int orderID) {
        //扣钱
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        theOrder.SetPrice(theOrder.GetPrice() * 2 / 3);
        Debug.Log("Late arrive to");
    }
    private void OnTimeArriveTo(int orderID) {
        //加钱
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        theOrder.SetPrice(theOrder.GetPrice() * 4 / 3);
        Debug.Log("On time arrive to");
    }

    public void WhenPickUp(int orderID) {
        //看这个订单的level
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        //随机生成1-100的数
        int random = UnityEngine.Random.Range(0, 100); 
        int threshold;
        if (theOrder.level == 1) {
            threshold = 15;
        } 
        else if (theOrder.level == 2) {
            threshold = 10;
        } 
        else{
            threshold = 3;
        }

        if (random < threshold) {
            FromNotPrepared();
        }

    }

    public void WhenArrive(int orderID) {
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        int random = UnityEngine.Random.Range(0, 100);
        int threshold;
        if (theOrder.GetIsLate()) {
            if (theOrder.level == 1) {
            threshold = 6;
            } 
            else if (theOrder.level == 2) {
                threshold = 10;
            } 
            else{
                threshold = 15;
            }
            if (random < threshold) {
                LateArriveTo(orderID);
            }
        } 
        else {
            if (theOrder.level == 1) {
            threshold = 7;
            } 
            else if (theOrder.level == 2) {
                threshold = 13;
            } 
            else{
                threshold = 17;
            }
            if (random < threshold) {
                OnTimeArriveTo(orderID);
            }
        }
    }
    
}