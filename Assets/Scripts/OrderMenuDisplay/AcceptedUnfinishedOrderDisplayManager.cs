using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AcceptedUnfinishedOrderDisplayManager : MonoBehaviour {
    public OrderMenuView menuView;
    private static AcceptedUnfinishedOrderDisplayManager instance;
    public static AcceptedUnfinishedOrderDisplayManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<AcceptedUnfinishedOrderDisplayManager>();
            }
            return instance;
        }
    }

    public void Start() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        Debug.Assert(menuView != null);
    }
    public void appendNewOrder(OrderInfo order) {
        ItemModel itemModel = null;
        if (order.locationType == LocationType.Restaurant) {
            itemModel = OrderItemBehaviour.spawnNewRestaurantOrderItem(order);
        } else if (order.locationType == LocationType.Customer) {
            itemModel = OrderItemBehaviour.spawnNewCustomerOrderItem(order);
        }
        Debug.Log("itemModel: " + itemModel.getIndex());
        // itemModel.gameObject.name = "Item " + menuView.transform.childCount;
        if (itemModel == null) {
            Debug.LogError("itemModel is null");
            return;
        }
        menuView.appendItem(itemModel);
    }
    public OrderInfo getFirstOrder() {
        if (menuView.getSize() == 0)
            return null;
        return ((OrderItemBehaviour)menuView.getItemAt(0)).getOrderInfo();
    }

    public void removeOrder(int orderID, LocationType locationType) {
        int size = menuView.getSize();
        for (int i = 0; i < size; i++) {
            OrderItemBehaviour itemModel = (OrderItemBehaviour)menuView.getItemAt(i);
            if (itemModel.getOrderInfo().orderID == orderID && itemModel.getOrderInfo().locationType == locationType) {
                menuView.removeAt(i);
                return;
            }
        }
    }


}

public enum LocationType {
    Restaurant,
    Customer
}

public class OrderInfo {
    public readonly TimeSpan dueTime;
    public readonly Color color;
    public readonly LocationType locationType;
    public readonly int pid;
    public readonly int orderID;

    public readonly int reward;
    public OrderInfo(TimeSpan dueTime, Color color, LocationType locationType, int pid, int orderID, int reward = 10721) {
        this.dueTime = dueTime;
        this.color = color;
        this.locationType = locationType;
        this.pid = pid;
        this.orderID = orderID;
        this.reward = reward;
    }
}

