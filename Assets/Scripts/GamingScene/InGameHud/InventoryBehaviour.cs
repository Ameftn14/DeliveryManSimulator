using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBehaviour : MonoBehaviour {
    [SerializeField] private EmptyInventorySlotBehaviour[] emptySlots;
    [SerializeField] private FilledInventorySlotBehaviour[] filledSlots;

    [SerializeField] private GameObject panelForEmpty;
    [SerializeField] private GameObject panelForFilled;
    [SerializeField] private int occupiedCnt;
    [SerializeField] private int capacity;
    void Start() {
        Debug.Assert(panelForEmpty != null);
        Debug.Assert(panelForFilled != null);
        emptySlots = panelForEmpty.GetComponentsInChildren<EmptyInventorySlotBehaviour>();
        filledSlots = panelForFilled.GetComponentsInChildren<FilledInventorySlotBehaviour>();
        syncDisplay();
    }

    void syncDisplay() {
        int i = 0;
        foreach (var slot in emptySlots) {
            slot.setUnlocked(i < capacity);
            i++;
        }
        i = 0;
        foreach (var slot in filledSlots) {
            slot.setOccupied(i < occupiedCnt);
            i++;
        }
    }

    public Vector2 getNextNewSlotPosition() {
        return emptySlots[capacity].getPosition();
    }
    public Vector2 getNextEmtpySlotPosition() {
        return emptySlots[occupiedCnt].getPosition();
    }
    public void addNewOrders(int numberOfOrders) {
        capacity += numberOfOrders;
        syncDisplay();
    }
    public void removeOrders(int numberOfOrders) {
        capacity -= numberOfOrders;
        syncDisplay();
    }

    // Update is called once per frame
    void Update() {
        syncDisplay();
    }
}
