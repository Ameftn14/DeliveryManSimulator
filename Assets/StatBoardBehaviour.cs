using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatBoardBehaviour : MonoBehaviour {
    public static StatBoardBehaviour instance = null;
    public static StatBoardBehaviour Instance => instance;
    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }
    void OnDstroy() {
        if (instance == this) {
            instance = null;
        }
    }

    public void AppendStatEntry(string name, string value) {
        StatEntryBehaviour.AppendStatEntry(gameObject, name, value);
    }
}
