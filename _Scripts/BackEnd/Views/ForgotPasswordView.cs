using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPasswordView : MonoBehaviour {
    public InputField Email;
    public Text EmailInvalid;
    public Button Confirm;
    // Start is called before the first frame update
    public static bool EmailConfirmationOnGoing = false;
    public bool EmailValid = false;
    void Start() {
        Confirm.interactable = false;
    }

    // Update is called once per frame
    public void OnChangedEmail(bool waitforpastconfirmation = true) {
        var check = waitforpastconfirmation?!EmailConfirmationOnGoing : true;
        if (check) {
            EmailConfirmationOnGoing = true;
            EmailInvalid.text = "Verifying...";
            UserController.UserExists("email", Email.text, EmailCheckResult, EmailCheckError);
        }
    }

    void EmailCheckResult(object source) {
        var result = (string) source;
        if (result == "Email already exist") {
            EmailValid = true;
            Confirm.interactable = true;
            EmailInvalid.text = "";
        } else {
            EmailValid = false;
            Confirm.interactable = false;
            EmailInvalid.text = "Email doesn't exist";
        }
        EmailConfirmationOnGoing = false;
    }

    void EmailCheckError(Exception err) {
        EmailInvalid.text = "Cannot verify";
        Confirm.interactable = false;
    }

    public void ConfirmForgotPass() {
        if (EmailValid) {
            UIManager.DisplayLoading();
            UserController.ResetPass(Email.text);
        }
    }
}