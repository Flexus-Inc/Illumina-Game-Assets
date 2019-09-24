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
    public Button SubmitButton;

    public Text UserNameExistText;
    public Text EmailExistText;
    public Text ConfirmPasswordNotMatchText;

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
        UserController.UserExists("username", UserName.text, UsernameExistsResult);
    }
    void UsernameExistsResult(object source) {
        UserNameExistText.text = (string) source;
        if (UserNameExistText.text == "Username already exists") {
            errors["username"] = true;
        } else {
            errors["username"] = false;
        }
    }
    public void OnEndEditPassword() {
        if (Password.text != ConfirmPassword.text && ConfirmPassword.text != "") {
            ConfirmPasswordNotMatchText.text = "Confirm password does not match";
            errors["password"] = true;
        } else {
            errors["password"] = false;
        }
    }

    public void OnEndEditEmail() {
        EmailExistText.text = "Verifying...";
        UserController.UserExists("email", Email.text, EmailExistsResult);
    }
    void EmailExistsResult(object source) {
        EmailExistText.text = (string) source;
        if (EmailExistText.text == "Email already exists") {
            errors["email"] = true;
        } else {
            errors["email"] = false;
        }
    }
    public void ChangeProfile(int index) {
        newUser.profile = index;
    }

    public void ConfirmTerms() {
        if (TermsAndConditions.isOn) {
            SubmitButton.interactable = true;
        } else {
            SubmitButton.interactable = false;
        }
    }

    // Start is called before the first frame update
    public void Submit() {
        User newUser = new User {
            username = UserName.text,
            password = Password.text,
            email = Email.text,
            name = DisplayName.text,
        };
        if (!errors.ContainsValue(true)) {
            UserController.Signup(newUser);
        } else {
            Debug.Log("Fix the errors first");
        }

    }

}