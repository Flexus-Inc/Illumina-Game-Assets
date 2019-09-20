using System.Collections;
using System.Collections.Generic;
using Illumina.Security;
using UnityEngine;
using UnityEngine.Networking;

namespace Illumina.Networking {

    public class Request {

        public Dictionary<string, string> requests;
        public Dictionary<string, string> headers;
        public string model_method = "CREATE";
        public string method = "CREATE";
        public string csrf_token = null;
        public string this [string key] {
            set {
                if (requests.ContainsKey(key)) {
                    requests[key] = value;
                } else {
                    requests.Add(key, value);
                }
            }
            get => requests[key];

        }
        public void SetHeader(string key, string value) {
            headers.Add(key, value);
        }
        public Request() {
            requests = new Dictionary<string, string>();
            headers = new Dictionary<string, string>();
        }
    }

    public class Response {
        public Dictionary<string, string> headers;
        public string text;
        public long code;
        public string error;
        public Response() {
            headers = new Dictionary<string, string>();
            text = null;
            error = null;
            code = 302;
        }
    }

    public static class WebRequestManager {

        public static string GetCSRFToken() {
            return LaravelWebRequestHandler.csrfToken;
        }
    }

}