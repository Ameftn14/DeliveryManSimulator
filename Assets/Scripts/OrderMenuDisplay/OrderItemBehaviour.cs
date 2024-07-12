using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Add this line to import the UnityEngine.UI namespace

public class OrderItemBehaviour : ItemModel {
    private OrderInfo orderInfo;
    public GameObject imageObject;
    public TMP_Text dueTimeText;
    public Color defaultColor = new Color(0, 0, 0, 255);
    private void setOrderInfo(OrderInfo orderInfo) {
        this.orderInfo = orderInfo;
        syncDisplay();
    }
    public OrderInfo getOrderInfo() {
        return orderInfo;
    }
    public void syncDisplay() {
        Color colorToSet;
        // colorToSet = defaultColor;
        colorToSet = orderInfo.color;
        imageObject.GetComponent<Image>().color = colorToSet;
        Debug.Assert(dueTimeText != null);
        DateTime d = new DateTime(orderInfo.dueTime.Ticks);
        // string dueTimeTextString = orderInfo.dueTime.ToString();
        string dueTimeTextString = d.ToString("HH:mm"); ;
        dueTimeText.text = dueTimeTextString;
    }
    void Start() {
        init();
        Debug.Assert(dueTimeText != null);
    }
    public static ItemModel spawnNewRestaurantOrderItem(OrderInfo orderInfo) {
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Restaurant Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        return orderItemBehaviour;
    }
    public static ItemModel spawnNewCustomerOrderItem(OrderInfo orderInfo) {
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Customer Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        return orderItemBehaviour;
    }

    //this is for testing purpose
    void Update() {
        syncDisplay();
    }
}