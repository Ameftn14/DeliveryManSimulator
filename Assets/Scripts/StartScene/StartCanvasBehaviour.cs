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
        SceneManager.LoadScene("SampleScene");
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
