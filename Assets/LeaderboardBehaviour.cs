using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardBehaviour : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject contentPanel;
    void Start() {
        Debug.Assert(contentPanel != null);
    }
    public void setLeaderboard(List<LeaderboardEntry> entries) {
        foreach (Transform child in contentPanel.transform) {
            Destroy(child.gameObject);
        }
        foreach (LeaderboardEntry entry in entries) {
            LeaderboardEntryBehaviour.AppendStatEntry(contentPanel, entry.username, entry.statValue, entry.statRank);
        }

    }
}


public class LeaderboardEntry {
    public string username;
    public string statValue;
    public int statRank;
    public LeaderboardEntry(string username, string statValue, int statRank) {
        this.username = username;
        this.statValue = statValue;
        this.statRank = statRank;
    }
}