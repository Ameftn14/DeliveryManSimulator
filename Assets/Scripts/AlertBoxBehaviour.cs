using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class AlertBoxBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    public TMP_Text title;
    public TMP_Text content;

    public float secondsToLive;
    public CanvasGroup canvasGroup;

    void Start() {
        Debug.Assert(title != null);
        Debug.Assert(content != null);
        Debug.Assert(secondsToLive > 0);
    }

    bool isFadingOut = false;
    public float fadeOutTime = 0.3f;
    // Update is called once per frame
    float alpha = 1;
    void Update() {
        if (isFadingOut) {
            alpha -= Time.smoothDeltaTime / fadeOutTime;
            canvasGroup.alpha = alpha;
            if (alpha <= 0) {
                decreaseOffset();
                Destroy(gameObject);
            }
        } else {
            secondsToLive -= Time.smoothDeltaTime;
            if (secondsToLive <= 0) {
                isFadingOut = true;
            }
        }
    }

    void setFontSize(int fontSize) {
        title.fontSize = fontSize;
        content.fontSize = 0.7f * fontSize;
    }


    public static void ShowAlertAtMiddle(string title, string content, float secondsToLive) {
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
    public static Vector2 bottomRightAlertOffset = new Vector2(-10, 10);
    public int x = 0;
    public int y = 0;
    void increaseOffset() {
        Vector2 selfSize = GetComponent<RectTransform>().rect.size;
        bottomRightAlertOffset.y += selfSize.y;
        x = (int)bottomRightAlertOffset.x;
        y = (int)bottomRightAlertOffset.y;
    }
    void decreaseOffset() {
        Vector2 selfSize = GetComponent<RectTransform>().rect.size;
        bottomRightAlertOffset.y -= selfSize.y;
        x = (int)bottomRightAlertOffset.x;
        y = (int)bottomRightAlertOffset.y;
    }
    public static void ShowAlertAtBottomRight(string title, string content, float secondsToLive) {
        GameObject alertBox = Instantiate(Resources.Load("Prefabs/UI/AlertBox")) as GameObject;
        AlertBoxBehaviour alertBoxBehaviour = alertBox.GetComponent<AlertBoxBehaviour>();
        alertBoxBehaviour.title.text = title;
        alertBoxBehaviour.content.text = content;
        alertBoxBehaviour.secondsToLive = secondsToLive;
        // alertBoxBehaviour.setFontSize(20);
        GameObject parent = GameObject.Find("Canvas");
        alertBox.transform.SetParent(parent.transform, false);
        // move the box to the bottom right of the screen
        RectTransform rectTransform = alertBox.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 0);
        rectTransform.anchorMax = new Vector2(1, 0);
        rectTransform.pivot = new Vector2(1, 0);
        rectTransform.anchoredPosition = new Vector2(-10, 50); // 右下角，向左和向上各有10单位的边距

        alertBoxBehaviour.increaseOffset();
        Debug.Log("bottomRightAlertOffset: " + bottomRightAlertOffset);
        Debug.Log("size: " + alertBox.GetComponent<RectTransform>().sizeDelta);
    }

    public static void ShowAlertAtBottomLeft(string title, string content, float secondsToLive) {
        GameObject alertBox = Instantiate(Resources.Load("Prefabs/UI/AlertBox")) as GameObject;
        alertBox.GetComponent<AlertBoxBehaviour>().title.text = title;
        alertBox.GetComponent<AlertBoxBehaviour>().content.text = content;
        alertBox.GetComponent<AlertBoxBehaviour>().secondsToLive = secondsToLive;
        GameObject parent = GameObject.Find("Canvas");
        alertBox.transform.SetParent(parent.transform, false);
        // move the box to the bottom left of the screen
        RectTransform rectTransform = alertBox.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 50); // 左下角，向右和向上各有10单位的边距
    }

    public static void ShowAlertAtBottomLeft2(string title, string content, float secondsToLive) {
        GameObject alertBox = Instantiate(Resources.Load("Prefabs/UI/AlertBox")) as GameObject;
        alertBox.GetComponent<AlertBoxBehaviour>().title.text = title;
        alertBox.GetComponent<AlertBoxBehaviour>().content.text = content;
        alertBox.GetComponent<AlertBoxBehaviour>().secondsToLive = secondsToLive;
        GameObject parent = GameObject.Find("Canvas");
        alertBox.transform.SetParent(parent.transform, false);
        // move the box to the bottom left of the screen
        RectTransform rectTransform = alertBox.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 200); // 左下角，向右和向上各有10单位的边距
    }
    public static void ShowAlertAtBottomLeft3(string title, string content, float secondsToLive) {
        GameObject alertBox = Instantiate(Resources.Load("Prefabs/UI/AlertBox")) as GameObject;
        alertBox.GetComponent<AlertBoxBehaviour>().title.text = title;
        alertBox.GetComponent<AlertBoxBehaviour>().content.text = content;
        alertBox.GetComponent<AlertBoxBehaviour>().secondsToLive = secondsToLive;
        GameObject parent = GameObject.Find("Canvas");
        alertBox.transform.SetParent(parent.transform, false);
        // move the box to the bottom left of the screen
        RectTransform rectTransform = alertBox.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(10, 350); // 左下角，向右和向上各有10单位的边距
    }
}
