using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public TMP_InputField userName, passWord, regName, regPass;
    public TMP_Text messageText;
    public Selectable first;
    private EventSystem eventSystem;
    public Button login, fogetPass; 



    public void NewGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.DeleteAll();
    }
    public void CheckRegister()
    {
        //if (passWord.text.Length < 6)
        //{
        //    messageText.text ="";
        //}        
        var request = new RegisterPlayFabUserRequest
        {
            Email = regName.text,
            Password = regPass.text,            
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    public void CheckLogin()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = userName.text,
            Password = passWord.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }
    public void CheckResetPass()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = userName.text,
            TitleId = "D6FD8"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetPassword, OnError);
    }
    void Start()
    {
        
    }
    void Update()
    {

    }    
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {       
        messageText.text = "Đăng ký thành công!";     
    }
    void OnResetPassword(SendAccountRecoveryEmailResult result)
    {
        messageText.text = "Đã gửi email đặt lại mật khẩu!";
    }
    void OnLoginSuccess(LoginResult result)
    {
        SceneManager.LoadScene(1);        
        messageText.text = "Đăng nhập thành công!";
    }    
    void OnError(PlayFabError error)
    {
        messageText.text = error.ErrorMessage;
        Debug.Log(error.GenerateErrorReport());
    }
    
}
