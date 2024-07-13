using UnityEngine;

public class OrderMenuView : MonoBehaviour {
    public OrderMenuListBehaviour listModel;
    void Start() {
        Debug.Assert(listModel != null);
    }
    public void appendItem(ItemModel item) {
        Debug.Log("order menu view: appendItem");
        listModel.addItemAt(item, listModel.getSize());
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
