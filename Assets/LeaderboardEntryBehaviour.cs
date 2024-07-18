using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntryBehaviour : MonoBehaviour {
    public TMP_Text username;
    public TMP_Text statValue;
    public TMP_Text statRank;
    void Start() {
        Debug.Assert(username != null);
        Debug.Assert(statValue != null);
    }

    public static void AppendStatEntry(GameObject parent, string name, string value, int rank) {
        // load prefab from Resources/Prefabs/UI/StatEntry
        GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/Leaderboard Entry");
        GameObject instance = Instantiate(prefab, parent.transform);
        LeaderboardEntryBehaviour leaderboardEntry = instance.GetComponent<LeaderboardEntryBehaviour>();
        leaderboardEntry.username.text = name;
        leaderboardEntry.statValue.text = value;
        leaderboardEntry.statRank.text = rank.ToString();
    }
}

