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
    public void appendNewOrder() {
        // TODO this is for static testing
        OrderInfo order;
        order = new OrderInfo(new TimeSpan(12, 13, 14), new Color(0, 0, 0, 255), LocationType.Customer);

        //OrderItemBehaviour itemModel;
        ItemModel itemModel;
        if (order.locationType == LocationType.Restaurant) {
            itemModel = OrderItemBehaviour.spawnNewRestaurantOrderItem();
        } else {
            itemModel = OrderItemBehaviour.spawnNewCustomerOrderItem();
        }
        menuView.appendItem(itemModel);
    }

    // TODO hook this up
    public void removeOrder(int pid) {
        // menuView.removeAt(index);
    }
}

public enum LocationType {
    Restaurant,
    Customer
}

public class OrderInfo : ItemModel {
    // TODO pid or whatever
    public readonly TimeSpan dueTime;
    public readonly Color color;
    public readonly LocationType locationType;
    public OrderInfo(TimeSpan dueTime, Color color, LocationType locationType) {
        this.dueTime = dueTime;
        this.color = color;
        this.locationType = locationType;
    }
}

