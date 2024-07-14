using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            if (GameObject.Find("DeliverymanManager") != null)
                Destroy(GameObject.Find("DeliverymanManager"));
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
}
