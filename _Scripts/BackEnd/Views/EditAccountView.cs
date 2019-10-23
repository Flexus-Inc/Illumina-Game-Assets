using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
using UnityEngine.UI;

public class EditAccountView : MonoBehaviour {
    public InputField DisplayName;
    public InputField Email;
    public Text EmailExistText;
    public InputField Password;
    public InputField NewPassword;
    public InputField ConfirmNewPassword;
    public Button SaveButton;
    public Text ConfirmPasswordNotMatchText;
    public Text OldPasswordIncorrectText;
    public InputField VerificationCode;
    public Text VerificationCodeMismatch;
    public Text VerificationEmail;
    public Button VerifyButton;
    public Animator VerificationPanel;
    User editUser;
    public bool emailerror = false;
    public bool oldpassworderror = false;
    public bool newpassworderror = false;
    public bool enableedit = true;
    string code = "";

    void Awake() {
        if (IlluminaWebRequest.CsrfToken == null) {
            IlluminaWebRequest.GetCsrfToken();
        }
        editUser = GameData.User;
        InitializeOldValues();
    }

    void InitializeOldValues() {
        DisplayName.text = editUser.name;
        Email.text = editUser.email;
        //insert also email here
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
        var result = (string) source;
        if (result == "Email already exist" && editUser.email != Email.text) {
            EmailExistText.text = result;
            emailerror = true;
        } else {
            emailerror = false;
        }
        EnableConfirmButton();
    }

    void EmailCheckError(Exception err) {
        EmailExistText.text = "Cannot verify";
        UIManager.Danger("Cannot verify your email address. Please check your internet connection or try again later");
        emailerror = true;
        EnableConfirmButton();
    }
    public void OnEndEditOldPassword() {
        var pass = IlluminaHash.GetHash(Password.text);
        if (IlluminaHash.CompareHash(pass, GameData.User.password) || (Password.text == "" || Password.text == null)) {
            oldpassworderror = false;
            OldPasswordIncorrectText.text = "";
        } else if (Password.text == "" || Password.text == null) {
            oldpassworderror = false;
            OldPasswordIncorrectText.text = "Old password is empty";
        } else {
            oldpassworderror = true;
            OldPasswordIncorrectText.text = "Old Password incorrect";
        }

        EnableConfirmButton();
    }

    public void OnEndEditPassword() {
        if ((NewPassword.text != ConfirmNewPassword.text) || Password.text == "" || Password.text == null) {
            if (Password.text == "" || Password.text == null) {
                OldPasswordIncorrectText.text = "Old password is empty";
            }
            if (NewPassword.text != ConfirmNewPassword.text) {
                ConfirmPasswordNotMatchText.text = "Confirm password does not match";
            } else {
                ConfirmPasswordNotMatchText.text = "";
            }
            newpassworderror = true;
        } else {
            if (OldPasswordIncorrectText.text == "Old password is empty") {
                OldPasswordIncorrectText.text = "";
            }
            ConfirmPasswordNotMatchText.text = "";
            newpassworderror = false;
        }

        if ((NewPassword.text == "" || NewPassword.text == null) && (ConfirmNewPassword.text == "" || ConfirmNewPassword.text == null)) {
            newpassworderror = false;
        }
        EnableConfirmButton();
    }

    void EnableConfirmButton() {
        if (!oldpassworderror && !newpassworderror && !emailerror) {
            SaveButton.interactable = true;
            enableedit = true;
        } else {
            SaveButton.interactable = false;
            enableedit = false;
        }
    }
    public void ChangeProfile(int index) {
        editUser.profile = index;
    }

    public void Edit() {
        EnableConfirmButton();
        if (enableedit) {
            if (editUser.email != Email.text) {
                var user = editUser;
                this.code = (new Key()).GenerateRandom(4);
                user.email = Email.text;
                user.code = this.code;
                UIManager.DisplayLoading();
                UserController.VerifyEmail(editUser, VerificationPanel);
            } else {
                editUser.name = DisplayName.text;
                if (NewPassword.text != "" && NewPassword.text != null) {
                    editUser.password = IlluminaHash.GetHash(NewPassword.text);
                    Debug.Log("password not null");
                }
                editUser.logged_in = true;
                UIManager.DisplayLoading();
                UserController.Edit(editUser);
            }
        } else {
            UIManager.Notify(Notification.Warning, "Fix the errors first");
        }

    }

    public void EditWithEmail() {
        editUser.name = DisplayName.text;
        if (enableedit) {
            if (NewPassword.text != "" && NewPassword.text != null) {
                editUser.password = IlluminaHash.GetHash(NewPassword.text);
                Debug.Log("password not null 2");
            }
            editUser.logged_in = true;
            editUser.email = Email.text;

            UIManager.DisplayLoading();
            UserController.Edit(editUser);
        } else {
            UIManager.Notify(Notification.Warning, "Fix the errors first");
        }
    }

}