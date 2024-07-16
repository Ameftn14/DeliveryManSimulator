using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertBoxBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    public TMP_Text title;
    public TMP_Text content;

    public float secondsToLive;

    void Start() {
        Debug.Assert(title != null);
        Debug.Assert(content != null);
    }

    // Update is called once per frame
    void Update() {
        secondsToLive -= Time.smoothDeltaTime;
        if (secondsToLive <= 0) {
            Destroy(gameObject);
        }
    }


    public static void ShowAlert(string title, string content, float secondsToLive) {
        GameObject alertBox = Instantiate(Resources.Load("Prefabs/UI/AlertBox")) as GameObject;
        alertBox.GetComponent<AlertBoxBehaviour>().title.text = title;
        alertBox.GetComponent<AlertBoxBehaviour>().content.text = content;
        alertBox.GetComponent<AlertBoxBehaviour>().secondsToLive = secondsToLive;
        GameObject parent = GameObject.Find("Canvas");
        alertBox.transform.SetParent(parent.transform, false);
        // move the box the the center of the screen
        RectTransform rectTransform = alertBox.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);

    }
}
