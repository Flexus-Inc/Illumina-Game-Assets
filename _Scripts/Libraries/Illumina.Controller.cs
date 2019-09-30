/*
    Author:Edrian Jose D.G. Ferrer
    Collaborators: None yet
    Version: 1.0
    Owned by: Flexus Group of Companies
 */
using System;
using Illumina.Models;
using Illumina.Networking;
using UnityEngine;

namespace Illumina.Controller {

    public class Controller {
        public static void Store<T>(Request request) {
            IlluminaWebRequest.Post<T>(request);
        }
        public static void Update<T>(Request request) {
            IlluminaWebRequest.Put<T>(request);
        }
        public static void Delete(Request request) {
            IlluminaWebRequest.Delete(request);
        }

        public static void LogError(Exception err) {
            Debug.Log(err);
        }

    }
}