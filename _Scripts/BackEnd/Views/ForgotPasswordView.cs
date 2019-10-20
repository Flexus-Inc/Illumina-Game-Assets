using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Security;
using UnityEngine;
using UnityEngine.UI;

public class ForgotPasswordView : MonoBehaviour {
    //TODO: make it working by sunday;
    public InputField Email;
    public InputField VerificationCode;
    public Text EmailInvalid;
    public Text VerificationCodeMismatch;
    public Button Confirm;
    public Button VerifyButton;
    // Start is called before the first frame update
    public static bool EmailConfirmationOnGoing = false;
    public bool EmailValid = false;
    string code;
    void Start() {
        Confirm.interactable = false;
    }

    // Update is called once per frame
    public void OnEndEditEmailEmail() {
        EmailInvalid.text = "Verifying...";
        UserController.UserExists("email", Email.text, EmailCheckResult, EmailCheckError);
    }
    public void OnEditCode() {
        if (VerificationCode.text != code) {
            VerificationCodeMismatch.text = "Invalid Code";
            VerifyButton.interactable = false;
        } else {
            VerificationCodeMismatch.text = "";
            VerifyButton.interactable = true;
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
    }

    void EmailCheckError(Exception err) {
        EmailInvalid.text = "Cannot verify";
        Confirm.interactable = false;
        UIManager.Notify(Notification.Warning, "Cannot connect from the server");
    }

    public void VerifyIdentity() {
        this.code = (new Key()).GenerateRandom(10);
        Debug.Log(this.code);
        User user = new User {
            email = Email.text,
            code = this.code
        };
        if (EmailValid) {
            UserController.VerifyEmail(user);
        }
    }

    public void ConfirmForgotPass() {
        if (EmailValid) {
            UIManager.DisplayLoading();
            UserController.ResetPass(Email.text);
        }
    }
}