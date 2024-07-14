using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarBehaviour : MonoBehaviour {
    [SerializeField] private ProgressBarBehaviour timeBar;
    [SerializeField] private PeakHourFlameBehaviour noonPeakFlame;
    [SerializeField] private PeakHourFlameBehaviour afternoonPeakFlame;

    int dayStartMinute;
    int dayEndMinute;
    int noonPeakMinute;
    int noonPeakEndMinute;
    int afternoonPeakMinute;
    int afternoonPeakEndMinute;
    int totalMinute;

    void Start() {
        Debug.Assert(timeBar != null);

        dayStartMinute = TimeToMinute(VirtualClockUI.Instance.GetDayStart());
        dayEndMinute = TimeToMinute(VirtualClockUI.Instance.GetDayEnd());
        totalMinute = dayEndMinute - dayStartMinute;

        noonPeakMinute = TimeToMinute(VirtualClockUI.Instance.GetLunchStart());
        noonPeakEndMinute = TimeToMinute(VirtualClockUI.Instance.GetLunchEnd());

        afternoonPeakMinute = TimeToMinute(VirtualClockUI.Instance.GetDinnerStart());
        afternoonPeakEndMinute = TimeToMinute(VirtualClockUI.Instance.GetDinnerEnd());

        setUpPeakFlameIcons();
    }
    int avgPeakMinute(int start, int end) {
        return (start + end) / 2;
    }
    void setUpPeakFlameIcons() {
        noonPeakFlame.initLocation((float)(avgPeakMinute(noonPeakMinute, noonPeakEndMinute) - dayStartMinute) / totalMinute);
        afternoonPeakFlame.initLocation((float)(avgPeakMinute(afternoonPeakEndMinute, afternoonPeakMinute) - dayStartMinute) / totalMinute);
    }
    int TimeToMinute(TimeSpan timeSpan) {
        return timeSpan.Hours * 60 + timeSpan.Minutes;
    }
    // Update is called once per frame
    void Update() {
        var currentTimeStamp = TimeToMinute(VirtualClockUI.Instance.GetTime());
        float progress = 0;
        if (currentTimeStamp < dayStartMinute) {
            progress = 0;
        } else if (currentTimeStamp <= dayEndMinute) {
            progress = (float)(currentTimeStamp - dayStartMinute) / totalMinute;
        } else {
            progress = 1;
        }
        timeBar.setPercentage(progress);
        // TODO check the peak time and giggle the flame icon
        noonPeakFlame.setGiggle(isDuringLaunchPeak(currentTimeStamp));
        afternoonPeakFlame.setGiggle(isDuringDinnerPeak(currentTimeStamp));
    }



    bool isDuringLaunchPeak(int currentTimeStamp) {
        return currentTimeStamp >= noonPeakMinute && currentTimeStamp <= noonPeakEndMinute;
    }
    bool isDuringDinnerPeak(int currentTimeStamp) {
        return currentTimeStamp >= afternoonPeakMinute && currentTimeStamp <= afternoonPeakEndMinute;
    }
}
