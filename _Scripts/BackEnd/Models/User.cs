namespace Illumina.Models {

    public class User : IlluminaModel {
        public string username;
        public string email;
        public string password;
        public string name;
        public int profile = 1;
        public bool logged_in = false;
    }

}