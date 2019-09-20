using Illumina.Model;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.UI;

public class LoginView : MonoBehaviour {
    public InputField Username;
    public InputField Password;

    public void Login() {
        User updateUser = new User();
        updateUser.request.method = "LOGIN";
        updateUser.request.model_method = "UPDATE";
        updateUser.Username = Username.text;
        updateUser.Password = Password.text;
        updateUser.Update();
    }
}