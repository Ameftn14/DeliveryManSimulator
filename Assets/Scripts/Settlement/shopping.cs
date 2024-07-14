using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEditor;
using UnityEngine;
using System;

public class Shopping : MonoBehaviour {
    private float addSpeed = 1.5f;
    private int addCapacity = 1;
    private float addSpeedUp = 25.0f;
    private float addTimeSlow = 25.0f;
    private int shoppingCount = 2;
    // private float speed = DeliverymanManager.speed;
    // private int allCapacity = DeliverymanManager.allCapacity;
    // private int money = DeliverymanManager.money;
    // Start is called before the first frame update

    public enum UpgradeType {
        Speed, Capacity, TimeSlow, TemporarySpeedUp
    };


    public class UpgradeOption {
        public UpgradeType type;
        public bool available;
    }

    void Start() {
        List<UpgradeOption> options = new List<UpgradeOption>();

        // foreach (UpgradeType type in Enum.GetValues(typeof(UpgradeType))) {
        //     UpgradeOption upgrade = new UpgradeOption();
        //     upgrade.type = type;
        //     if(DeliverymanManager.)
        //     options.Add(new UpgradeOption { type = type, available = true });
        // }
        UpgradeOption upgradeSpeed = new UpgradeOption();
        upgradeSpeed.type = UpgradeType.Speed;
        if(DeliverymanManager.speedAvailable>0){
            upgradeSpeed.available = true;
        }
        else{
            upgradeSpeed.available = false;
        }


        UpgradeOption upgradeCapacity = new UpgradeOption();
        upgradeCapacity.type = UpgradeType.Capacity;
        if(DeliverymanManager.capacityAvailable>0){
            upgradeCapacity.available = true;
        }
        else{
            upgradeCapacity.available = false;
        }


        UpgradeOption upgradeSpeedUp = new UpgradeOption();
        upgradeSpeedUp.type = UpgradeType.TemporarySpeedUp;
        if(DeliverymanManager.speedUpAvailable>0){
            upgradeSpeedUp.available = true;
        }
        else{
            upgradeSpeedUp.available = false;
        }


        UpgradeOption upgradeTimeSlow = new UpgradeOption();
        upgradeTimeSlow.type = UpgradeType.TimeSlow;
        if(DeliverymanManager.timeSlowAvailable>0){
            upgradeTimeSlow.available = true;
        }
        else{
            upgradeTimeSlow.available = false;
        }

        options.Add(upgradeSpeed);
        options.Add(upgradeCapacity);
        options.Add(upgradeSpeedUp);
        options.Add(upgradeTimeSlow);



        //initOptions(options);
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetKeyDown(KeyCode.Y)) {
        //     DeliverymanManager.money += 100;
        // }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("In shopping: speed:" + DeliverymanManager.speed + "allCapacity:" + DeliverymanManager.allCapacity + "speedup:" + DeliverymanManager.speedUp + "timeSlow:" + DeliverymanManager.timeSlow + "money:" + DeliverymanManager.money);
        }
    }

    void doPurchace(UpgradeOption option) {
        //Speed, Capacity, TimeSlow, TemporarySpeedUp
        UpgradeType type = option.type;
        switch (type) {
            case UpgradeType.Speed:
                if (shoppingCount > 0 && DeliverymanManager.money > 0) {
                    DeliverymanManager.speed *= addSpeed;
                    DeliverymanManager.money -= 50;
                    shoppingCount--;
                }
                break;
            case UpgradeType.Capacity:
                if (shoppingCount > 0 && DeliverymanManager.money > 0) {
                    DeliverymanManager.allCapacity += addCapacity;
                    DeliverymanManager.money -= 50;
                    shoppingCount--;
                }
                break;
            case UpgradeType.TimeSlow:
                if (shoppingCount > 0 && DeliverymanManager.money > 0) {
                    DeliverymanManager.timeSlow += addTimeSlow;
                    DeliverymanManager.money -= 50;
                    shoppingCount--;
                }
                break;
            case UpgradeType.TemporarySpeedUp:
                if (shoppingCount > 0 && DeliverymanManager.money > 0) {
                    DeliverymanManager.speedUp += addSpeedUp;
                    DeliverymanManager.money -= 50;
                    shoppingCount--;
                }
                break;
            default:
                Debug.Log("error");
                break;

        }

        Debug.Log("Now shoppingCount: " + shoppingCount+" Money: "+ DeliverymanManager.money);
    }

}
