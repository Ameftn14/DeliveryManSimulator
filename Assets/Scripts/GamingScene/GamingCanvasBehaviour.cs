using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamingCanvasBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 检测空格键是否被按下
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Destroy(instance);
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("Settlement");
        }
    }
}
