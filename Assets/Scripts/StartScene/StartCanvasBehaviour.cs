using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCanvasBehaviour : MonoBehaviour
{
    private int state = 0;
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
        if (state == 1 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) {
            GameObject.Find("Canvas").transform.Find("T1").gameObject.SetActive(false);
            GameObject.Find("Canvas").transform.Find("T2").gameObject.SetActive(true);
            state = 2;
        } else if (state == 2 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))) {
            GameObject.Find("Canvas").transform.Find("T2").gameObject.SetActive(false);
            state = 0;
        }
    }
}
