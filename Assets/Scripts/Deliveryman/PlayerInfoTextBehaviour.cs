using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoTextBehaviour : MonoBehaviour
{
    public Property property;
    // Start is called before the first frame update
    void Start()
    {
        property = GameObject.Find("Deliveryman").GetComponent<Property>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject.Find("Text").GetComponent<TMP_Text>().text = "Money: " + property.money + "\t" + "Speed: " + property.speed + "\t" + "Capacity: " + property.nowCapacity;
    }
}
