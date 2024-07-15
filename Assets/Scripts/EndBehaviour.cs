using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class EndBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            DeliverymanManager.Instance.Reset();
            SceneManager.LoadSceneAsync("SampleScene");
        }
    }
}
