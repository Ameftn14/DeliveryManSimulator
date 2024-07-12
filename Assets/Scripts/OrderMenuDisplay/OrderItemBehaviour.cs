using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Add this line to import the UnityEngine.UI namespace

public class OrderItemBehaviour : ItemModel, IPointerEnterHandler, IPointerExitHandler {
    private OrderInfo orderInfo;
    public GameObject imageObject;
    public TMP_Text dueTimeText;
    public Color defaultColor = new Color(0, 0, 0, 255);
    public MenuItemTimeLeftBarController timeLeftBarController;
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
        Debug.Assert(dueTimeText != null);
        Debug.Assert(imageObject != null);
        Debug.Assert(timeLeftBarController != null);
        timeLeftBarController = GetComponentInChildren<MenuItemTimeLeftBarController>();
        timeLeftBarController.setDueTime(orderInfo.dueTime);
        init(); // this is from the super class
    }
    public static ItemModel spawnNewRestaurantOrderItem(OrderInfo orderInfo) {
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Menu Item/Version 2/Restaurant Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        return orderItemBehaviour;
    }
    public static ItemModel spawnNewCustomerOrderItem(OrderInfo orderInfo) {
        //return spawnNewRestaurantOrderItem(orderInfo);

        // original code
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Menu Item/Version 2/Customer Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        return orderItemBehaviour;
    }

    // when the mouse hovers over the order item:
    // 1. highlight the order item, both in the menu and in the game world
    // 2. show more info about the order? so we should make this expandable
    // 3. if player is dragging another item, stop the dragging if that order is the restaurant order of this one
    public void OnPointerEnter(PointerEventData eventData) {
        throw new NotImplementedException();
    }
    override public void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
    }

    // when the mouse leaves the order item:
    // 1. unhighlight the order item
    // 2. hide the extra info
    public void OnPointerExit(PointerEventData eventData) {
        throw new NotImplementedException();
    }

    // //this is for testing purpose
    // void Update() {
    //     syncDisplay();
    // }
}