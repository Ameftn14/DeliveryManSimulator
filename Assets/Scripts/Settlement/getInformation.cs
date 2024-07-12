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
        float speed = Property.speed;
        int money = Property.money;
        if(Input.GetKeyDown(KeyCode.I)){
            Debug.Log("speed is" + speed + "and money is" + money);
        }
        
    }
}
