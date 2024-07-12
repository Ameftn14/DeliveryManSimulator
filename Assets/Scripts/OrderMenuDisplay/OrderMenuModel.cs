using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OrderMenuListBehaviour : ListModel {
    /* -------------------------------------------------------------------------- */
    /*                          // for singleton pattern                          */
    /* -------------------------------------------------------------------------- */
    private OrderMenuListBehaviour instance;
    public OrderMenuListBehaviour Instance {
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

    [SerializeField] private int? draggingItemIndex = null;
    [SerializeField] private int? hoveringItemIndex = null;


}
