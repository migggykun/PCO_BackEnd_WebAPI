using PCO_BackEnd_WebAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class StringManipulationHelper
    {
        private const string baseAdminAddress = "https://stgadmin.philippineoptometry.org/#";
        private const string baseClientAddress = "https://philippineoptometry.org";
        private const string confirmEmailPage = "/confirm-email";
        private const string resetPasswordPage = "/reset-password";
        private const string confirmEmailBaseURLAdmin = baseAdminAddress + confirmEmailPage;
        private const string resetPasswordBaseURLAdmin = baseAdminAddress + resetPasswordPage;
        private const string confirmEmailBaseURLClient = baseClientAddress + confirmEmailPage;
        private const string resetPasswordBaseURLClient = baseClientAddress + resetPasswordPage;
        private const string confirmPhoneBaseURLAdmin = baseAdminAddress + confirmEmailPage;
        private const string confirmPhoneBaseURLClient = baseClientAddress + confirmEmailPage;

        /// <summary>
        /// Use in encryption formula. To be added in UserID
        /// </summary>
        private const long ADDEND = 1245689;
        /// <summary>
        /// Cipher - Dicipher string (RN - Renz MZ - Migz GY - Gendy MC - Michelle JS- Jonas)
        /// </summary>
        private const string CUSTOM_ALPHABET = "RNMZGYMCJS";
        public static string SetParameter(string aEmail, string token, bool isAdmin)
        {
            if (isAdmin)
            {
                return string.Format("{0}/{1}", aEmail, HttpUtility.UrlEncode(token, System.Text.Encoding.UTF8));
            }
            else
            {
                return string.Format("email={0}&token={1}", aEmail, HttpUtility.UrlEncode(token, System.Text.Encoding.UTF8));
            }
        }

        public static string ConvertToHyperLink(string link)
        {
            return string.Format("<a href=\"{0}\">link</a>", link);
        }

        public static string SetConfirmEmailUrl(string token, bool isAdmin)
        {
            if (isAdmin)
            {
                return string.Format("{0}/{1}", confirmEmailBaseURLAdmin, token);
            }
            else
            {
                return string.Format("{0}?{1}", confirmEmailBaseURLClient, token);
            }
        }

        public static string SetConfirmPhoneURL(string token, bool isAdmin)
        {
            if (isAdmin)
            {
                return string.Format("{0}/{1}", confirmPhoneBaseURLAdmin, token);
            }
            else
            {
                return string.Format("{0}?{1}", confirmPhoneBaseURLClient, token);
            }
        }

        public static string DecodeCodeToEmailToken(string code)
        {
            string decode = HttpUtility.UrlDecode(code);
            int userIdStartIndex = decode.IndexOf("=") + 1;
            int userIdEndIndex = decode.IndexOf("&");
            int tokenStartIndex = decode.IndexOf("n=") + 2;
            string userEmail = decode.Substring(userIdStartIndex, userIdEndIndex - userIdStartIndex);
            string token = decode.Substring(tokenStartIndex);
            return token;
        }

        public static string SetResetPasswordURL(string token, bool isAdmin)
        {
            if (isAdmin)
            {
                return string.Format("{0}/{1}", resetPasswordBaseURLAdmin, token);
            }
            else
            {
                return string.Format("{0}/{1}", resetPasswordBaseURLClient, token);
            }
        }

        /// <summary>
        /// Convert UserId to ciphered string.
        /// </summary>
        /// <param name="aID"></param>
        /// <returns></returns>
        static string Cipher(int aID)
        {
            string tempString = (aID + ADDEND).ToString();
            tempString = StringReverse(tempString);

            string cipheredString = String.Empty;
            for (int i = 0; i < tempString.Length; i++)
            {
                int index = Int16.Parse(tempString[i].ToString());
                cipheredString += GetString(index);
            }

            return cipheredString;
        }

        /// <summary>
        /// Convert string to original userID
        /// </summary>
        /// <param name="aText"></param>
        /// <returns></returns>
        static string DeCipher(string aText)
        {
            string tempString = String.Empty;
            for (int i = 0; i < aText.Length; i++)
            {
                string temp = GetNumber(aText[i].ToString());
                tempString += temp;
            }
            string DeCipheredText = StringReverse(tempString);
            long userId = Int32.Parse(DeCipheredText) - ADDEND;
            DeCipheredText = userId.ToString();

            return DeCipheredText;
        }

        /// <summary>
        /// Reverse string
        /// </summary>
        /// <param name="aText"></param>
        /// <returns></returns>
        static string StringReverse(string aText)
        {
            string reversedString = String.Empty;
            for (int i = aText.Length - 1; i >= 0; i--)
            {
                reversedString += aText[i];
            }
            return reversedString;
        }

        /// <summary>
        /// Convert number to string
        /// </summary>
        /// <param name="aIndex"></param>
        /// <returns></returns>
        static string GetString(int aIndex)
        {
            return CUSTOM_ALPHABET[aIndex].ToString();
        }

        /// <summary>
        /// Convert string to number
        /// </summary>
        /// <param name="aText"></param>
        /// <returns></returns>
        static string GetNumber(string aText)
        {
            string stringIndex = CUSTOM_ALPHABET.IndexOf(aText).ToString();
            return stringIndex;
        }
    }
}