using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRandomMove : MonoBehaviour {
    public Vector3 startPos;
    public Vector3 speedDir;
    public float speed = 1f;
    // Start is called before the first frame update
    void Start() {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        Vector3 accDir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-0.1f, 0.1f));
        accDir = accDir.normalized;
        accDir += (startPos - transform.position) * 0.02f;
        speedDir += 2 * accDir * Time.deltaTime;
        speedDir = speedDir.normalized;
        transform.position += speedDir * speed * Time.deltaTime;
    }
}
