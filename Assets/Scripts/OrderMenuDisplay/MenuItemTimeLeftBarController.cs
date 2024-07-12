using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItemTimeLeftBarController : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private Image barImage;
    [SerializeField] private float percentage;

    [SerializeField] private float totalWidth;
    [SerializeField] private float componentWidth;
    [SerializeField] private TimeSpan dueTime;
    [SerializeField] long totalMinutes;
    int getTotalMinutes(TimeSpan timeSpan) {
        return timeSpan.Hours * 60 + timeSpan.Minutes;
    }
    public void setDueTime(TimeSpan timeSpan) {
        dueTime = timeSpan;
        TimeSpan now = VirtualClockUI.Instance.GetTime();
        totalMinutes = getTotalMinutes(dueTime - now);
        if (totalMinutes <= 0) {
            totalMinutes = 1;
        }
    }
    float getPercentage() {
        TimeSpan now = VirtualClockUI.Instance.GetTime();
        int minutesLeft = getTotalMinutes(dueTime - now);
        percentage = (float)minutesLeft / totalMinutes;
        return percentage;
    }


    void Start() {
        Debug.Assert(barImage != null);

    }

    // Update is called once per frame
    void Update() {
        totalWidth = GetComponent<RectTransform>().sizeDelta.x;
        componentWidth = totalWidth * getPercentage();
        // Use componentWidth to update the length of the component 
        barImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, componentWidth);
    }
}
