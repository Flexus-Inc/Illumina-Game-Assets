using System.Collections;
using System.Collections.Generic;
using Illumina.Networking;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour {
    public static string Laravel_Uri = "https://server.ilumina.flexus.online";

    //TODO: change to https://www.server.ilumina.flexus.online
    public static string Firebase_Uri = "https://illumina-6a2f2.firebaseio.com/";

    void OnEnable() {
        IlluminaWebRequest.GetCsrfToken();
    }

    // Update is called once per frame
    void Start() {
        StartCoroutine(ListenToConnectionChanges());
    }

    IEnumerator ListenToConnectionChanges() {
        var closed = true;
        while (true) {
            if (Application.internetReachability == NetworkReachability.NotReachable && closed) {
                UIManager.AlertBox(Notification.Warning, "No internet connection. ", false);
                closed = false;
            }
            if (Application.internetReachability != NetworkReachability.NotReachable && !closed) {
                UIManager.enableClosing = true;
                UIManager.popup_open = false;
                closed = true;
            }
            yield return new WaitForSeconds(1);

        }
    }
}