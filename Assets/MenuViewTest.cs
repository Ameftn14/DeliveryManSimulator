using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class MenuViewTest : MonoBehaviour {
    // Start is called before the first frame update
    public MenuView menuView;
    public int index;
    int n = 0;
    public void setIndex(string index) {
        Debug.Log(index);
        this.index = Convert.ToInt32(index);
    }
    public void add() {
        OrderInfo info = new OrderInfo(new TimeSpan(01, 02, 03), new Color(0, 0, 0, 255), LocationType.Customer);
        ItemModel item = OrderItemBehaviour.spawnNewCustomerOrderItem(info);
        item.gameObject.name = "Item " + n++;
        menuView.insertAt(item, index);
    }
    public void remove() {
        menuView.removeAt(index);
    }
}
