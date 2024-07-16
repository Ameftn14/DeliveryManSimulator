using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListModel : MonoBehaviour {
    public List<ItemModel> items = new List<ItemModel>();
    public int getSize() {
        return items.Count;
    }

    public void drop(int dropId, int targetId) {
        if (dropId == targetId) return;
        else if (dropId < targetId) {
            swap(dropId, dropId + 1);
            drop(dropId + 1, targetId);
        } else {
            swap(dropId, dropId - 1);
            drop(dropId - 1, targetId);
        }
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
        //Debug.Log("Base Model Adding item at " + index);
        preInsertActions?.Invoke(index);

        items.Insert(index, itemModel);
        itemModel.bindTo(this);
        itemModel.setSiblingIndex(index);
    }

    public delegate void listUpdateAction(int deletedIndex);
    public event listUpdateAction postDeleteActions;
    public event listUpdateAction preInsertActions;

    public void removeAt(int index) {
        if (items.Count <= 0 || index >= items.Count) return;
        postDeleteActions?.Invoke(index);
        items.RemoveAt(index);
        // force refresh the vertical layout
        GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }
    public ItemModel getItemAt(int index) {
        if (index < 0 || index >= items.Count) return null;
        return items[index];
    }
}
