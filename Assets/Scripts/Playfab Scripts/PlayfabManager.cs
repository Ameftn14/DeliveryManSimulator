using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class PlayfabManager : MonoBehaviour {
    // Start is called before the first frame update


    /* -------------------------------------------------------------------------- */
    /*                                    Login                                   */
    /* -------------------------------------------------------------------------- */
    public void Login() {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            // handling player name
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, CreateLoginSuccessFunction((result) => {
            Debug.Log("hi i am a lambda function");
        }), onLoginFailure);
    }

    Func<Action<LoginResult>, Action<LoginResult>> CreateLoginSuccessFunction = (a) => (result) => {
        Debug.Log("LoginSuccess");
        a(result);
    };
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
        // but first we login and submit the display name
        // TODO this should be chained together using the 'on success' thingy
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
            int rank = player.Position + 1;
            string playerName = player.DisplayName ?? "not set";
            int score = player.StatValue;
            Debug.Log($"Rank: {rank}, Name: {playerName}, Score: {score}");
        }
    }
    void OnLeaderboardGetFailed(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    /* -------------------------------------------------------------------------- */
    /*                          Submit Player DisplayName                         */
    /* -------------------------------------------------------------------------- */
    // refer to the tmpInputField in the unity editor
    public TMP_InputField inputField;
    public void SubmitName() {
        string name = inputField.text;
        Debug.Log($"Submitting display name: {name}");
        if (name.Length < 3 || name.Length > 25) {
            Debug.LogWarning("Display name length must be between 3 and 25 characters.");
            return;
        }
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSubmitSuccess, OnDisplayNameSubmitFailed);
    }
    void OnDisplayNameSubmitSuccess(UpdateUserTitleDisplayNameResult result) {
        Debug.Log("Successfully submitted your display name!");
    }
    void OnDisplayNameSubmitFailed(PlayFabError error) {
        Debug.LogWarning("Something went wrong with your API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }
    /* -------------------------------------------------------------------------- */
    /*                      Auto Send Score with DisplayName                      */
    /* -------------------------------------------------------------------------- */
    // public void submitScoreChain() {
    //     string name = inputField.text;
    //     Debug.Log($"Submitting display name: {name}");
    //     if (name.Length < 3 || name.Length > 25) {
    //         Debug.LogWarning("Display name length must be between 3 and 25 characters.");
    //         return;
    //     }
    //     // login
    //     var request = new LoginWithCustomIDRequest {
    //         CustomId = name, // uuid will be generated based on this
    //         CreateAccount = true,
    //         // handling player name
    //         InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
    //             GetPlayerProfile = true
    //         }
    //     };
    //     PlayFabClientAPI.LoginWithCustomID(request, CreateLoginSuccessFunction((result) => {
    //         SubmitName();
    //     }), onLoginFailure);

    // }

}
