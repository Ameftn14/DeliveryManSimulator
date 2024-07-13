using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Add this line to import the UnityEngine.UI namespace

public class OrderItemBehaviour : ItemModel, IPointerEnterHandler, IPointerExitHandler {
    private OrderInfo orderInfo;
    public GameObject imageObject;
    public TMP_Text dueTimeText;

    public Color defaultColor = new Color(0, 0, 0, 128);
    public MenuItemTimeLeftBarController timeLeftBarController;
    private void setOrderInfo(OrderInfo orderInfo) {
        this.orderInfo = orderInfo;
        syncDisplay();
    }
    public OrderInfo getOrderInfo() {
        return orderInfo;
    }
    public void syncDisplay() {
        Color colorToSet;
        // colorToSet = defaultColor;
        colorToSet = orderInfo.color;
        imageObject.GetComponent<Image>().color = colorToSet;
        Debug.Assert(dueTimeText != null);
        DateTime d = new DateTime(orderInfo.dueTime.Ticks);
        // string dueTimeTextString = orderInfo.dueTime.ToString();
        string dueTimeTextString = d.ToString("HH:mm"); ;
        dueTimeText.text = dueTimeTextString;
    }
    void Start() {
        Debug.Assert(dueTimeText != null);
        Debug.Assert(imageObject != null);
        timeLeftBarController = GetComponentInChildren<MenuItemTimeLeftBarController>();
        Debug.Assert(timeLeftBarController != null);
        timeLeftBarController.setDueTime(orderInfo.dueTime);
        init(); // this is from the super class
    }
    /* -------------------------------------------------------------------------- */
    /*                           Static Spawning Methods                          */
    /* -------------------------------------------------------------------------- */
    public static ItemModel spawnNewRestaurantOrderItem(OrderInfo orderInfo) {
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Menu Item/Version 2/Restaurant Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        orderItemBehaviour.name = "Restaurant Order Item " + orderInfo.orderID;
        return orderItemBehaviour;
    }
    public static ItemModel spawnNewCustomerOrderItem(OrderInfo orderInfo) {
        //return spawnNewRestaurantOrderItem(orderInfo);

        // original code
        OrderItemBehaviour orderItemBehaviour = (OrderItemBehaviour)spawnNewItem("Prefabs/UI/Menu Item/Version 2/Customer Menu Item");
        orderItemBehaviour.setOrderInfo(orderInfo);
        orderItemBehaviour.name = "Customer Order Item " + orderInfo.orderID;
        orderItemBehaviour.gameObject.name = "Customer Order Item " + orderInfo.orderID;
        return orderItemBehaviour;
    }

    /* -------------------------------------------------------------------------- */
    /*                         Interaction with the Mouse                         */
    /* -------------------------------------------------------------------------- */
    // TODO clean up the debug prints
    [SerializeField] bool dragDamping = false;
    [SerializeField] bool followingAlong = false;
    [SerializeField] Vector2 offset;
    // when the mouse hovers over the order item:
    // 1. highlight the order item, both in the menu and in the game world
    // 2. show more info about the order? so we should make this expandable
    // 3. if player is dragging another item, stop the dragging if that order is the restaurant order of this one
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Mouse Enter " + name);
        OrderMenuListBehaviour.Instance.setMouseHoverItem(this);
    }
    override public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log("Begin Drag " + name);
        base.OnBeginDrag(eventData);
        OrderMenuListBehaviour.Instance.setMouseDragItem(this);
    }
    override public void OnDrag(PointerEventData eventData) {
        Debug.Log("Dragging " + name);
        if (!dragDamping) {
            base.OnDrag(eventData);
        } else {
            rectTransform.anchoredPosition += dampingFactor * eventData.delta;
            OrderMenuListBehaviour.Instance.setSharedEventData(eventData);
        }
    }
    override public void OnEndDrag(PointerEventData eventData) {
        Debug.Log("End Drag " + name);
        base.OnEndDrag(eventData);
        OrderMenuListBehaviour.Instance.setMouseDragItem(null);
    }
    public override void OnDrop(PointerEventData eventData) {
        if (OrderMenuListBehaviour.Instance.isInBlockingMode())
            return;
        int droppedIndex = eventData.pointerDrag.GetComponent<ItemModel>().getIndex();
        if (OrderMenuListBehaviour.Instance.swapIsAllowed(droppedIndex, index)) {
            Debug.Log("DROPPED " + droppedIndex + " ON " + index + "SWAP ALLOWED");
            OrderMenuListBehaviour.Instance.swap(droppedIndex, index);
        }
    }

    float dampingFactor = 0.2f; // smaller the stronger
    public void blockDragging() {
        dragDamping = true;
    }
    public void unblockDragging() {
        Debug.Log("Unblock dragging");
        dragDamping = false;
    }

    public void startFollowingAlong() {
        if (followingAlong) {
            return;
        }
        originalPosition = rectTransform.anchoredPosition;
        rectTransform.anchoredPosition = originalPosition + offset;
        followingAlong = true;
    }
    public void stopFollowingAlong() {
        if (!followingAlong) {
            return;
        }
        rectTransform.anchoredPosition = originalPosition;
        followingAlong = false;
    }
    void Update() {
        if (followingAlong) {
            PointerEventData eventData = OrderMenuListBehaviour.Instance.getSharedEventData();
            rectTransform.anchoredPosition += eventData.delta * dampingFactor * 1.1f;
        }
    }
    // when the mouse leaves the order item:
    // 1. unhighlight the order item
    // 2. hide the extra info
    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Mouse Exit " + name);
        OrderMenuListBehaviour.Instance.setMouseHoverItem(null);
    }
}