using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class PlayfabManager : MonoBehaviour {
    // Start is called before the first frame update


    /* -------------------------------------------------------------------------- */
    /*                                    Login                                   */
    /* -------------------------------------------------------------------------- */
    public void Login() {
        var request = new LoginWithCustomIDRequest {
            CustomId = "GettingStartedGuide",
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, onLoginSuccess, onLoginFailure);
    }
    void onLoginSuccess(LoginResult result) {
        Debug.Log("Congratulations, you made your first successful API call!");
    }
    void onLoginFailure(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    /* -------------------------------------------------------------------------- */
    /*                        Send to  Leaderboard                                */
    /* -------------------------------------------------------------------------- */
    //you will have to login first though
    public void sendScore() {
        int score = 100;
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate {
                    StatisticName = "Test",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnStatUploadSuccess, OnStatUploadFailed);
    }
    void OnStatUploadSuccess(UpdatePlayerStatisticsResult result) {
        Debug.Log("Successfully sent your score to the leaderboard!");
    }
    void OnStatUploadFailed(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }


    /* -------------------------------------------------------------------------- */
    /*                            Get from Leaderboard                            */
    /* -------------------------------------------------------------------------- */
    public void getLeaderboard() {
        var request = new GetLeaderboardRequest {
            StatisticName = "Test",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnLeaderboardGetFailed);
    }
    void OnLeaderboardGet(GetLeaderboardResult result) {
        Debug.Log("Successfully got the leaderboard!");
        foreach (var player in result.Leaderboard) {
            Debug.Log(player.Position + " " + player.DisplayName + ": " + player.StatValue);
        }
    }
    void OnLeaderboardGetFailed(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
