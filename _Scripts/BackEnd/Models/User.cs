using System;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;
namespace Illumina.Model {

    //users
    class User : Model {

        public Field Username, Name, Email, Profile;
        public ProtectedField Password;

        
        public User() {
            Username = new Field("username");
            Name = new Field("name");
            Email = new Field("email");
            Profile = new Field("profile");
            Password = new ProtectedField("password");
            create_url = "/user";
        }

        public override bool OnBeforeCreate(Request req) {
            req["username"] = Username;
            req["name"] = Name;
            req["email"] = Email;
            req["profile"] = Profile;
            req["password"] = Password;
            req.SetHeader("X-CSRF-TOKEN", LaravelWebRequestHandler.csrfToken);
            return this.SetValues(req);
        }

        public override void OnCreated(Response response) {
            if (response.code == 302) {
                Debug.Log("You are now registered");
            } else {
                Debug.Log(response.text);
            }
        }

        public override bool OnBeforeUpdate(Request req) {
            if (req.method == "LOGIN") {
                update_url = "/user/login";
                //+ IlluminaHash.GetUniqueDateTimeHash();
                req["username"] = Username;
                req["name"] = Name;
                req["email"] = Email;
                req["profile"] = Profile;
                req["password"] = Password;
                req.SetHeader("X-CSRF-TOKEN", LaravelWebRequestHandler.csrfToken);
                return this.SetValues(req);
            }
            update_url = "/error.php";
            return true;
        }

        public override void OnUpdated(Response response) {
            if (response.code == 302) {
                Debug.Log(response.text);
                string[] edr = JsonUtility.FromJson<string[]>(response.text);
                foreach (var item in edr) {
                    Debug.Log(item);
                }
            } else {
                Debug.Log(response.error);
            }
        }

    }

}