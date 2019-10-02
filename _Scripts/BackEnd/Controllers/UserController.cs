using System;
using Illumina.Models;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
using static Illumina.Networking.Request;

namespace Illumina.Controller {

    public class UserController : Controller {
        public static string password;

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

        public static void VerifyEmail(User user) {
            user.password = IlluminaHash.GetHash(user.password);
            Request verifyRequest = new Request {
                uri = "/user/verifyemail/" + IlluminaHash.GetUniqueDateTimeHash(),
                body = user
            };

            verifyRequest.RequestSuccessEvents += OnVerifyRequestSuccess;
            verifyRequest.RequestFailedEvents += OnVerifyRequestFailed;
            Store<User>(verifyRequest);
        }

        public static void Signup(User user) {
            password = user.password;
            user.password = IlluminaHash.GetHash(user.password);
            Request signupRequest = new Request {
                uri = "/user",
                body = user
            };
            signupRequest.RequestSuccessEvents += OnSignUpRequestSuccess;
            signupRequest.RequestFailedEvents += OnSignUpRequestFailed;
            Store<User>(signupRequest);
        }

        public static void OnVerifyRequestSuccess(object source) {
            var user = (User) source;
            Debug.Log("Code is sent to the email : " + user.email);
        }

        public static void OnVerifyRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Alert(err.Message);
        }
        public static void OnSignUpRequestSuccess(object source) {
            var user = (User) source;
            user.logged_in = true;
            user.password = password;
            Login(user);
            user.password = null;
        }

        public static void OnSignUpRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Alert(err.Message);
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

        public static void Logout(User user) {
            Request logoutRequest = new Request {
                uri = "/user/logout",
                body = user
            };

            logoutRequest.RequestSuccessEvents += OnLogoutRequestSuccess;
            logoutRequest.RequestFailedEvents += OnLogoutRequestFailed;
            Update<User>(logoutRequest);
        }

        public static void OnLogoutRequestSuccess(object source) {
            GameData.User = null;
            var user = (User) source;
            if (GameData.User == null) {
                Debug.Log(user.GetServerMessage());
                UIManager.HideLoading();
                ScenesManager.GoToScene(2);
            } else {
                UIManager.Alert("Error in logging out");
            }

        }

        public static void OnLogoutRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.Alert(err.Message);
        }

    }
}