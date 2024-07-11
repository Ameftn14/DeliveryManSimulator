using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListModel : MonoBehaviour {
    public List<ItemModel> items = new List<ItemModel>();
    public int getSize() {
        return items.Count;
    }
    public void swap(int id1, int id2) {
        // swap them as a gameobject
        items[id1].setSiblingIndex(id2);
        items[id2].setSiblingIndex(id1);

        // swap them in a list
        ItemModel temp = items[id1];
        items[id1] = items[id2];
        items[id2] = temp;
    }


    public void addItemAt(ItemModel itemModel, int index = 0) {
        if (index > items.Count)
            index = items.Count;
        else if (index < 0) {
            index = 0;
        }
        preInsertActions?.Invoke(index);

        items.Insert(index, itemModel);
        itemModel.bindTo(this);
        itemModel.setSiblingIndex(index);
    }
    public void appendItem(ItemModel itemModel) {
        items.Add(itemModel);
        itemModel.bindTo(this);
    }

    public delegate void listUpdateAction(int deletedIndex);
    public event listUpdateAction postDeleteActions;
    public event listUpdateAction preInsertActions;

    public void removeAt(int index) {
        // remove the first one
        if (items.Count <= 0 || index >= items.Count) return;
        postDeleteActions?.Invoke(index);
        items.RemoveAt(index);
    }
    public ItemModel getItemAt(int index) {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }
}
