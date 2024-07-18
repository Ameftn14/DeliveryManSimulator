using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class LeaderboardController : MonoBehaviour {
    public GameObject blurPanel;
    public GameObject leaderboardPanel;
    public GameObject setusernamePanel;
    void Awake() {
        Debug.Assert(blurPanel != null);
        Debug.Assert(leaderboardPanel != null);
        Debug.Assert(setusernamePanel != null);
        model = DeliverymanManager.Instance; // TODO uncomment this
        // model = new DeliverymanManager(); // TODO remove this
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
            string name = entry.DisplayName ?? "not set";
            string score = entry.StatValue.ToString();
            int rank = entry.Position + 1;
            entries.Add(new LeaderboardEntry(name, score, rank));
        }
        LeaderboardBehaviour leaderboardBehaviour = leaderboardPanel.GetComponent<LeaderboardBehaviour>();
        leaderboardBehaviour.setLeaderboard(entries);
    }

    void getNewLeaderboard() {
        if (!model.loggedIn) {
            showUsernameSetting();
            return;
        }
        Debug.Log("Getting new leaderboard");
        var request = new GetLeaderboardRequest {
            StartPosition = 0,
            StatisticName = "Test", //TODO change this to the correct statistic name
            MaxResultsCount = 100 // TODO what should this be?
        };
        PlayFab.PlayFabClientAPI.GetLeaderboard(request, refreshLeaderboard, (error) => {
            Debug.Log("Leaderboard Failed");
            Debug.Log(error.GenerateErrorReport());
            AlertBoxBehaviour.ShowAlertAtBottomLeft("Error", error.GenerateErrorReport(), 3);
        });
    }
    /* -------------------------------------------------------------------------- */
    /*                         respond to username setting                        */
    /* -------------------------------------------------------------------------- */
    public void onUsernameChangeButtonHit() {
        model.loggedIn = false;
        showUsernameSetting();
    }
    public void onUsernameSet(string username) {
        model.loggedIn = true;
        model.currentUsername = username;
        showLeaderboard();
        getNewLeaderboard();
    }
    [SerializeField] private int score = 0;
    public void uploadScore() {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Test", //TODO change this to the correct statistic name
                    Value = score
                }
            }
        };
        PlayFab.PlayFabClientAPI.UpdatePlayerStatistics(request, (result) => {
            Debug.Log("UpdatePlayerStatistics Success");
            getNewLeaderboard();
        }, (error) => {
            Debug.Log("UpdatePlayerStatistics Failed");
            Debug.Log(error.GenerateErrorReport());
            AlertBoxBehaviour.ShowAlertAtBottomLeft("Error", error.GenerateErrorReport(), 3);
        });
    }
    /* -------------------------------------------------------------------------- */
    /*      理论上说外界只需要在适当时候创建这个类，然后调用这个方法就好                */
    /* -------------------------------------------------------------------------- */
    public void submitNewScore(int score) {
        this.score = score;
        Debug.Assert(model?.currentUsername != null);
        if (!model.loggedIn) {
            showUsernameSetting();
            return;
        }
        uploadScore();
    }
}
