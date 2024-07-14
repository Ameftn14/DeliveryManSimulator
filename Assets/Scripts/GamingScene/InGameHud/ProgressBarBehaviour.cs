using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarBehaviour : MonoBehaviour {
    public GameObject bar;
    public float fillAmount;
    private CanvasGroup canvasGroup;
    [SerializeField] private bool diplayed = true;
    [SerializeField] private float lerpSpeed = 30;
    [SerializeField] private float selfWidth;
    [SerializeField] private float barWidth;
    private RectTransform selfRectTransform;
    private RectTransform barRectTransform;
    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
        selfRectTransform = GetComponent<RectTransform>();
        barRectTransform = bar.GetComponent<RectTransform>();
        selfWidth = selfRectTransform.rect.width;
        barWidth = bar.GetComponent<RectTransform>().rect.width;
        lerpSpeed = lerpSpeed <= 0 ? 10 : lerpSpeed;
    }
    float getBarWidth() {
        return selfRectTransform.rect.width * fillAmount;
    }
    // Update is called once per frame
    public void setPercentage(float percentage) {
        fillAmount = percentage;
        if (fillAmount > 1) {
            fillAmount = 1;
        }
        if (fillAmount < 0) {
            fillAmount = 0;
        }
    }
    [SerializeField] private float disabledAlpha = 0;

    void Update() {
        if (!diplayed) {
            // set the transparency of itself and all its children to 0
            canvasGroup.alpha = disabledAlpha;
            return;
        }
        float newBarWidth = getBarWidth();
        if (barWidth != newBarWidth) {
            barWidth = Mathf.Lerp(barWidth, newBarWidth, lerpSpeed * Time.deltaTime);
            barRectTransform.sizeDelta = new Vector2(barWidth, barRectTransform.rect.height);
        }
    }
}
