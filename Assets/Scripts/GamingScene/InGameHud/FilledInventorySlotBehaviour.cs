using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledInventorySlotBehaviour : MonoBehaviour {
    [SerializeField] private bool isOccupied = false;
    [SerializeField] private Image image;
    void syncDisplay() {
        if (isOccupied) {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
        } else {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }
    void Awake() {
        image = GetComponent<Image>();
        syncDisplay();
    }
    public void setOccupied(bool isOccupied) {
        this.isOccupied = isOccupied;
        syncDisplay();
    }
}
