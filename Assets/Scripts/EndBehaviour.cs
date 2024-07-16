using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.SocialPlatforms.Impl;

public class EndBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        string score;
        if (DeliverymanManager.money < 2000) {
            score = "F";
        } else if (DeliverymanManager.money < 3000) {
            score = "D";
        } else if (DeliverymanManager.money < 4000) {
            if (DeliverymanManager.latecount > 5 && DeliverymanManager.badcount > 0) {
                score = "D";
            } else {
                score = "C";
            }
        } else if (DeliverymanManager.money < 5000) {
            if (DeliverymanManager.latecount > 5 && DeliverymanManager.badcount > 0) {
                score = "C";
            } else {
                score = "B";
            }
        } else if (DeliverymanManager.money < 6000) {
            if (DeliverymanManager.latecount > 5 && DeliverymanManager.badcount > 0) {
                score = "B";
            } else {
                score = "A";
            }
        } else {
            if (DeliverymanManager.badcount > 5) {
                score = "B";
            } else if (DeliverymanManager.latecount > 5) {
                score = "A";
            } else if (DeliverymanManager.badcount > 0) {
                score = "S";
            } else if (DeliverymanManager.latecount > 0) {
                score = "SS";
            } else {
                score = "SSS";
            }
        }
        StatBoardBehaviour.Instance.AppendStatEntry("You Have Earned: ", "$" + DeliverymanManager.money.ToString());
        StatBoardBehaviour.Instance.AppendStatEntry("Finished Order: ", DeliverymanManager.finishedcount.ToString());
        StatBoardBehaviour.Instance.AppendStatEntry("Late Order: ", DeliverymanManager.latecount.ToString());
        StatBoardBehaviour.Instance.AppendStatEntry("Bad Order: ", DeliverymanManager.badcount.ToString());
        StatBoardBehaviour.Instance.AppendStatEntry("Score: ", score);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        } else if (Input.GetKeyDown(KeyCode.R)) {
            DeliverymanManager.Instance.Reset();
            SceneManager.LoadSceneAsync("SampleScene");
        }
    }
}
