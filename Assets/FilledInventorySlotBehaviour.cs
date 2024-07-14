using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilledInventorySlotBehaviour : MonoBehaviour {
    [SerializeField] private bool isOccupied = false;
    [SerializeField] private Image image;
    void syncDisplay() {
        if (isOccupied) {
            image.color = new Color(1, 1, 1, 1);
        } else {
            image.color = new Color(1, 1, 1, 0);
        }
    }
    void Start() {
        image = GetComponent<Image>();
        syncDisplay();
    }
    public void setOccupied(bool isOccupied) {
        this.isOccupied = isOccupied;
        syncDisplay();
    }
    void Update() {

    }
}
