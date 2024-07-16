using UnityEngine;

public class RandomEventManager : MonoBehaviour{
    public static RandomEventManager Instance { get; set; }

    public static OrderDB orderDB = null;
    public static Property property = null;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    //需要接口：pickup, finish状态变化
    //骑手停一会
    //改变钱

    void Start() {
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
    }

    void Update() {
    }

    private void FromNotPrepared() {
        //停一会
    }

    private void LateArriveTo() {
        //扣钱
        
    }
    private void OnTimeArriveTo() {
        //加钱
    }

    public void WhenPickUp(int orderID) {
        //看这个订单的level
        PairOrder theOrder = orderDB.orderDict[orderID];
        //随机生成1-100的数
        int random = Random.Range(0, 100);
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
        PairOrder theOrder = orderDB.orderDict[orderID];
        int random = Random.Range(0, 100);
        int threshold;
        if (theOrder.GetIsLate()) {
            if (theOrder.level == 1) {
            threshold = 8;
            } 
            else if (theOrder.level == 2) {
                threshold = 15;
            } 
            else{
                threshold = 25;
            }
            if (random < threshold) {
                LateArriveTo();
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
                OnTimeArriveTo();
            }
        }
    }
}