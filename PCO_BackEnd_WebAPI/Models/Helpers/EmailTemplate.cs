using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class EmailTemplate
    {
        public  const string CONFIRM_EMAIL_HEADER = "Confirm your Email";
        public  const string RESET_PASSWORD_HEADER = "Recover your password";

        private const string confirmEmailBody =
                      @"<body>
                      <p>Hello!</p>
                      <p>We received a request to set up your account with us.
                         If this is correct, please confirm by clicking this {0}</p>
                      <br>
                      <br>
                      </body>
                      </html>";

        private const string resetPasswordBody =
                     @"<body>
                      <p>Hello!</p>
                      <p>We received a request to reset your password .
                         If this is correct, please confirm by clicking this {0}</p>
                      <br>
                      <br>
                      <p>Thank you</p>
                      </body>
                      </html>";


        public static string FormatConfirmEmailBody(string url)
        {
            return string.Format(confirmEmailBody, url);
        }

        public static string FormatResetPasswordBody(string url)
        {
            return string.Format(resetPasswordBody, url);
        }
    }
}