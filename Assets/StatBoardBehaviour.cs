using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StatBoardBehaviour : MonoBehaviour {
    public StatBoardBehaviour instance = null;
    public StatBoardBehaviour Instance {
        get {
            if (instance == null) {
                instance = this;
            }
            return instance;
        }
    }
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
