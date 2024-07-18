using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoginOrRegister : MonoBehaviour {
    public TMP_InputField password;
    public TMP_InputField username;

    public void login() {
        username.placeholder.GetComponent<TMP_Text>().text = "kook";
        string user = username.text;
        string pass = password.text;
        Debug.Log("Username: " + user + " Password: " + pass);

        var request = new LoginWithEmailAddressRequest {
            Email = user,
            Password = pass
        };
        PlayFab.PlayFabClientAPI.LoginWithEmailAddress(request, (result) => {
            Debug.Log("Login Success");
        }, (error) => {
            Debug.Log("Login Failed");
            Debug.Log(error.GenerateErrorReport());
            Debug.Log(error.ErrorMessage);
        });
    }
    public void register() {
        string user = username.text;
        string pass = password.text;
        Debug.Log("Username: " + user + " Password: " + pass);

        var request = new RegisterPlayFabUserRequest {
            Email = user,
            Password = pass,
            RequireBothUsernameAndEmail = false
        };
        PlayFab.PlayFabClientAPI.RegisterPlayFabUser(request, (result) => {
            Debug.Log("Register Success");
        }, (error) => {
            Debug.Log("Register Failed");
        });
    }
}
