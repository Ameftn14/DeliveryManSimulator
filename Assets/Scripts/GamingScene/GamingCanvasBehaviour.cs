using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using TMPro;

public class GamingCanvasBehaviour : MonoBehaviour {
    public OrderDB orderDB;
    public VirtualClockUI virtualClockUI;
    // Start is called before the first frame update
    void Start() {
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        virtualClockUI = GameObject.Find("Time").GetComponent<VirtualClockUI>();
    }

    // Update is called once per frame
    void Update() {
        TimeSpan timespan = virtualClockUI.GetTime();
        // 检测空格键是否被按下
        if (orderDB.IsClear() && timespan.Hours >= 19) {
            //Destroy(instance);
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Settlement");
        }
    }
}
