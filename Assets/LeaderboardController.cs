using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class LeaderboardController : MonoBehaviour {
    public GameObject blurPanel;
    public GameObject leaderboardPanel;
    public GameObject setusernamePanel;
    void Start() {
        Debug.Assert(blurPanel != null);
        Debug.Assert(leaderboardPanel != null);
        Debug.Assert(setusernamePanel != null);
        model = DeliverymanManager.Instance;
        if (model.currentUsername == null) {
            showUsernameSetting();
        } else {
            showLeaderboard();
            getNewLeaderboard();
        }
    }
    DeliverymanManager model;

    /* -------------------------------------------------------------------------- */
    /*                             visibility control                             */
    /* -------------------------------------------------------------------------- */
    public void showLeaderboard() {
        blurPanel.SetActive(false);
        leaderboardPanel.SetActive(true);
        setusernamePanel.SetActive(false);
    }
    void showUsernameSetting() {
        blurPanel.SetActive(true);
        // leaderboardPanel.SetActive(true);
        setusernamePanel.SetActive(true);
    }


    /* -------------------------------------------------------------------------- */
    /*                             Respond to refresh                             */
    /* -------------------------------------------------------------------------- */
    public void onRefreshButtonHit() {
        getNewLeaderboard();
    }
    void refreshLeaderboard(GetLeaderboardResult result) {
        Debug.Log("refreshing leaderboard display");
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        foreach (PlayerLeaderboardEntry entry in result.Leaderboard) {
            entries.Add(new LeaderboardEntry(entry.DisplayName, entry.StatValue.ToString(), entry.Position + 1));
        }
        LeaderboardBehaviour leaderboardBehaviour = leaderboardPanel.GetComponent<LeaderboardBehaviour>();
        leaderboardBehaviour.setLeaderboard(entries);
    }

    void getNewLeaderboard() {
        if (model.currentUsername == null) {
            showUsernameSetting();
            return;
        }
        Debug.Log("Getting new leaderboard");
        var request = new GetLeaderboardRequest {
            StartPosition = 0,
            StatisticName = "Test", //TODO change this to the correct statistic name
            MaxResultsCount = 10 // TODO what should this be?
        };
        PlayFab.PlayFabClientAPI.GetLeaderboard(request, refreshLeaderboard, (error) => {
            Debug.Log("Leaderboard Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    /* -------------------------------------------------------------------------- */
    /*                         respond to username setting                        */
    /* -------------------------------------------------------------------------- */
    public void onUsernameChangeButtonHit() {
        model.currentUsername = null;
        showUsernameSetting();
    }
    public void onUsernameSet(string username) {
        model.currentUsername = username;
        showLeaderboard();
        getNewLeaderboard();
    }
    private int score = 0;
    public void uploadScore() {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Test", //TODO change this to the correct statistic name
                    Value = score
                }
            }
        };
    }
    /* -------------------------------------------------------------------------- */
    /*      理论上说外界只需要在适当时候创建这个类，然后调用这个方法就好                */
    /* -------------------------------------------------------------------------- */
    public void submitNewScore(int score) {
        this.score = score;
        if (model.currentUsername == null) {
            showUsernameSetting();
            return;
        }
        uploadScore();
    }
}
