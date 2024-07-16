using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartCanvasBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ClickStart()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadSceneAsync("SampleScene");
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
