using TMPro;
using UnityEngine;
using UnityEngine.UI; // Add this line to import the UnityEngine.UI namespace

public class OrderItemBehaviour : ItemModel {
    public OrderInfo orderInfo;
    public GameObject imageObject;
    public TMP_Text dueTimeText;
    public Color defaultColor = new Color(0, 0, 0, 255);
    public OrderItemBehaviour(OrderInfo orderInfo) {
        this.orderInfo = orderInfo;
    }
    public void setDisplayEffect() {
        Color colorToSet;
        colorToSet = defaultColor;
        // colorToSet = orderInfo.color;
        imageObject.GetComponent<Image>().color = colorToSet;
        Debug.Assert(dueTimeText != null);
        Debug.Log("due time: " + orderInfo.dueTime);
        dueTimeText.text = orderInfo.dueTime.ToString();
    }
    void Start() {
        init();
        // build the view
        Debug.Assert(dueTimeText != null);
        setDisplayEffect();
    }
    public static ItemModel spawnNewRestaurantOrderItem() {
        return spawnNewItem("Prefabs/UI/Restaurant Menu Item");

    }
    public static ItemModel spawnNewCustomerOrderItem() {
        return spawnNewItem("Prefabs/UI/Customer Menu Item");
    }

    //this is for testing purpose
    void Update() {
        setDisplayEffect();
    }
}