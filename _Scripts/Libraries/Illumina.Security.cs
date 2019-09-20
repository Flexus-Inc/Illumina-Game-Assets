using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using FedLibrary.Encrypting;
using UnityEngine;

namespace Illumina.Security {

    public static class Hasher {
        // Start is called before the first frame update

        public static string GetHash(HashAlgorithm hashAlgorithm, string input) {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++) {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash) {
            // Hash the input.
            var hashOfInput = GetHash(hashAlgorithm, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, hash) == 0;
        }
    }
    static class Keys {
        public static string DataTimeKey(bool Hashed = false, bool ContainingSeconds = false) {
            var now = DateTime.UtcNow;
            var str = "" + now.Year + now.Month + now.Day + now.Hour + now.Minute;
            str = ContainingSeconds ? str + now.Second : str;
            if (Hashed) {
                return Hasher.GetHash(MD5.Create(), str);
            }
            return str;
        }

        public static string RandomKey(int length = 10) {
            var rand = new System.Random();
            char[] keyChar = new char[length];
            for (int i = 0; i < length; i++) {
                var startingChar = rand.Next(0, 2) == 1 ? 'a' : 'A';
                keyChar[i] = (char) (startingChar + rand.Next(0, 26));
            }
            return new string(keyChar);
        }

    }

    static class IlluminaHash {

        public static string GetUniqueDateTimeHash() {
            return GetHash(Keys.DataTimeKey());
        }
        public static string GetHash(string msg) {
            var keystring = Keys.RandomKey(msg.Length);
            char[] cipheredMsg = new char[msg.Length * 2];
            for (int i = 0; i < msg.Length; i++) {
                cipheredMsg[i * 2] = msg[msg.Length - 1 - i];
                cipheredMsg[(i * 2) + 1] = msg[i];
            }
            var hashedKeyString = Hasher.GetHash(SHA1.Create(), keystring);
            var hashedCipheredMsg = Hasher.GetHash(MD5.Create(), new string(cipheredMsg));
            char[] IlliminaHashedMsg = new char[hashedCipheredMsg.Length * 2];
            for (int i = 0; i < hashedCipheredMsg.Length; i++) {
                IlliminaHashedMsg[i * 2] = hashedKeyString[i];
                IlliminaHashedMsg[(i * 2) + 1] = hashedCipheredMsg[i];
            }
            return new string(IlliminaHashedMsg);
        }

        public static bool CompareHash(string leftHash, string rightHash) {
            char[] leftOriginalHash = new char[leftHash.Length / 2];
            char[] rightOriginalHash = new char[rightHash.Length / 2];
            for (int i = 0; i < (leftHash.Length / 2); i++) {
                leftOriginalHash[i] = leftHash[(i * 2) + 1];
                rightOriginalHash[i] = rightHash[(i * 2) + 1];
            }
            if (new string(leftOriginalHash) == new string(rightOriginalHash)) {
                return true;
            } else {
                return false;
            }
        }
    }

    static class IlluminaCipher {

        static string[] keywords = { "HAVING", "FUN", "WITH", "ILLUMINA" };
        public static string Encipher(string value) {
            string randomKey = Keys.RandomKey(value.Length * 4);
            List<string> cipheredParts = new List<string>();
            List<List<char>> keys = new List<List<char>>() {
                new List<char>(),
                new List<char>(),
                new List<char>(),
                new List<char>()
            };

            var charArray = value.ToCharArray();
            int a = 0, b = 0;
            for (int i = 0; i < value.Length; i++) {
                a = 0;
                b = 0;
                int temp = charArray[i] - 32;
                int temp_part = temp / 4;
                int temp_parts_excess = temp % 4;

                if (i % 2 == 0) { a = temp_parts_excess; } else { b = temp_parts_excess; }
                keys[0].Add((char) (temp_part + a + 65));
                keys[1].Add((char) (temp_part + 65));
                keys[2].Add((char) (temp_part + 65));
                keys[3].Add((char) (temp_part + b + 65));
            }

            for (int i = 0; i < 4; i++) {
                cipheredParts.Add(new string(keys[i].ToArray()));
                cipheredParts[i] = VigenèreCipher.Encrypt(cipheredParts[i], keywords[i]);
            }
            var cipheredString = cipheredParts[0] + cipheredParts[1] + cipheredParts[2] + cipheredParts[3];
            char[] cipheredMsg = new char[cipheredString.Length * 2];
            for (int i = 0; i < cipheredString.Length; i++) {
                cipheredMsg[i * 2] = randomKey[i];
                cipheredMsg[(i * 2) + 1] = cipheredString[i];
            }

            return new string(cipheredMsg);

        }

        public static string Decipher(string raw) {
            string originalDecipheredString = "";
            char[] originalDecipheredChar = new char[raw.Length / 2];
            List<string> decipheredParts = new List<string>();
            for (int i = 0; i < (raw.Length / 2); i++) {
                originalDecipheredChar[i] = raw[(i * 2) + 1];
            }
            originalDecipheredString = new string(originalDecipheredChar);
            int len = originalDecipheredString.Length / 4;
            for (int i = 0; i < 4; i++) {
                decipheredParts.Add(originalDecipheredString.Substring(i * len, len));
                decipheredParts[i] = VigenèreCipher.Decrypt(decipheredParts[i], keywords[i]);
            }
            char[] decipheredChar = new char[len];
            for (int j = 0; j < len; j++) {
                int sum = 0;
                for (int k = 0; k < 4; k++) {
                    sum += decipheredParts[k].ToCharArray() [j] - 65;
                }
                sum += 32;
                decipheredChar[j] = (char) sum;
            }

            return new string(decipheredChar);

        }
    }
}