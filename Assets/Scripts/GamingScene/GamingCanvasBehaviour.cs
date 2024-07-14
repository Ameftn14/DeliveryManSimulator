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

    //public GameObject PurchaseMenu;
    //private bool isMenuVisible = false; // 用于追踪菜单是否可见
    // Start is called before the first frame update
    void Start() {
        orderDB = GameObject.Find("OrderDB").GetComponent<OrderDB>();
        virtualClockUI = GameObject.Find("Time").GetComponent<VirtualClockUI>();
        //PurchaseMenu = GameObject.Find("PurchaseMenu");
    }

    // Update is called once per frame
    void Update() {
        TimeSpan timespan = virtualClockUI.GetTime();
        // 检测空格键是否被按下
        if (orderDB.IsClear() && timespan.Hours >= 10) {
            //Destroy(instance);
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Settlement");
        }
    }
}
