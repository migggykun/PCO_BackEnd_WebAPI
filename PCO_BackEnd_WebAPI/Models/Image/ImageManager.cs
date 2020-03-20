using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace PCO_BackEnd_WebAPI.Models.Image
{
    public class ImageManager
    {
        public string UploadImage(string imageCode)
        {
            Model obj;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.imgur.com/3/upload");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers["Authorization"] = "Bearer 47854a93751229523fb118c081355c31a5c1fce8";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{" + string.Format("\"{0}\":\"{1}\"", "image", imageCode) + "}";

                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                obj = JsonConvert.DeserializeObject<Model>(result.ToString());
            }
            return obj.Data.link;
        }
    }
}