using System;
using System.Collections.Generic;
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
            var uri = NetworkManager.Laravel_Uri + "/user/exists/" + tokenQuery;
            uri += "?" + identifier + "=" + value;
            Request userExistRequest = new Request {
                uri = uri,
            };
            userExistRequest.RequestSuccessEvents += e;
            userExistRequest.RequestFailedEvents += f;
            IlluminaWebRequest.Get(userExistRequest);
        }

        public static void VerifyEmail(User user) {

            Request verifyRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/user/verifyemail/" + IlluminaHash.GetUniqueDateTimeHash(),
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
                uri = NetworkManager.Laravel_Uri + "/user",
                body = user
            };
            signupRequest.RequestSuccessEvents += OnSignUpRequestSuccess;
            signupRequest.RequestFailedEvents += OnSignUpRequestFailed;
            Store<User>(signupRequest);
        }

        public static void OnVerifyRequestSuccess(object source) {
            UIManager.HideLoading();
            SignUpView.VerificationCodePanel.SetTrigger("Start");
            var user = (User) source;
            Debug.Log("Code is sent to the email : " + user.email);
        }

        public static void OnVerifyRequestFailed(Exception err) {
            UIManager.HideLoading();
            Debug.Log(err);
            UIManager.Danger("Cannot send email verification to your email. try again later or check if you input a valid email address");
        }
        public static void OnSignUpRequestSuccess(object source) {
            var user = (User) source;
            if (user.response_code == "0") {
                user.logged_in = true;
                user.password = password;
                var messages = new List<NotificationObject>();
                messages.Add(new NotificationObject {
                    type = Notification.Light,
                        message = "Welcome to Ilumina " + user.name,
                        show_at_top = true
                });
                messages.Add(new NotificationObject {
                    type = Notification.Primary,
                        message = "Play Now by clicking 'Enclasp' ",
                        show_at_top = false
                });
                ScenesManager.SceneStartUpMessages.Add(3, messages);
                Login(user);
                user.password = null;
                password = null;
            } else {
                UIManager.Warning(user.GetServerMessage());
            }

        }

        public static void OnSignUpRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.HideLoading();
            UIManager.Danger(err.Message);
        }
        public static void Login(User user) {
            user.password = IlluminaHash.GetHash(user.password);
            //user.username = IlluminaCipher.Encipher(user.username);
            Request loginRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/user/login",
                body = user
            };
            loginRequest.RequestSuccessEvents += OnLoginRequestSuccess;
            loginRequest.RequestFailedEvents += OnLoginRequestFailed;
            Update<User>(loginRequest);
        }

        public static void OnLoginRequestSuccess(object source) {
            var user = (User) source;
            Debug.Log(user.GetServerMessage());
            UIManager.HideLoading();
            if (user.response_code == "0") {
                GameData.User = user;
                ScenesManager.GoToScene(3);
            } else {
                UIManager.Warning(user.GetServerMessage());
            }

        }

        public static void OnLoginRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.HideLoading();
            UIManager.Danger(err.Message);
        }

        public static void Logout(User user) {
            Request logoutRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/user/logout",
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
                UIManager.HideLoading();
                UIManager.Danger("Error in logging out");
            }

        }

        public static void OnLogoutRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.HideLoading();
            UIManager.Danger(err.Message);
        }

        public static void ResetPass(string email) {
            User findUser = new User {
                email = email
            };
            Request forgotPassRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/user/forgotPass",
                body = findUser
            };
            forgotPassRequest.RequestSuccessEvents += OnForgotPassRequestSuccess;
            Update<User>(forgotPassRequest);
        }

        public static void OnForgotPassRequestSuccess(object source) {
            UIManager.HideLoading();
            ScenesManager.GoToScene(2);
        }
        public static void OnForgotPassRequestFailed(Exception err) {
            Debug.Log(err);
            UIManager.HideLoading();
            UIManager.Danger(err.Message);
        }

        public static void Edit(User user) {
            user.password = IlluminaHash.GetHash(user.password);
            password = user.password;
            Request editRequest = new Request {
                uri = NetworkManager.Laravel_Uri + "/user/editaccount",
                body = user
            };
            editRequest.RequestSuccessEvents += OnEditRequestSuccess;
            editRequest.RequestFailedEvents += OnEditRequestFailed;
            Store<User>(editRequest);
        }

        public static void OnEditRequestSuccess(object source) {
            var user = (User) source;
            user.password = password;
            GameData.User = user;
            password = null;
            Debug.Log(user.response_message);
            UIManager.HideLoading();
            ScenesManager.GoToScene(3);
        }
        public static void OnEditRequestFailed(Exception err) {
            UIManager.HideLoading();
            UIManager.Danger(err.Message);
        }

    }
}