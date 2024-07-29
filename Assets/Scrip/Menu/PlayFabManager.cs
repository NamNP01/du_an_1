using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{    
    public TMP_Text messageText;
   
    void Start()
    {
        Login();
    }    
    void Update()
    {
        
    }
    void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }
    
    
    void OnSuccess(LoginResult result)
    {        
        Debug.Log("Successful login/ account creat!");
    }
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
}
