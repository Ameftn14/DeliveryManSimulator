using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoTextBehaviour : MonoBehaviour
{
    //public Property property;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Text").GetComponent<TMP_Text>().text = "Money: " + Property.money + "\t" + "Speed: " + Property.speed + "\t" + "Capacity: " + Property.nowCapacity;
    }
}
