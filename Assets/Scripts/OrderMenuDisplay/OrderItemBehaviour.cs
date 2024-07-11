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
    }
    public OrderInfo getOrderInfo() {
        return orderInfo;
    }
    public void setDisplayEffect() {
        Color colorToSet;
        // colorToSet = defaultColor;
        colorToSet = orderInfo.color;
        imageObject.GetComponent<Image>().color = colorToSet;
        Debug.Assert(dueTimeText != null);
        // Debug.Log("due time: " + orderInfo.dueTime);
        dueTimeText.text = orderInfo.dueTime.ToString("HH:mm");
    }
    void Start() {
        init();
        // build the view
        Debug.Assert(dueTimeText != null);
        setDisplayEffect();
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
        setDisplayEffect();
    }
}