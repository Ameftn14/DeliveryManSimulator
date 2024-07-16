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
        GameObject Text = GameObject.Find("TmpTutorialText");
        Debug.Assert(Text != null);
        Text.GetComponent<TMP_Text>().text = "Click icon on the map to accept order";
    }

    public static void SortList() {
        sortlist = true;
        GameObject Text = GameObject.Find("TmpTutorialText");
        Debug.Assert(Text != null);
        Text.GetComponent<TMP_Text>().text = "Click the icon on the map to go somewhere first\n Or drag item in the list to sort";
    }

    public static void SpeedUp() {
        speedup = true;
        GameObject Text = GameObject.Find("TmpTutorialText");
        Debug.Assert(Text != null);
        Text.GetComponent<TMP_Text>().text = "You are late! hold LShift to use SpeedUp skill!";
    }

    public static void TimeSlow() {
        timeslow = true;
        GameObject Text = GameObject.Find("TmpTutorialText");
        Debug.Assert(Text != null);
        Text.GetComponent<TMP_Text>().text = "Hold LCtrl to slowdown time and choose carefully!";
    }

    public static void Skip() {
        GameObject Text = GameObject.Find("TmpTutorialText");
        Debug.Assert(Text != null);
        Text.GetComponent<TMP_Text>().text = "Press Space to Go to Next Level";
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }
}
