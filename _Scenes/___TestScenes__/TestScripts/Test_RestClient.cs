using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;
using Proyecto26;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class _Post {
    public string Username;
    public int age;

}

public class Test_RestClient : MonoBehaviour {
    public InputField input;
    public Text result;

    // Start is called before the first frame update
    void Start() {
        IlluminaWebRequest.GetCsrfToken();
    }

    // Update is called once per frame
    public void Submit() {
        var post = new User {
            username = input.text,
        };
        Debug.Log(post.username);
        Request request = new Request {
            uri = "/test",
            body = post
        };
        request.RequestSuccessEvents += OnSuccess;
        request.RequestFailedEvents += OnFailure;
        IlluminaWebRequest.Post<User>(request);
    }

    public void OnSuccess(object source) {
        Debug.Log(((User) source).username);
        Debug.Log(((User) source).GetServerMessage());
    }
    public void OnFailure(Exception err) {
        Debug.Log(err);
    }
}