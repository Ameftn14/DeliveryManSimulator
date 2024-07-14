using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBarBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] private ProgressBarBehaviour bar1;
    [SerializeField] private ProgressBarBehaviour bar2;
    public float fillAmount;

    void Start() {
        Debug.Assert(bar1 != null);
        Debug.Assert(bar2 != null);
    }
    public void setPercentage(float percentage) {
        fillAmount = percentage;
        if (fillAmount > 2) {
            fillAmount = 2;
        }
        if (fillAmount < 0) {
            fillAmount = 0;
        }
    }

    void Update() {
        if (fillAmount > 1) {
            bar1.setPercentage(1);
            bar2.setPercentage(fillAmount - 1);
        } else {
            bar1.setPercentage(fillAmount);
            bar2.setPercentage(0);
        }
    }
}
