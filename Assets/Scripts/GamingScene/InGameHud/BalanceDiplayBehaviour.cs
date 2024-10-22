using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalanceDiplayBehaviour : MonoBehaviour {
    [SerializeField] private int balance;
    public TMP_Text textDisplay;

    public void setBalance(int balance) {
        this.balance = balance;
    }

    void Start() {
        Debug.Assert(textDisplay != null);
    }

    void Update() {
        textDisplay.text = balance.ToString();
    }
}
