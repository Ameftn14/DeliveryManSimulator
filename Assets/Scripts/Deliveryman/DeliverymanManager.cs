using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverymanManager : MonoBehaviour {
    public static DeliverymanManager Instance { get; private set; }

    public static float speed = 10.0f;

    public static int allCapacity = 3;

    public static int money = 100;

    public static int finishedcount = 0;

    public static float speedUp = 25.0f;
    public static float timeSlow = 25.0f;

    public static int speedAvailable = 2;
    public static int capacityAvailable = 2;
    public static int speedUpAvailable = 2;
    public static int timeSlowAvailable = 2;

    public static float addSpeedUp = 25.0f;
    public static float addTimeSlow = 25.0f;


    private Property property;
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
        property = deliveryman.GetComponent<Property>();
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
        BalanceDiplayBehaviour balanceDiplayBehaviour = GameObject.Find("Money").GetComponent<BalanceDiplayBehaviour>();
        balanceDiplayBehaviour.setBalance(money);
        SkillBarBehaviour speedSkillBar = GameObject.Find("SpeedSkillBar").GetComponent<SkillBarBehaviour>();
        speedSkillBar.setPercentage(searchRoad.realSpeedUp / 25.0f);
        SkillBarBehaviour timeSlowSkillBar = GameObject.Find("TimeSlowSkillBar").GetComponent<SkillBarBehaviour>();
        timeSlowSkillBar.setPercentage(searchRoad.realTimeSlow / 25.0f);
    }
}
