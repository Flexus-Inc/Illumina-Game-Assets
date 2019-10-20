using System;
using System.Collections;
using System.Collections.Generic;

namespace FedLibrary.Encrypting {

    public class VigenèreCipher {
        string keyword;

        public VigenèreCipher(string keyword) {
            this.keyword = keyword;
        }

        static string Crypt(string text, string keyword, bool isEncryption = true) {
            int factor = isEncryption ? 1 : -1;
            /*
            INT FACTOR = 1;
            if(isEncryption){
                factor =
            }
             */
            int len = text.Length, j = 0;
            string key = "", encrypted = "";
            while (j < len) {
                key += keyword;
                j += keyword.Length;
            }
            key = key.Substring(0, len);

            for (int i = 0; i < len; i++) {
                int temp = (int) (text[i] - 65) + (int) ((key[i] - 65) * factor);
                bool condition = isEncryption ? temp > 25 : temp < 0;
                temp = condition ? temp - (26 * factor) : temp;
                encrypted += (char) (temp + 65);
            }

            return encrypted;
        }

        public string Encrypt(string text) {
            return Crypt(text, this.keyword);
        }
        public string Decrypt(string text) {
            return Crypt(text, this.keyword, false);
        }
        public static string Encrypt(string text, string keyword) {
            return Crypt(text, keyword);
        }
        public static string Decrypt(string text, string keyword) {
            return Crypt(text, keyword, false);
        }
    }

    public static class FedCipher {
        static List<char> vowels = new List<char>() { 'A', 'E', 'I', 'O', 'U', 'a', 'e', 'i', 'o', 'u' };

        public static string Encrypt(string text, bool CommaSeperated = false) {
            char[] text_chars = text.ToCharArray();
            string encrypted = "";
            foreach (var item in text_chars) {
                int temp = ((int) item - 65);
                int encrypted_char = (999 - (14 * temp));
                if (vowels.Contains(item)) {
                    encrypted_char--;
                }
                encrypted += encrypted_char.ToString();
                encrypted += CommaSeperated ? ", " : "";
            }
            encrypted = CommaSeperated ? encrypted.Substring(0, encrypted.Length - 2) : encrypted;
            return encrypted;
        }
        public static string Decrypt(string text, bool CommaSeperated = false) {
            List<int> text_ints = new List<int>();
            char[] text_chars = text.ToCharArray();
            string decrypted = "";
            for (int i = 0; i < text_chars.Length; i += 3) {
                if (i + 2 < text_chars.Length) {
                    string whole_int = "" + text_chars[i] + text_chars[i + 1] + text_chars[i + 2];
                    text_ints.Add(Convert.ToInt32(whole_int));
                }

            }

            foreach (var item in text_ints) {
                int temp = item;
                temp = temp % 2 == 0 ? temp + 1 : temp;
                temp = ((999 - temp) / 14) + 65;

                char decrypted_int = (char) temp;
                decrypted += decrypted_int;
                decrypted += CommaSeperated ? ", " : "";
            }
            decrypted = CommaSeperated ? decrypted.Substring(0, decrypted.Length - 2) : decrypted;
            return decrypted;
        }
    }

    public class CaesarCipher {

        public static string Encrypt(string text, int shifts, int direction) {
            var text_chars = text.ToCharArray();
            string encrypted = "";
            foreach (var item in text_chars) {
                int temp = (Convert.ToInt32(item - 65)) + (shifts * direction);
                if (temp < 0) {
                    temp = 26 + temp;
                }
                if (temp > 25) {
                    temp = (temp - 26);
                }
                char n = Convert.ToChar(temp + 65);
                encrypted += n;
            }

            return encrypted;
        }
    }

}