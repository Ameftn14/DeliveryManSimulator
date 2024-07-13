using System;
using UnityEngine;

public class MenuViewTest : MonoBehaviour {
    // Start is called before the first frame update
    public OrderMenuView menuView;
    public int index;
    int n = 0;
    public void setIndex(string index) {
        Debug.Log(index);
        this.index = Convert.ToInt32(index);
    }
    public void add() {
        Color color = ColorDictionary.GetColor(n);
        TimeSpan dueTime = VirtualClockUI.Instance.GetTime().Add(new TimeSpan(2, 0, 0));
        OrderInfo restInfo = new OrderInfo(dueTime, color, LocationType.Restaurant, n, n);
        OrderInfo custInfo = new OrderInfo(dueTime, color, LocationType.Customer, n, n);
        ItemModel restItem = OrderItemBehaviour.spawnNewRestaurantOrderItem(restInfo);
        ItemModel custItem = OrderItemBehaviour.spawnNewCustomerOrderItem(custInfo);
        restItem.gameObject.name = "Item " + n++;
        menuView.insertAt(restItem, index);
        custItem.gameObject.name = "Item " + n++;
        menuView.insertAt(custItem, index + 1);
    }
    public void remove() {
        menuView.removeAt(index);
    }
}
