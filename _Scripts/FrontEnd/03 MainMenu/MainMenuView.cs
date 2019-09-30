using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour {

    public Text DisplayName;
    public Text Email;
    public Image Profile;
    // Start is called before the first frame update
    void Start() {
        MusicManager.ChangeMusic(1, true, 1, 1);
        Profile.sprite = GameDataManager.GetProfileAvatar();
        DisplayName.text = GameData.User.name;
        Email.text = GameData.User.email;
        Debug.Log(GameData.User.name);
    }

    // Update is called once per frame
    void Update() {

    }
}