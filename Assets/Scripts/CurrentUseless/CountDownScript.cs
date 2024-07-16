using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownScript : MonoBehaviour {
    public float duration = 10f; // 倒计时时长
    public float timeLeft;
    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        Debug.Assert(lineRenderer != null);
        timeLeft = duration;
        SetupCircle();
    }

    void Update() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            UpdateCircle((timeLeft / duration) * 360);
        } else {
            Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    void SetupCircle() {
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.loop = true; // 使线条首尾相连
    }

    void UpdateCircle(float angle) {
        int segments = 360;
        int pointCount = (int)(segments * (angle / 360f)) + 1; // 根据剩余时间计算点的数量
        lineRenderer.positionCount = pointCount * 2;
        Vector3[] points = new Vector3[pointCount * 2];
        for (int i = 0; i < pointCount; i++) {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * 1f, Mathf.Cos(rad) * 1f, 0);
            points[pointCount * 2 - i - 1] = new Vector3(Mathf.Sin(rad) * 0.8f, Mathf.Cos(rad) * 0.8f, 0);
        }
        //("pointCount: " + pointCount);
        lineRenderer.SetPositions(points);
    }
}
