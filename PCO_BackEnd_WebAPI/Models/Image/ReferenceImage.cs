using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Drawing;

namespace PCO_BackEnd_WebAPI.Models.Images
{
    public class ReferenceImage : ImageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String">base64string format of image</param>
        public ReferenceImage(string base64String)
        {
            Base64Value = base64String;
            byte[] imgArray = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imgArray, 0, imgArray.Length))
            {
                var img = Image.FromStream(ms, true);
                Image = CreateNewImage(img);
            }
        }

        public ReferenceImage(Image img)
        {
            Image = CreateNewImage(img);
        }

        public void ReduceImageSize()
        {
            double scaleFactor = .9;
            do
            {
                //Resize Image
                var newWidth = (int)(Image.Width * scaleFactor);
                var newHeight = (int)(Image.Height * scaleFactor);
                Bitmap newBitMap = new Bitmap(newWidth, newHeight);
                Image = CreateNewImage(newBitMap);
                scaleFactor -= .01;
            }
            while (FileSize > 100000);
        }
    }
}