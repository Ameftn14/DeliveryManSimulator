using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : MonoBehaviour {

    public float speed = 5.0f;

    public int capacity = 2;

    public int money = 100;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            increaseSpeed();
        }

        if (Input.GetKeyDown(KeyCode.K)) {
            increaseCapacity();
        }
    }

    void increaseSpeed() {
        Debug.Log("Speed up!");
        speed = speed * (float)1.5;
        money -= 100;
    }

    void increaseCapacity() {
        Debug.Log("Capacity + 1!");
        capacity += 1;
        money -= 200;
    }
    void increaseMoney(int earning) {
        money += earning;
    }
}
