using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class EmailTemplate
    {
        public  const string CONFIRM_EMAIL_HEADER = "Email Verification";
        public  const string RESET_PASSWORD_HEADER = "Password Reset";
        private const string CONFIRM_EMAIL_BODY = "To complete your email verification. Please click the button below to proceed.";
        private const string RESET_PASSWORD_BODY = "You've recently requested to reset your password.<br>Please click the button below to proceed.";
        private const string BUTTON_NAME_CONFIRMEMAIL = "Verify Email";
        private const string BUTTON_NAME_RESETPW = "Reset Password";
        
        public static string FormatConfirmEmailBody(string url)
        {
            string template = ReadEmailTemplate();
            string result = template.Replace("{header}", CONFIRM_EMAIL_HEADER)
                                    .Replace("{body}", CONFIRM_EMAIL_BODY)
                                    .Replace("{url}", url)
                                    .Replace("{button}", BUTTON_NAME_CONFIRMEMAIL);
            return result;
        }

        public static string FormatResetPasswordBody(string url)
        {
            string template = ReadEmailTemplate();
            string result = template.Replace("{header}", RESET_PASSWORD_HEADER)
                                    .Replace("{body}", RESET_PASSWORD_BODY)
                                    .Replace("{url}", url)
                                    .Replace("{button}", BUTTON_NAME_RESETPW);
            return result;
        }

        private static string ReadEmailTemplate()
        {
            string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);
            string emailTemplateFullPath= binPath + @"\Resources\emailTemplate.html";
            return File.ReadAllText(emailTemplateFullPath);
        }
    }
}