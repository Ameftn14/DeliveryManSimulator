using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //Destroy(instance);
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
}
