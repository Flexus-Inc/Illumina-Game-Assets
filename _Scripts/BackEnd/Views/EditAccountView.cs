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
    public InputField Password;
    public InputField NewPassword;
    public InputField ConfirmNewPassword;
    public Button SaveButton;
    public Text ConfirmPasswordNotMatchText;
    public Text OldPasswordIncorrectText;
    User editUser;
    public bool oldpassworderror = false;
    public bool newpassworderror = false;
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

    public void OnEndEditOldPassword() {
        var pass = IlluminaHash.GetHash(Password.text);
        if (IlluminaHash.CompareHash(pass, GameData.User.password) && Password.text != null) {
            OldPasswordIncorrectText.text = "";
        } else {
            OldPasswordIncorrectText.text = "Old Password incorrect";
        }
    }

    public void OnEndEditPassword() {
        if (NewPassword.text != ConfirmNewPassword.text && ConfirmNewPassword.text != "") {
            ConfirmPasswordNotMatchText.text = "Confirm password does not match";
            newpassworderror = true;
        } else {
            ConfirmPasswordNotMatchText.text = "";
            newpassworderror = false;
        }
        EnableConfirmButton();
    }

    void EnableConfirmButton() {
        if (!oldpassworderror && !newpassworderror) {
            SaveButton.interactable = true;
        } else {
            SaveButton.interactable = false;
        }
    }
    public void ChangeProfile(int index) {
        editUser.profile = index;
    }

    public void Edit() {
        if (editUser.email != Email.text) {

        } else {
            editUser.name = DisplayName.text;
            editUser.password = NewPassword.text;
            editUser.logged_in = true;
            UIManager.DisplayLoading();
            UserController.Edit(editUser);
        }

    }

}