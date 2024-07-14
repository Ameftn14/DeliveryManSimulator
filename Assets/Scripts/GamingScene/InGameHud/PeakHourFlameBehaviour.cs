using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeakHourFlameBehaviour : MonoBehaviour {
    [SerializeField] private GameObject container;
    [SerializeField] private float percentage;
    [SerializeField] private float parentWidth;
    [SerializeField] private float width;
    [SerializeField] private Animator animator;
    void Start() {
        Debug.Assert(container != null);
    }
    public void initLocation(float percentage) {
        if (percentage > 1) {
            percentage = 1;
        }
        if (percentage < 0) {
            percentage = 0;
        }
        this.percentage = percentage;
        var rectTransform = GetComponent<RectTransform>();
        var parentRectTransform = container.GetComponent<RectTransform>();

        var parentWidth = parentRectTransform.rect.width; this.parentWidth = parentWidth;
        var parentHeight = parentRectTransform.rect.height;

        var width = rectTransform.rect.width; this.width = width;
        var height = rectTransform.rect.height;
        var x = parentWidth * percentage;
        var y = parentHeight / 2;
        rectTransform.anchoredPosition = new Vector2(x, y);
    }
    public void setGiggle(bool isActive) {
        animator.SetBool("giggle", isActive);
    }
    // Update is called once per frame
    void Update() {
        initLocation(percentage);
    }
}
