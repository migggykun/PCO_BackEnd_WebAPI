using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class StringManipulationHelper
    {
        private const string confirmEmailBaseURL = "http://localhost:4200/confirm-email"; //http://pcosample.somee.com
        private const string resetPasswordBaseURL = "http://localhost:4200/reset-password";

        public static string EncodeIdTokenToCode(int id, string token)
        {
            return HttpUtility.UrlEncode(string.Format("?id={0}&token={1}", id, token));
        }

        public static string ConvertToHyperLink(string link)
        {
            return string.Format("<a href=\"{0}\">link</a>", link);
        }

        public static string SetConfirmEmailUrl(string token)
        {
            return string.Format("{0}/{1}", confirmEmailBaseURL, token);
        }

        public static KeyValuePair<int, string> DecodeCodeToIdToken(string code)
        {
            string decode = HttpUtility.UrlDecode(code);
            int userIdStartIndex = decode.IndexOf("=") + 1;
            int userIdEndIndex = decode.IndexOf("&");
            int tokenStartIndex = decode.IndexOf("n=") + 2;
            string userId = decode.Substring(userIdStartIndex, userIdEndIndex - userIdStartIndex);
            string token = decode.Substring(tokenStartIndex);
            return new KeyValuePair<int, string>(Convert.ToInt32(userId), token);
        }

        public static string SetResetPasswordURL(string token)
        {
           return string.Format("{0}/{1}", confirmEmailBaseURL, token);
        }
    }
}