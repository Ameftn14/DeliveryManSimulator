using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor;
using UnityEngine;
using System;

public class Shopping : MonoBehaviour {
    private float addSpeed = 1.5f;
    private int addCapacity = 1;
    private float addSpeedUp = 15.0f;
    private float addTimeSlow = 10.0f;
    public int shoppingCount = 2;
    // private float speed = DeliverymanManager.speed;
    // private int allCapacity = DeliverymanManager.allCapacity;
    // private int money = DeliverymanManager.money;
    // Start is called before the first frame update

    // public enum UpgradeType {
    //     Speed, Capacity, TimeSlow, TemporarySpeedUp
    // };


    // public class UpgradeOption {
    //     public UpgradeType type;
    //     public bool available;
    // }

    public List<UpgradeOption> options = new List<UpgradeOption>();

    void Awake() {

        // PermanentSpeedBoost,
        // BiggerStorage,
        // TempararySpeedBoost,
        // TempararyTimeSlow
        bool tempSpeedIsAvailable;
        if (DeliverymanManager.speedAvailable > 0) {
            tempSpeedIsAvailable = true;
        } else {
            tempSpeedIsAvailable = false;
        }
        options.Add(new UpgradeOption(UpgradeType.PermanentSpeedBoost,tempSpeedIsAvailable));


        
        bool tempCapacityIsAvailable;
        if (DeliverymanManager.capacityAvailable > 0) {
            tempCapacityIsAvailable = true;
        } else {
            tempCapacityIsAvailable = false;
        }
        options.Add(new UpgradeOption(UpgradeType.BiggerStorage,tempCapacityIsAvailable));


        
        bool tempSpeedUpIsAvailable;
        if (DeliverymanManager.speedUpAvailable > 0) {
            tempSpeedUpIsAvailable = true;
        } else {
            tempSpeedUpIsAvailable = false;
        }
        options.Add(new UpgradeOption(UpgradeType.TempararySpeedBoost,tempSpeedUpIsAvailable));

        bool tempTimeSlowIsAvailable;
        if (DeliverymanManager.timeSlowAvailable > 0) {
            tempTimeSlowIsAvailable = true;
        } else {
            tempTimeSlowIsAvailable = false;
        }
        options.Add(new UpgradeOption(UpgradeType.TempararyTimeSlow,tempTimeSlowIsAvailable));


        //initOptions(options);
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(KeyCode.Y)) {
        //     DeliverymanManager.money += 100;
        // }

        // if (Input.GetKeyDown(KeyCode.Q)) {
        //     Debug.Log("In shopping: speed:" + DeliverymanManager.speed + "allCapacity:" + DeliverymanManager.allCapacity + "speedup:" + DeliverymanManager.speedUp + "timeSlow:" + DeliverymanManager.timeSlow + "money:" + DeliverymanManager.money);
        // }
    }

    public void doPurchace(UpgradeOption option) {
        // PermanentSpeedBoost,
        // BiggerStorage,
        // TempararySpeedBoost,
        // TempararyTimeSlow
        UpgradeType type = option.type;
        switch (type) {
            case UpgradeType.PermanentSpeedBoost:
                if (shoppingCount > 0) {
                    DeliverymanManager.speed *= addSpeed;
                    DeliverymanManager.speedAvailable--;
                    shoppingCount--;
                }
                break;
            case UpgradeType.BiggerStorage:
                if (shoppingCount > 0) {
                    DeliverymanManager.allCapacity += addCapacity;
                    DeliverymanManager.capacityAvailable--;
                    shoppingCount--;
                }
                break;
            case UpgradeType.TempararyTimeSlow:
                if (shoppingCount > 0) {
                    DeliverymanManager.timeSlow += addTimeSlow;
                    DeliverymanManager.timeSlowAvailable--;
                    shoppingCount--;
                }
                break;
            case UpgradeType.TempararySpeedBoost:
                if (shoppingCount > 0) {
                    DeliverymanManager.speedUp += addSpeedUp;
                    DeliverymanManager.speedUpAvailable--;
                    shoppingCount--;
                }
                break;
            default:
                Debug.Log("error");
                break;

        }
        if (shoppingCount == 0) {
            GameObject Canvas = GameObject.Find("Canvas");
            GameObject nextDayPanel = Canvas.transform.Find("NextDayPanel").gameObject;
            nextDayPanel.SetActive(true);
        }
        Debug.Log("Now shoppingCount: " + shoppingCount + " Money: " + DeliverymanManager.money);
    }
    

}
