using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliverymanManager : MonoBehaviour {
    public static DeliverymanManager Instance { get; private set; }

    public static float speed = 10.0f;

    public static int allCapacity = 3;

    public static int money = 100;

    public static int finishedcount = 0;

    public static float speedUp = 15.0f;
    public static float timeSlow = 10.0f;

    public static int speedAvailable = 2;
    public static int capacityAvailable = 2;
    public static int speedUpAvailable = 2;
    public static int timeSlowAvailable = 2;

    public static float addSpeedUp = 15.0f;
    public static float addTimeSlow = 10.0f;
    public int round = 0;


    // Start is called before the first frame update
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start() {
    }

    // Update is called once per frame
    void Update() {
        GameObject deliveryman = GameObject.Find("Deliveryman");
        if (deliveryman == null) {
            return;
        }
        Property property = deliveryman.GetComponent<Property>();
        if (property == null) {
            return;
        }

        if (money != property.money) {
            money = property.money;
        }
        if (finishedcount != property.finishedcount) {
            finishedcount = property.finishedcount;
        }
        SearchRoad searchRoad = GameObject.Find("Deliveryman").GetComponent<SearchRoad>();
        BalanceDiplayBehaviour balanceDiplayBehaviour = GameObject.Find("balance bar").GetComponent<BalanceDiplayBehaviour>();
        balanceDiplayBehaviour.setBalance(money);
        SkillBarBehaviour speedSkillBar = GameObject.Find("temp speed up skill").GetComponent<SkillBarBehaviour>();
        speedSkillBar.setPercentage(searchRoad.realSpeedUp / 25.0f);
        SkillBarBehaviour timeSlowSkillBar = GameObject.Find("time slow down skill").GetComponent<SkillBarBehaviour>();
        timeSlowSkillBar.setPercentage(searchRoad.realTimeSlow / 25.0f);
        InventoryBehaviour inventoryBehaviour = GameObject.Find("inventory").GetComponent<InventoryBehaviour>();
        inventoryBehaviour.SetCapacity(property.allCapacity);
        inventoryBehaviour.SetOccupiedCnt(property.allCapacity - property.nowCapacity);
    }
}
