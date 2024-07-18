using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSubmissionController : MonoBehaviour {
    public TMP_InputField username;
    public MenuView menuView;
    void Start() {
        Debug.Assert(username != null);
        Debug.Assert(menuView != null);
        username.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter a name";
    }

    /* -------------------------------------------------------------------------- */
    /*                           these are for testings                           */
    /* -------------------------------------------------------------------------- */
    public void OnRefreshButtonHit() {
        refreshLeaderboard();
    }



    /* -------------------------------------------------------------------------- */
    /*             these are the actual functions that you need to use            */
    /* -------------------------------------------------------------------------- */

    public void refreshLeaderboard() {
        var request = new GetLeaderboardRequest {
            StartPosition = 0,
            StatisticName = "Test", //TODO change this to actual statistic name
        };
        PlayFab.PlayFabClientAPI.GetLeaderboard(request,
        (result) => {
            Debug.Log("Leaderboard Success");
            foreach (var entry in result.Leaderboard) {
                int rank = entry.Position + 1;
                string name = entry.DisplayName ?? "not set";
                int score = entry.StatValue;
                Debug.Log("Rank: " + rank + " Name: " + name + " Score: " + score);
                // TODO visualize them
            }
        }, (error) => {
            Debug.Log("Leaderboard Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    void submitScore(int score) {
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Test", //TODO change this to actual statistic name
                    Value = score
                }
            }
        };
        PlayFab.PlayFabClientAPI.UpdatePlayerStatistics(request,
        (result) => {
            Debug.Log("UpdatePlayerStatistics Success");
            refreshLeaderboard();
        }, (error) => {
            Debug.Log("UpdatePlayerStatistics Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }

}
