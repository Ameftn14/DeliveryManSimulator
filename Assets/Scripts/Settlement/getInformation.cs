using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class getInformation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("speed is" + DeliverymanManager.speed + "and money is" + DeliverymanManager.money);
        }
        if(Input.GetKeyDown(KeyCode.O)){
            DeliverymanManager.money += 10;
            Debug.Log("Moneyup! Now money is" + DeliverymanManager.money);
        }

        if(Input.GetKeyDown(KeyCode.Space)){
            // 加载指定的场景
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
        
    }
}
