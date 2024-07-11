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
        ItemModel item = ItemModel.spawnNewItem();
        item.gameObject.name = "Item " + n++;
        menuView.insertAt(item, index);
    }
    public void remove() {
        menuView.removeAt(index);
    }
}
