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
    public static bool skip = false;
    public static bool assign = false;
    public static bool hot = false;

    public static void AddList() {
        addlist = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Tutorial: Accept", "Click icon on the map", 5);
    }

    public static void SortList() {
        sortlist = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Sort", "Click icon to go there or drag to sort in the list", 5);
    }

    public static void SpeedUp() {
        speedup = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Speed Up", "Hold LShift to speed up", 5);
    }

    public static void TimeSlow() {
        timeslow = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Time Slow", "Hold LCtrl to slow down time and choose", 5);
    }

    public static void Sunny() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Weather: Sunny", "No special changes", 5);
    }

    public static void Rainy() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Weather: Rainy", "Slower speed with High profit per order", 5);
    }

    public static void Foggy() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Weather: Foggy", "Speed reduced. Order-accepting time extended", 5);
    }

    public static void Cloudy() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Weather: Cloudy", "Order-taking time shortened", 5);
    }

    public static void NotAccept() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Full Bag", "Remember to upgrade your bag size", 5);
    }

    public static void AssignedOrder() {
        assign = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Assigned Order", "Not accept orders with \"!\" will result in a penalty", 5);
    }

    public static void HotOrder(){
        hot = true;
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Hot Order", "Short time to accept, but high profit", 5);
    }

    public static void Skip() {
        skip = true;
        AlertBoxBehaviour.ShowAlertAtMiddle("PRESS SPACE", "Go to the next level", 60);
    }

    public static void FromNotPrepared() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Food Not Prepared", "Hit SPACE to urge", 5);

    }

    public static void LateArriveTo() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Arrive Late", "You are punished", 5);
    }

    public static void OnTimeArriveTo() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("On Time Arrive", "You got some tips", 5);
    }

    public static void BuyInfo() {
        AlertBoxBehaviour.ShowAlertAtBottomLeft("Buy Upgrades", "Press Space to skip", 10);
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
}
