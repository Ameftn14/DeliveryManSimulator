using System;
using System.Collections;
using System.Collections.Generic;
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
    public bool isNotInBlockingMode() {
        return !isInSpecialMode;
    }
    void backToNormal() {
        if (hoveringItem != null) {
            hoveringItem.stopFollowingAlong();
        }
        if (draggingItem != null) {
            draggingItem.unblockDragging();
        }
    }
    void checkBlockingCondition() {
        if (hoveringItem == null || draggingItem == null) {
            isInSpecialMode = false;
            backToNormal();
            return;
        }
        OrderInfo hoveringOrder = hoveringItem.getOrderInfo();
        OrderInfo draggingOrder = draggingItem.getOrderInfo();
        if (hoveringOrder.orderID != draggingOrder.orderID || hoveringOrder.locationType == draggingOrder.locationType) {
            isInSpecialMode = false;
            backToNormal();
            return;
        }
        isInSpecialMode = true;
        if (draggingOrder.locationType == LocationType.Restaurant) {
            draggingItem.blockDragging();
            hoveringItem.startFollowingAlong();
        } else {
            draggingItem.blockDragging();
            hoveringItem.startFollowingAlong();
        }
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
        OnMouseHoverOrderChanged?.Invoke(item);
        checkBlockingCondition();
    }
    public void setMouseDragItem(OrderItemBehaviour item) {
        draggingItem = item;
        OnMouseDragOrderChanged?.Invoke(item);
        checkBlockingCondition();
    }

    public delegate void MouseHoverOrderChangedHandler(OrderItemBehaviour orderInfo);
    public event MouseHoverOrderChangedHandler OnMouseHoverOrderChanged;
    public delegate void MouseDragOrderChangedHandler(OrderItemBehaviour orderInfo);
    public event MouseDragOrderChangedHandler OnMouseDragOrderChanged;




}
