using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HomescreenButtomBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    // Start is called before the first frame update
    public float scaleFactor = 1.2f;
    public float lerpSpeed = 28f;
    private float targetScale = 1.0f;
    void Update() {
        float scale = transform.localScale.x;
        scale = Mathf.Lerp(scale, targetScale, Time.deltaTime * lerpSpeed);
        // 应用缩放
        transform.localScale = Vector3.one * scale;
    }
    public void OnPointerEnter(PointerEventData eventData) {
        targetScale = scaleFactor;
    }
    public void OnPointerExit(PointerEventData eventData) {
        targetScale = 1.0f;
    }
}
