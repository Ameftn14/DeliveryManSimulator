using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AcceptedUnfinishedOrderDisplayManager : MonoBehaviour {
    public MenuView menuView;
    public void Start() {
        Debug.Assert(menuView != null);
    }
    public void appendNewOrder(OrderInfo order) {
        ItemModel itemModel = null;
        if (order.locationType == LocationType.Restaurant) {
            itemModel = OrderItemBehaviour.spawnNewRestaurantOrderItem(order);
        } else if (order.locationType == LocationType.Customer) {
            itemModel = OrderItemBehaviour.spawnNewCustomerOrderItem(order);
        }
        itemModel.gameObject.name = "Item " + menuView.transform.childCount;
        if (itemModel == null) {
            Debug.LogError("itemModel is null");
            return;
        }
        menuView.appendItem(itemModel);
    }

    public void removeOrder(int orderID, bool isFrom) {
        int size = menuView.getSize();
        for (int i = 0; i < size; i++) {
            OrderItemBehaviour itemModel = (OrderItemBehaviour)menuView.getItemAt(i);
            if (itemModel.getOrderInfo().orderID == orderID && itemModel.getOrderInfo().isFrom == isFrom) {
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
    // TODO pid or whatever
    public readonly TimeSpan dueTime;
    public readonly Color color;
    public readonly LocationType locationType;
    public readonly int pid;
    public readonly int orderID;
    public readonly bool isFrom;
    public OrderInfo(TimeSpan dueTime, Color color, LocationType locationType, int pid, int orderID, bool isFrom) {
        this.dueTime = dueTime;
        this.color = color;
        this.locationType = locationType;
        this.pid = pid;
        this.orderID = orderID;
        this.isFrom = isFrom;
    }
}

