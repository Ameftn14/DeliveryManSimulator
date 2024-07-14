// using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private ProgressBarBehaviour[] bars;
    public float fillAmount;

    void Start() {
        bars = GetComponentsInChildren<ProgressBarBehaviour>();
    }
    void syncDisplay() {
        float percentage = fillAmount;
        for (int i = 0; i < bars.Length; i++) {
            if (percentage > 1) {
                bars[i].setPercentage(1);
                percentage -= 1;
            } else {
                bars[i].setPercentage(percentage);
                percentage = 0;
            }
        }
    }
    public void setPercentage(float percentage) {
        fillAmount = percentage;
        syncDisplay();
    }

    void Update() {
        syncDisplay();
    }
}