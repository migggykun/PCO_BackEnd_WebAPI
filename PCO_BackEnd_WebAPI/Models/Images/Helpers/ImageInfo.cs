using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
namespace PCO_BackEnd_WebAPI.Models.Images.Helpers
{
    public class ImageInfo
    {
        private string _encodedString;
        public string MimeType
        {
            get
            {
                return GetMIMEType();
            }
        }


        public string Base64String
        {
            get
            {
                return GetBase64();
            }
        }

        public ImageInfo(string encodedString)
        {
            _encodedString = encodedString;
        }

        private string GetMIMEType()
        {
            string result = string.Empty;
            Regex regex = new Regex(@"(?<=data:)[a-zA-Z0-9/]+");
            var mime = regex.Match(this._encodedString);
            if (mime != null)
            {
                return mime.Value;
            }
            return result;
        }

        private string GetBase64()
        {
            var splittedStrings = this._encodedString.Split(',');
            string result = splittedStrings[1];
            return result;
        }
        
    }
}