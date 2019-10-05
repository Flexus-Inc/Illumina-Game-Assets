namespace Illumina.Models {

    [System.Serializable]
    public class User : IlluminaModel {
        public string username;
        public string email;
        public string password;
        public string name;
        public int profile = 0;
        public bool logged_in = false;
        public string code = "";
    }

}