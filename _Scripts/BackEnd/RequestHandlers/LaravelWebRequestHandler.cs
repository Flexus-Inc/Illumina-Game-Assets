using System.Collections;
using System.Collections.Generic;
using Illumina.Model;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
using UnityEngine.Networking;

public class LaravelWebRequestHandler : MonoBehaviour {
    public static string csrfToken = null;
    public static Dictionary<string, Response> responses = new Dictionary<string, Response>();

    void Awake() {
        StartCoroutine(_GetCSRFToken());
    }
    IEnumerator _GetCSRFToken() {

        string tokenQuery = IlluminaHash.GetUniqueDateTimeHash();
        string url = "/" + tokenQuery + "/token";
        responses.Add(url, new Response());
        using(UnityWebRequest www = UnityWebRequest.Get(url)) {

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {

                responses[url].error = www.error;
                responses[url].code = www.responseCode;

            } else {
                csrfToken = www.downloadHandler.text;
                responses[url].text = csrfToken;
            }

            responses[url].headers = www.GetResponseHeaders();
        }
    }

    public static void CreateGet(string url, Model model) {
        GameObject.Find("__WebRequestManagers").GetComponent<LaravelWebRequestHandler>()._Get(url, model);
    }
    public void _Get(string url, Model model) {
        StartCoroutine(Get(url, model));
    }
    public static IEnumerator Get(string url, Model model) {
        url += "?" + model.ToString();
        if (responses.ContainsKey(url)) {
            responses[url] = new Response();
        } else {
            responses.Add(url, new Response());
        }

        using(UnityWebRequest www = UnityWebRequest.Get(url)) {
            if (model.request.headers != null) {
                foreach (var item in model.request.headers) {
                    www.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                responses[url].error = www.error;
                responses[url].code = www.responseCode;

            } else {
                responses[url].text = csrfToken;
            }

            responses[url].headers = www.GetResponseHeaders();
        }
        model.CallAfterEvents(responses[url]);
    }

    public static void CreatePost(string url, Model model) {
        GameObject.Find("__WebRequestManagers").GetComponent<LaravelWebRequestHandler>()._Post(url, model);
    }
    public void _Post(string url, Model model) {
        StartCoroutine(Post(url, model));
    }
    public static IEnumerator Post(string url, Model model) {

        if (responses.ContainsKey(url)) {
            responses[url] = new Response();
        } else {
            responses.Add(url, new Response());
        }
        using(UnityWebRequest www = UnityWebRequest.Post(url, model.ToForm())) {
            if (model.request.headers != null) {
                foreach (var item in model.request.headers) {
                    www.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                responses[url].error = www.error;
                responses[url].code = www.responseCode;
                responses[url].text = www.downloadHandler.text;
            } else {
                if (www.downloadHandler.text != null) {
                    responses[url].text = www.downloadHandler.text;
                } else {
                    responses[url].text = "SUCCESS";
                }

            }

        }

        model.CallAfterEvents(responses[url]);
    }

    public static void CreatePut(string url, Model model) {
        GameObject.Find("__WebRequestManagers").GetComponent<LaravelWebRequestHandler>()._Put(url, model);
    }
    public void _Put(string url, Model model) {
        StartCoroutine(Put(url, model));
    }
    public static IEnumerator Put(string url, Model model) {

        if (responses.ContainsKey(url)) {
            responses[url] = new Response();
        } else {
            responses.Add(url, new Response());
        }
        using(UnityWebRequest www = UnityWebRequest.Put(url, model.ToString())) {
            if (model.request.headers != null) {
                foreach (var item in model.request.headers) {
                    www.SetRequestHeader(item.Key, item.Value);
                }
            }
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError) {
                responses[url].error = www.error;
                responses[url].code = www.responseCode;
                responses[url].text = www.downloadHandler.text;
            } else {
                if (www.downloadHandler.text != null) {
                    responses[url].text = www.downloadHandler.text;
                } else {
                    responses[url].text = "SUCCESS";
                }

            }

        }

        model.CallAfterEvents(responses[url]);
    }

}