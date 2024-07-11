using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class MenuView : MonoBehaviour {
    public ListModel listModel;
    void Start() {
        Debug.Assert(listModel != null);
    }
    public void appendItem(ItemModel item) {
        listModel.addItemAt(item);
    }
    public void insertAt(ItemModel item, int index) {
        listModel.addItemAt(item, index);
    }
    public void removeAt(int index) {
        listModel.removeAt(index);
    }
    public int getSize() {
        return listModel.getSize();
    }


    // returns null if index is illegal
    public ItemModel getItemAt(int index) {
        return listModel.getItemAt(index);
    }
}
