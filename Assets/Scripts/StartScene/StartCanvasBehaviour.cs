using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCanvasBehaviour : MonoBehaviour
{
    public int state = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ClickStart()
    {
        if (state != 0)
            return;
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ClickExit()
    {
        if (state != 0)
            return;
        Application.Quit();
    }

    public void ClickHelp() {
        if (state != 0)
            return;
        GameObject.Find("Canvas").transform.Find("T1").gameObject.SetActive(true);
        state = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != 0) {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                GameObject.Find("Canvas").transform.Find("T" + state).gameObject.SetActive(false);
                state++;
                Transform nextHelp = GameObject.Find("Canvas").transform.Find("T" + state);
                if (nextHelp != null) {
                    nextHelp.gameObject.SetActive(true);
                } else {
                    state = 0;
                }
            } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                GameObject.Find("Canvas").transform.Find("T" + state).gameObject.SetActive(false);
                state--;
                Transform nextHelp = GameObject.Find("Canvas").transform.Find("T" + state);
                if (nextHelp != null) {
                    nextHelp.gameObject.SetActive(true);
                } else {
                    state = 0;
                }
            } else if (Input.GetKeyDown(KeyCode.Escape)) {
                GameObject.Find("Canvas").transform.Find("T" + state).gameObject.SetActive(false);
                state = 0;
            }
        }
    }
}
