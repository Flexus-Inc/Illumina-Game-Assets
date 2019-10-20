using System.Collections;
using System.Collections.Generic;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {
    public static string Laravel_Uri = "";
    //TODO: change to https://server.ilumina.flexus.online
    public static string Firebase_Uri = "https://illumina-6a2f2.firebaseio.com/";

    void OnEnable() {
        IlluminaWebRequest.GetCsrfToken();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SendPlayData(WWWForm form) {
        StartCoroutine(UploadPlayData(form));
    }

    public static void SendGameData(WWWForm form) {
        GameObject.Find("__NetworkManager").GetComponent<NetworkManager>().SendPlayData(form);
    }

    IEnumerator UploadPlayData(WWWForm form) {
        using(var w = UnityWebRequest.Post(Laravel_Uri + "/upload", form)) {
            //w.SetRequestHeader("X-CSRF-TOKEN", IlluminaWebRequest.csrf_token);
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError) {
                print(w.error);
            } else {
                print("Finished Uploading Screenshot");
            }
        }
    }
}