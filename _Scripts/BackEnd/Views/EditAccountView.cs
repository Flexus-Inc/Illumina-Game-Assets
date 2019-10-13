using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Illumina.Controller;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;

public class EditAccountView : MonoBehaviour
{
    public InputField DisplayName;
    public InputField Password;
    public InputField NewPassword;
    public InputField ConfirmNewPassword;
    public Button SaveButton;
    public Text ConfirmPasswordNotMatchText;
    public Text OldPasswordIncorrectText;
    User editUser;
    public bool password = true;
    
    void Awake(){
        if (IlluminaWebRequest.CsrfToken == null){
            IlluminaWebRequest.GetCsrfToken();
        }
        editUser = new User();
    }

    public void OnEndEditPassword(){
        if (NewPassword.text != ConfirmNewPassword.text && ConfirmNewPassword.text != "") {
            ConfirmPasswordNotMatchText.text = "Confirm password does not match";
            password = true;
            SaveButton.interactable = false;
        } else {
            ConfirmPasswordNotMatchText.text = "";
            password = false;
            SaveButton.interactable = true;
        }
    }
    public void ChangeProfile(int index) {
        editUser.profile = index;
    }
    
    public void Edit(){
        editUser = new User(){
            name = DisplayName.text,
            password = NewPassword.text
        };
        if(password == true){
            SaveButton.interactable = true;
            //UserController.Edit(editUser);
        }
        else{
            Debug.Log("Fix the errors first");
        }
    }

}
