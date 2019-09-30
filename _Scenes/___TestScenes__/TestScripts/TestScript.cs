using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Illumina.Security;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {
    // Start is called before the first frame update
    public Text result;

    public void Open() {
        UIManager.PopUp("hello", "I am edrian", true, 0, 2);
        UIManager.CancelEvent += ChangeText;
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.Log("No internet connection");
        }
    }

    public void ChangeText() {
        result.text = "Confirmed";
    }

}