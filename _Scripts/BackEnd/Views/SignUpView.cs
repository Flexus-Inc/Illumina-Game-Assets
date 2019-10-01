using Illumina.Model;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.UI;

public class SignUpView : MonoBehaviour {
    public InputField UserName;
    public InputField Password;
    public InputField ConfirmPassword;
    public InputField Email;
    public InputField DisplayName;
    public Toggle TermsAndConditions;

    public string csrfToken = "a3Z0CCiml6OrmxfeN4r4nu6Z9yTebBP4CfjYmqRG";
    public void Submit() {
        User newUser = new User();
        newUser.Username = UserName.text;
        newUser.Name = DisplayName.text;
        newUser.Email = Email.text;
        newUser.Password = Password.text;
        newUser.Profile = "2";
        newUser.Create();
    }

}