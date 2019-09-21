using System;
using System.Collections;
using System.Collections.Generic;
using Illumina.Security;
using Proyecto26;
using UnityEngine;
using UnityEngine.Networking;

namespace Illumina.Networking {

    public class Request {
        public delegate void RequestSuccessEventHandler(object source);
        public delegate void RequestFailedEventHandler(Exception error);
        public event RequestSuccessEventHandler RequestSuccessEvents;
        public event RequestFailedEventHandler RequestFailedEvents;
        public Dictionary<string, string> headers;
        public string uri = null;
        public object body = null;
        public void SetHeader(string key, string value) {
            headers.Add(key, value);
        }
        public Request() {
            headers = new Dictionary<string, string>();
        }

        public void CallSuccessEvents(object source) {
            RequestSuccessEvents(source);
        }
        public void CallFailedEvents(Exception error) {
            RequestFailedEvents(error);
        }
    }

    class IlluminaWebRequest {
        private static string csrf_token = null;

        public static string CsrfToken {
            get {
                if (csrf_token == null) {
                    return null;
                }
                return IlluminaHash.GetHash(csrf_token);
            }
        }

        public static void GetCsrfToken() {
            var tokenQuery = IlluminaHash.GetUniqueDateTimeHash();
            var uri = "/" + tokenQuery + "/token";
            Request csrfRequest = new Request {
                uri = uri,
            };
            csrfRequest.RequestSuccessEvents += SetCsrfToken;
            Get(csrfRequest);
        }

        private static void SetCsrfToken(object source) {
            csrf_token = (string) source;
        }

        public static void Post(Request request) {
            RequestHelper postRequest = new RequestHelper {
                Uri = request.uri,
                Body = request.body,
                Headers = request.headers
            };
            postRequest.Headers.Add("X-CSRF-TOKEN", csrf_token);

            RestClient.Post(postRequest)
                .Then(res => request.CallSuccessEvents(res.Text))
                .Catch(err => request.CallFailedEvents(err));
        }
        public static void Post<T>(Request request) {
            RequestHelper postRequest = new RequestHelper {
                Uri = request.uri,
                Body = request.body,
                Headers = request.headers
            };
            postRequest.Headers.Add("X-CSRF-TOKEN", csrf_token);
            RestClient.Post<T>(postRequest)
                .Then(res => request.CallSuccessEvents(res))
                .Catch(err => request.CallFailedEvents(err));
        }
        public static void Put(Request request) {
            RequestHelper putRequest = new RequestHelper {
                Uri = request.uri,
                Body = request.body,
                Headers = request.headers
            };
            putRequest.Headers.Add("X-CSRF-TOKEN", csrf_token);
            RestClient.Put(putRequest)
                .Then(res => request.CallSuccessEvents(res.Text))
                .Catch(err => request.CallFailedEvents(err));
        }
        public static void Put<T>(Request request) {
            RequestHelper putRequest = new RequestHelper {
                Uri = request.uri,
                Body = request.body,
                Headers = request.headers
            };
            putRequest.Headers.Add("X-CSRF-TOKEN", csrf_token);
            RestClient.Put<T>(putRequest)
                .Then(res => request.CallSuccessEvents(res))
                .Catch(err => request.CallFailedEvents(err));
        }

        public static void Delete(Request request) {
            RestClient.Delete(request.uri)
                .Then(res => request.CallSuccessEvents(res.Text))
                .Catch(err => request.CallFailedEvents(err));
        }

        public static void Get(Request request) {
            RestClient.Get(request.uri)
                .Then(res => request.CallSuccessEvents(res.Text))
                .Catch(err => request.CallFailedEvents(err));
        }
        public static void Get<T>(Request request) {
            RestClient.Get<T>(request.uri)
                .Then(res => request.CallSuccessEvents(res))
                .Catch(err => request.CallFailedEvents(err));
        }
        public static void GetArray<T>(Request request) {
            RestClient.GetArray<T>(request.uri)
                .Then(res => request.CallSuccessEvents(res))
                .Catch(err => request.CallFailedEvents(err));
        }

    }

}