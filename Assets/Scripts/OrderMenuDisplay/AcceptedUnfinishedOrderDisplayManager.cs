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
    public OrderInfo getFirstOrder() {
        if(menuView.getSize() == 0)
            return null;
        return ((OrderItemBehaviour)menuView.getItemAt(0)).getOrderInfo();
    }

    public void removeOrder(int orderID, LocationType locationType) {
        OrderInfo orderInfo = getFirstOrder();
        if (orderInfo == null) {
            Debug.LogError("No order to remove");
        }
        else
        {
            Debug.Log("test1:Order to remove: " + orderInfo.pid + " " + orderInfo.orderID + " " + orderInfo.locationType + " " + orderInfo.dueTime);
        }
        int size = menuView.getSize();
        for (int i = 0; i < size; i++) {
            OrderItemBehaviour itemModel = (OrderItemBehaviour)menuView.getItemAt(i);
            if (itemModel.getOrderInfo().orderID == orderID && itemModel.getOrderInfo().locationType == locationType) {
                menuView.removeAt(i);
                orderInfo = getFirstOrder();
                if (orderInfo == null) {
                    Debug.LogError("No order to remove");
                }
                else
                {
                    Debug.Log("test1:After Remove: " + orderInfo.pid + " " + orderInfo.orderID + " " + orderInfo.locationType + " " + orderInfo.dueTime);
                }
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
    public OrderInfo(TimeSpan dueTime, Color color, LocationType locationType, int pid, int orderID) {
        this.dueTime = dueTime;
        this.color = color;
        this.locationType = locationType;
        this.pid = pid;
        this.orderID = orderID;
    }
}

