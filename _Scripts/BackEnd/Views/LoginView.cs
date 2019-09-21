using Illumina.Controller;
using Illumina.Models;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {
    public InputField UserName;
    public InputField Password;
    public Text UserNameExistText;
    public Button LoginButton;

    User user;
    void Awake() {
        if (IlluminaWebRequest.CsrfToken == null) {
            IlluminaWebRequest.GetCsrfToken();
        }
    }

    public void OnEndEditUsername() {
        UserNameExistText.text = "Verifying...";
        UserController.UserExists("username", UserName.text, UsernameExistsResult);
    }

    void UsernameExistsResult(object source) {
        if ((string) source != "Username already exists") {
            UserNameExistText.text = "Username not registered";
            LoginButton.interactable = false;
        } else {
            UserNameExistText.text = "";
            LoginButton.interactable = true;
        }
    }
    public void Login() {
        User user = new User {
            username = UserName.text,
            password = Password.text,
            logged_in = true
        };
        UserController.Login(user);
    }
}