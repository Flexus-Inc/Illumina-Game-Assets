using System;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
using static Illumina.Networking.Request;

namespace Illumina.Controller {

    public class UserController : Controller {

        public static void UserExists(string identifier, string value, RequestSuccessEventHandler e, RequestFailedEventHandler f) {
            var tokenQuery = IlluminaHash.GetUniqueDateTimeHash();
            var uri = "/user/exists/" + tokenQuery;
            uri += "?" + identifier + "=" + value;
            Request userExistRequest = new Request {
                uri = uri,
            };
            userExistRequest.RequestSuccessEvents += e;
            userExistRequest.RequestFailedEvents += f;
            IlluminaWebRequest.Get(userExistRequest);
        }

        public static void Signup(User user) {
            user.password = IlluminaHash.GetHash(user.password);
            //user.username = IlluminaCipher.Encipher(user.username);
            Request signupRequest = new Request {
                uri = "/user",
                body = user
            };
            signupRequest.RequestSuccessEvents += OnSignUpRequestSuccess;
            signupRequest.RequestFailedEvents += OnSignUpRequestFailed;
            Store<User>(signupRequest);
        }

        public static void OnSignUpRequestSuccess(object source) {
            var user = (User) source;
            Debug.Log(user.GetServerMessage());
        }

        public static void OnSignUpRequestFailed(Exception err) {
            Debug.Log(err);
        }
        public static void Login(User user) {
            user.password = IlluminaHash.GetHash(user.password);
            //user.username = IlluminaCipher.Encipher(user.username);
            Request loginRequest = new Request {
                uri = "/user/login",
                body = user
            };
            loginRequest.RequestSuccessEvents += OnLoginRequestSuccess;
            loginRequest.RequestFailedEvents += OnLoginRequestFailed;
            Update<User>(loginRequest);
        }

        public static void OnLoginRequestSuccess(object source) {
            GameData.User = (User) source;
            Debug.Log(GameData.User.GetServerMessage());
            UIManager.HideLoading();
            ScenesManager.GoToScene(3);
        }

        public static void OnLoginRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Alert(err.Message);
        }

    }
}