using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverymanManager : MonoBehaviour {
    public static DeliverymanManager Instance { get; private set; }

    private float maxSpeed = 20.0f;
    private int maxCapacity = 5;

    public static float speed = 10.0f;

    public static int allCapacity = 3;

    public static int nowCapacity;

    public static int money = 100;

    public static int finishedcount = 0;

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
        //DontDestroyOnLoad(gameObject);
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
        nowCapacity = allCapacity;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Debug.Log("In SampleScene money is" + money);
        }

        if (speed != property.speed){
            speed = property.speed;
        }
        if(allCapacity != property.allCapacity){
            allCapacity = property.allCapacity;
        }
        if(nowCapacity != property.nowCapacity){
            nowCapacity = property.nowCapacity;
        }
        if(money != property.money){
            money = property.money;
        }
        if(finishedcount != property.finishedcount){
            finishedcount = property.finishedcount;
        }
    }
}
