using System;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
using UnityEngine.UI;

public class SignUpView : MonoBehaviour {
    public InputField UserName;
    public InputField Password;
    public InputField ConfirmPassword;
    public InputField Email;
    public InputField DisplayName;
    public Toggle TermsAndConditions;
    public Button VerifyButton;
    public Button SubmitButton;

    public Text UserNameExistText;
    public Text EmailExistText;
    public Text ConfirmPasswordNotMatchText;
    public Text VerificationEmail;
    public InputField VerificationCode;
    public Text VerificationCodeMismatch;
    string code;
    User newUser;
    Dictionary<string, bool> errors;

    void Awake() {
        if (IlluminaWebRequest.CsrfToken == null) {
            IlluminaWebRequest.GetCsrfToken();
        }
        newUser = new User();
        errors = new Dictionary<string, bool>();
        errors.Add("username", true);
        errors.Add("password", true);
        errors.Add("email", true);
    }

    public void OnEndEditUsername() {
        UserNameExistText.text = "Verifying...";
        UserController.UserExists("username", UserName.text, UsernameExistsResult, UsernameCheckError);
    }
    void UsernameExistsResult(object source) {
        UserNameExistText.text = (string) source;
        if (UserNameExistText.text == "Username already exist") {
            errors["username"] = true;
        } else {
            errors["username"] = false;
        }
        ConfirmTerms();
    }
    void UsernameCheckError(Exception err) {
        UserNameExistText.text = "Cannot verify";
    }

    public void OnEndEditPassword() {
        if (Password.text != ConfirmPassword.text && ConfirmPassword.text != "") {
            ConfirmPasswordNotMatchText.text = "Confirm password does not match";
            errors["password"] = true;
        } else {
            ConfirmPasswordNotMatchText.text = "";
            errors["password"] = false;
        }
        ConfirmTerms();
    }

    public void OnEndEditEmail() {
        EmailExistText.text = "Verifying...";
        UserController.UserExists("email", Email.text, EmailExistsResult, EmailCheckError);
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
    void EmailExistsResult(object source) {
        EmailExistText.text = (string) source;
        if (EmailExistText.text == "Email already exist") {
            errors["email"] = true;
        } else {
            errors["email"] = false;
        }
        ConfirmTerms();
    }

    void EmailCheckError(Exception err) {
        EmailExistText.text = "Cannot verify";
    }
    public void ChangeProfile(int index) {
        newUser.profile = index;
    }

    public void ConfirmTerms() {
        if (TermsAndConditions.isOn && !errors.ContainsValue(true)) {
            SubmitButton.interactable = true;
        } else {
            SubmitButton.interactable = false;
        }
    }

    public void Verify() {
        this.code = (new Key()).GenerateRandom(6);
        Debug.Log(this.code);
        newUser = new User {
            username = UserName.text,
            password = Password.text,
            email = Email.text,
            name = DisplayName.text,
            code = this.code
        };

        if (!errors.ContainsValue(true)) {
            VerificationEmail.text = newUser.email;
            UserController.VerifyEmail(newUser);
        } else {
            Debug.Log("Fix the errors first");
        }
    }
    public void Submit() {
        newUser.password = Password.text;

        if (!errors.ContainsValue(true)) {
            UserController.Signup(newUser);
            UIManager.DisplayLoading();
        } else {
            Debug.Log("Fix the errors first");
        }

    }

}