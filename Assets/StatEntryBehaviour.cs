using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatEntryBehaviour : MonoBehaviour {
    public TMP_Text statName;
    public TMP_Text statValue;
    void Start() {
        Debug.Assert(statName != null);
        Debug.Assert(statValue != null);
    }

    public static void AppendStatEntry(GameObject parent, string name, string value) {
        // load prefab from Resources/Prefabs/UI/StatEntry
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/StatEntry");
        GameObject instance = Instantiate(prefab, parent.transform);
        StatEntryBehaviour statEntry = instance.GetComponent<StatEntryBehaviour>();
        statEntry.statName.text = name;
        statEntry.statValue.text = value;
    }
}
