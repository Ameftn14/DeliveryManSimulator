using UnityEngine;
using System;

public class RandomEventManager : MonoBehaviour{
    public static RandomEventManager Instance { get; set; }
    public  Property property = null;
    public SearchRoad searchRoad = null;
    public TimeSpan NPeventTime;
    public TimeSpan NPrecoveryTime;
    public bool Prepared ;
    public int NotPreID = -1;

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
        Prepared = true;
        NPrecoveryTime = new TimeSpan(0, 0, 0);
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
        searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
    }

    void Update() {
        if(!Prepared) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OrderDB.Instance.orderDict[NotPreID].NotPreparedTime -= new TimeSpan(0, 2, 0);
                searchRoad.recoveryTime -= new TimeSpan(0, 2, 0);
                AudioSource audioSource = GameObject.Find("LateVoice").GetComponent<AudioSource>();
                audioSource.Play();
            }
            if (searchRoad.recoveryTime <= VirtualClockUI.Instance.GetTime()) {
                FromPrepared(NotPreID);
            }
        }
    }

    private void FromNotPrepared(int orderID) {
        //停一会
        TutorialManagerBehaviour.FromNotPrepared();
        Prepared = false;
        NotPreID = orderID;
        NPeventTime = VirtualClockUI.Instance.GetTime();
        int NPminutes = 20 + 5 * DeliverymanManager.Instance.round;
        NPrecoveryTime = new TimeSpan(0, NPminutes, 0) + VirtualClockUI.Instance.GetTime();
        searchRoad.FallintoStop(new TimeSpan(0, NPminutes, 0), orderID);
        OrderDB.Instance.orderDict[orderID].NotPreparedTime = NPrecoveryTime;
        OrderDB.Instance.orderDict[orderID].SetIsStop(true);
        Debug.Log("Fall into stop");
    }

    public void FromPrepared(int orderID) {
        NPrecoveryTime = new TimeSpan(0, 0, 0);
        Prepared = true;
        OrderDB.Instance.orderDict[orderID].SetIsStop(false);
        NotPreID = -1;
    }

    private void LateArriveTo(int orderID) {
        //扣钱
        TutorialManagerBehaviour.LateArriveTo();
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        theOrder.SetPrice(theOrder.GetPrice() * 2 / 3);
        Debug.Log("Late arrive to");
    }
    private void OnTimeArriveTo(int orderID) {
        //加钱
        TutorialManagerBehaviour.OnTimeArriveTo();
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        theOrder.SetPrice(theOrder.GetPrice() * 4 / 3);
        Debug.Log("On time arrive to");
    }

    public void WhenPickUp(int orderID) {
        //看这个订单的level
        PairOrder theOrder = OrderDB.Instance.orderDict[orderID];
        //随机生成1-100的数
        int random = UnityEngine.Random.Range(0, 100); 
        int threshold = GetNPThreshold(theOrder.level, DeliverymanManager.Instance.round);
        if (random < threshold) {
            FromNotPrepared(orderID);
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
                threshold = 15;
            } 
            else{
                threshold = 25;
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
                threshold = 14;
            } 
            else{
                threshold = 20;
            }
            if (random < threshold) {
                OnTimeArriveTo(orderID);
            }
        }
    }

    private int GetNPThreshold(int level, int round){
        switch(round)
        {
            case 0:
                if(level == 1) return 0;
                else if(level == 2) return 0;
                else if(level == 3) return 0;
                break;
            case 1:
                if(level == 1) return 0;
                if(level == 2) return 5;
                if(level == 3) return 10;
                break;
            case 2:
                if(level == 1) return 5;
                if(level == 2) return 8;
                if(level == 3) return 14;
                break;
            case 3:
                if(level == 1) return 7;
                if(level == 2) return 12;
                if(level == 3) return 18;
                break;
            case 4:
            default:
                if(level == 1) return 10;
                if(level == 2) return 15;
                if(level == 3) return 20;
                break;
        }
        return 0;
    }

    // private int GetLateThreshold(int level, int round){
    //     switch(round)
    //     {
    //         case 0:
    //             if(level == 1) return 0;
    //             else if(level == 2) return 0;
    //             else if(level == 3) return 0;
    //             break;
    //         case 1:
    //             if(level == 1) return 10;
    //             if(level == 2) return 5;
    //             if(level == 3) return 0;
    //             break;
    //         case 2:
    //             if(level == 1) return 15;
    //             if(level == 2) return 8;
    //             if(level == 3) return 5;
    //             break;
    //         case 3:
    //             if(level == 1) return 18;
    //             if(level == 2) return 12;
    //             if(level == 3) return 7;
    //             break;
    //         case 4:
    //         default:
    //             if(level == 1) return 22;
    //             if(level == 2) return 15;
    //             if(level == 3) return 10;
    //             break;
    //     }
    //     return 0;
    // }
    
}