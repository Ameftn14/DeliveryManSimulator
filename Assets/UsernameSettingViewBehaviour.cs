using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UsernameSettingViewBehaviour : MonoBehaviour {
    public TMP_InputField username;
    public string currentUsername;
    public UnityEvent<string> onUsernameSet;

    void Start() {
        Debug.Assert(username != null);
    }
    void Update() {
        username.placeholder.GetComponent<TextMeshProUGUI>().text = currentUsername ?? "Username";
    }
    public bool hasUsername() {
        return currentUsername != null;
    }

    public void onSubmitButtonHit() {
        string usernameStr = username.text;
        if (usernameStr.Length < 3 || usernameStr.Length > 25) {
            AlertBoxBehaviour.ShowAlertAtBottomLeft("Error", "Username must be between 3 and 25 characters", 3);
            return;
        }
        loginSetName(usernameStr);
    }
    public static UsernameSettingViewBehaviour spawnUsernameSettingView(GameObject parent, string currentUsername) {
        GameObject usernameSettingView = Instantiate(Resources.Load("Prefabs/UI/UsernameSettingView")) as GameObject;
        usernameSettingView.transform.SetParent(parent.transform, false);
        usernameSettingView.GetComponent<UsernameSettingViewBehaviour>().currentUsername = currentUsername;
        return usernameSettingView.GetComponent<UsernameSettingViewBehaviour>();
    }
    void loginSetName(string userid) {
        Debug.Log("Username: " + userid);
        var request = new LoginWithCustomIDRequest {
            CustomId = userid,
            CreateAccount = true
        };
        PlayFab.PlayFabClientAPI.LoginWithCustomID(request,
        (result) => {
            Debug.Log("Login Success");
            setUsername(userid);
        }, (error) => {
            Debug.Log("Login Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    void login(string userid) {
        Debug.Log("Username: " + userid);
        var request = new LoginWithCustomIDRequest {
            CustomId = userid,
            CreateAccount = true
        };
        PlayFab.PlayFabClientAPI.LoginWithCustomID(request,
        (result) => {
            Debug.Log("Login Success");
        }, (error) => {
            Debug.Log("Login Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    void setUsername(string username) {
        var request2 = new UpdateUserTitleDisplayNameRequest {
            DisplayName = username
        };
        PlayFab.PlayFabClientAPI.UpdateUserTitleDisplayName(request2,
        (result) => {
            Debug.Log("UpdateUserTitleDisplayName Success");
            currentUsername = username;
            onUsernameSet.Invoke(username);
        }, (error) => {
            Debug.Log("UpdateUserTitleDisplayName Failed");
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
