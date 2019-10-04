using System;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {
    public InputField UserName;
    public InputField Email;
    public InputField Password;
    public Text EmailExistText;
    public Text UserNameExistText;
    public Button LoginButton;
    public Button ConfirmButton;

    User user;
    void Awake() {
        if (IlluminaWebRequest.CsrfToken == null) {
            IlluminaWebRequest.GetCsrfToken();
        }

    }

    void Start() {

    }

    public void OnEndEditUsername() {
        UserNameExistText.text = "Verifying...";
        UserController.UserExists("username", UserName.text, UsernameExistsResult, UsernameCheckError);
    }

    void UsernameExistsResult(object source) {
        if ((string) source != "Username already exist") {
            UserNameExistText.text = "Username not registered";
            LoginButton.interactable = false;
        } else {
            UserNameExistText.text = "";
            LoginButton.interactable = true;
        }
    }
    void UsernameCheckError(Exception err) {
        UserNameExistText.text = "Cannot verify";
        LoginButton.interactable = false;
        DisplayWarning();
    }

    public void OnEndEditEmail() {
        EmailExistText.text = "Verifying...";
        UserController.UserExists("email", Email.text, EmailExistsResult, EmailCheckError);
    }

    void EmailExistsResult(object source) {
        if ((string) source == "Email already exist") {
            EmailExistText.text = "";
            ConfirmButton.interactable = true;
        } else {
            EmailExistText.text = "";
            ConfirmButton.interactable = false;
        }
    }

    void EmailCheckError(Exception err) {
        EmailExistText.text = "Cannot verify";
        DisplayWarning();
    }

    void DisplayWarning() {
        UIManager.Notify(Notification.Warning, "Cannot connect from the server");
    }
    public void Login() {
        User user = new User {
            username = UserName.text,
            password = Password.text,
            logged_in = true
        };
        UserController.Login(user);
        UIManager.DisplayLoading();
    }

    public void ConfirmForgotPassword() {
        User user = new User {
            email = Email.text
        };
        UserController.ResetPass(user);
    }
}