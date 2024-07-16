using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TutorialManagerBehaviour : MonoBehaviour {
    public static bool addlist = false;
    public static bool sortlist = false;
    public static bool speedup = false;
    public static bool timeslow = false;

    public static void AddList() {
        addlist = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Tutorial:Accept", "Click icon on the map", 5);
    }

    public static void SortList() {
        sortlist = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Tutorial:Sort", "Click icon to go there, or drag to sort in the list", 5);
    }

    public static void SpeedUp() {
        speedup = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Tutorial:Speed Up", "Hold LShift to speed up", 5);
    }

    public static void TimeSlow() {
        timeslow = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Tutorial:Time Slow", "Hold LCtrl to slow down time, and then choose wisely", 5);
    }

    public static void Skip() {
        AlertBoxBehaviour.ShowAlertAtMiddle("Press Space", "Go to the next level", 20);
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
}
