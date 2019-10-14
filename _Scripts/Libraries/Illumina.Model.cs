/*
    Author:Edrian Jose D.G. Ferrer
    Collaborators: None yet
    Version: 1.0
    Owned by: Flexus Group of Companies
 */

using System;
using System.Collections.Generic;
using Illumina.Networking;
using Illumina.Security;
using UnityEngine;

namespace Illumina.Models {

    [System.Serializable]
    public class IlluminaModel {
        public string response_message = "";
        public string response_code = "";

        public string GetServerMessage() => response_message;
        public string GetServerResponseCode() => response_code;
    }

}