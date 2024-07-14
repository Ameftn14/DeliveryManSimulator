
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyInventorySlotBehaviour : MonoBehaviour {
    RectTransform rectTransform;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private Image image;
    public Vector2 position;
    void syncDisplay() {
        if (isUnlocked) {
            image.color = new Color(1, 1, 1, 1);
        } else {
            image.color = new Color(1, 1, 1, 0);
        }
    }
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        position = rectTransform.anchoredPosition;
        syncDisplay();
    }
    public void setOccupied(bool isUnlocked) {
        this.isUnlocked = isUnlocked;
        syncDisplay();
    }
    public void getPosition() {
        position = rectTransform.anchoredPosition;
    }
}
