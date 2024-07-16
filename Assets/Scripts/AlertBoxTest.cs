using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBoxTest : MonoBehaviour {
    // Start is called before the first frame updatev
    public void ShowAlert() {
        Debug.Log("ShowAlert");
        string title = "Title";
        string content = "Content";
        float secondsToLive = 5;
        AlertBoxBehaviour.ShowAlertAtMiddle(title, content, secondsToLive);
        AlertBoxBehaviour.ShowAlertAtBottomRight(title, content, secondsToLive);
    }
}
