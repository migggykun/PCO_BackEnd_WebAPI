using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PCO_BackEnd_WebAPI.Security.Cryptography.Hash
{
    public class HashManager
    {
        private string _salt;
        private string _plainTextValue;


        private string _hashedValue;

        public string HashedValue
        {
            get { return _hashedValue; }
            set { _hashedValue = value; }
        }

        public HashManager(string text, string salt)
        {
            _salt = salt;
            _plainTextValue = text;
            _hashedValue = GetSha256Hash(_plainTextValue, _salt);
        }

        /// <summary>
        /// Generates SHA256 encrypted string
        /// </summary>
        /// <param name="rawData"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private string GetSha256Hash(string rawData, string salt)
        {
            string hashWithSalt = rawData + salt;
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hashWithSalt));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        
    }
}