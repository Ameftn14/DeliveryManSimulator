using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class OrderMenuListBehaviour : ListModel {
    /* -------------------------------------------------------------------------- */
    /*                          for singleton pattern                             */
    /* -------------------------------------------------------------------------- */
    private static OrderMenuListBehaviour instance;
    public static OrderMenuListBehaviour Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<OrderMenuListBehaviour>();
            }
            return instance;
        }
    }
    // base class doesn't have a Start method
    void Start() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }
    void OnDestroy() {
        if (instance == this) {
            instance = null;
        }
    }
    /* -------------------------------------------------------------------------- */
    /*             for the additional dragging and hovering functionality         */
    /* -------------------------------------------------------------------------- */

    [SerializeField] private OrderItemBehaviour draggingItem = null;
    [SerializeField] private OrderItemBehaviour hoveringItem = null;

    public OrderItemBehaviour getMouseHoverItem() {
        return hoveringItem;
    }
    public OrderItemBehaviour getMouseDragItem() {
        return draggingItem;
    }
    public bool isInSpecialMode = false;
    public bool isInBlockingMode() {
        return isInSpecialMode;
    }
    void backToNormal() {
        if (hoveringItem != null) {
            hoveringItem.stopFollowingAlong();
        }
        if (draggingItem != null) {
            draggingItem.unblockDragging();
        }
    }
    void checkBlockingCondition(OrderItemBehaviour droppedItem, OrderItemBehaviour targetItem) {
        if (hoveringItem == null || draggingItem == null) {
            isInSpecialMode = false;
            backToNormal();
            return;
        }
        // OrderInfo hoveringOrder = hoveringItem.getOrderInfo();
        // OrderInfo draggingOrder = draggingItem.getOrderInfo();
        // if (hoveringOrder.orderID != draggingOrder.orderID || hoveringOrder.locationType == draggingOrder.locationType) {
        //     isInSpecialMode = false;
        //     backToNormal();
        //     return;
        // }
        if (dropIsAllowed(draggingItem.getIndex(), hoveringItem.getIndex())) {
            isInSpecialMode = false;
            backToNormal();
            return;
        }
        isInSpecialMode = true;
        draggingItem.blockDragging();
        hoveringItem.startFollowingAlong();
    }
    public PointerEventData sharedEventData;
    public void setSharedEventData(PointerEventData eventData) {
        sharedEventData = eventData;
    }
    public PointerEventData getSharedEventData() {
        return sharedEventData;
    }

    public void setMouseHoverItem(OrderItemBehaviour item) {
        if (isInSpecialMode) return;
        hoveringItem = item;
        if (item != null) OnMouseHoverOrderChanged?.Invoke(item.getOrderInfo());
        else OnMouseHoverOrderChanged?.Invoke(null);
        checkBlockingCondition(targetItem: hoveringItem, droppedItem: draggingItem);
    }
    public void setMouseDragItem(OrderItemBehaviour item) {
        if (item == null) {
            isInSpecialMode = false;
            backToNormal();
        }
        draggingItem = item;
        OnMouseDragOrderChanged?.Invoke(item.getOrderInfo());
        checkBlockingCondition(targetItem: hoveringItem, droppedItem: draggingItem);
    }

    /* -------------------------------------------------------------------------- */
    /*  Unfortunatly, player can bypass the above system, so... here you go:      */
    /* -------------------------------------------------------------------------- */
    public bool dropIsAllowed(int droppedIndex, int targetIndex) {
        OrderItemBehaviour droppedItem = (OrderItemBehaviour)getItemAt(droppedIndex);
        OrderInfo droppedOrder = droppedItem.getOrderInfo();
        if (droppedOrder.locationType == LocationType.Restaurant) {
            for (int i = droppedIndex + 1; i <= targetIndex; i++) {
                OrderItemBehaviour item = (OrderItemBehaviour)getItemAt(i);
                if (item.getOrderInfo().orderID == droppedOrder.orderID &&
                    item.getOrderInfo().locationType == LocationType.Customer) {
                    return false;
                }
            }
            return true;
        } else if (droppedOrder.locationType == LocationType.Customer) {
            for (int i = targetIndex; i < droppedIndex; i++) {
                OrderItemBehaviour item = (OrderItemBehaviour)getItemAt(i);
                if (item.getOrderInfo().orderID == droppedOrder.orderID &&
                    item.getOrderInfo().locationType == LocationType.Restaurant) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
    public bool swapIsAllowed(int indexA, int indexB) {
        // if (indexA == indexB) return false;
        // if (indexA < 0 || indexA >= getSize() || indexB < 0 || indexB >= getSize()) return false;
        return dropIsAllowed(indexA, indexB) && dropIsAllowed(indexB, indexA);
    }

    /* -------------------------------------------------------------------------- */
    /*                 To Solve the Deleting And Inserting Problem                */
    /* -------------------------------------------------------------------------- */

    public new void removeAt(int index) {
        if (getSize() < 0 || index >= getSize()) return;
        // fix this
        Debug.Log("Removing " + items[index].name);
        if (hoveringItem != null && hoveringItem.getIndex() == index) {
            hoveringItem = null;
        }
        if (draggingItem != null && draggingItem.getIndex() == index) {
            draggingItem = null;
        }
        checkBlockingCondition(draggingItem, hoveringItem);
        base.removeAt(index);
    }

    public new void addItemAt(ItemModel itemModel, int index = 0) {
        Debug.Log("Menu List Model: adding item at " + index);
        base.addItemAt(itemModel, index);
    }


    /* -------------------------------------------------------------------------- */
    /*                         To Whoever it may concern:                         */
    /* -------------------------------------------------------------------------- */
    public delegate void MouseHoverOrderChangedHandler(OrderInfo orderInfo);
    public event MouseHoverOrderChangedHandler OnMouseHoverOrderChanged;
    public delegate void MouseDragOrderChangedHandler(OrderInfo orderInfo);
    public event MouseDragOrderChangedHandler OnMouseDragOrderChanged;
}
