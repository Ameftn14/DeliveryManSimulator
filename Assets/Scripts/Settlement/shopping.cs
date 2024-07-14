using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

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
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }

        if (Input.GetKeyDown(KeyCode.U) && shoppingCount > 0 && DeliverymanManager.money>0) {
            DeliverymanManager.speed *= addSpeed;
            DeliverymanManager.money -= 50;
            shoppingCount--;
        }

        if (Input.GetKeyDown(KeyCode.I) && shoppingCount > 0&& DeliverymanManager.money>0) {
            DeliverymanManager.allCapacity += addCapacity;
            DeliverymanManager.money -= 50;
            shoppingCount--;
        }

        if (Input.GetKeyDown(KeyCode.O) && shoppingCount > 0&& DeliverymanManager.money>0) {
            DeliverymanManager.speedUp += addSpeedUp;
            DeliverymanManager.money -= 50;
            shoppingCount--;
        }

        if (Input.GetKeyDown(KeyCode.P) && shoppingCount > 0&& DeliverymanManager.money>0) {
            DeliverymanManager.timeSlow += addTimeSlow;
            DeliverymanManager.money -= 50;
            shoppingCount--;
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            DeliverymanManager.money += 100;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("In shopping: speed:" + DeliverymanManager.speed + "allCapacity:" + DeliverymanManager.allCapacity + "speedup:" + DeliverymanManager.speedUp + "timeSlow:" + DeliverymanManager.timeSlow + "money:" + DeliverymanManager.money);
        }
    }


}
