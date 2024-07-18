using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardSceneControler : MonoBehaviour {
    // Start is called before the first frame update
    // Update is called once per frame
    public void returnToHomeScreen() {
        DeliverymanManager.Instance.Reset();
        SceneManager.LoadSceneAsync("Start");
    }
}
