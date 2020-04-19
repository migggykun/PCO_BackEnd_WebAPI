using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Drawing;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;

namespace PCO_BackEnd_WebAPI.Models.Images
{
    public class ReferenceImage : ImageBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64String">base64string format of image</param>
        public ReferenceImage(string imageString) : base()
        {
            ImageInfo imageInfo = new ImageInfo(imageString);
            MimeType = imageInfo.MimeType;
            byte[] imgArray = Convert.FromBase64String(imageInfo.Base64String);

            using (var ms = new MemoryStream(imgArray, 0, imgArray.Length))
            {
                Image = Image.FromStream(ms, true);
            }
        }

        public ReferenceImage(byte[] imageInArray) : base()
        {
            using (MemoryStream ms = new MemoryStream(imageInArray, 0, imageInArray.Length))
            {
                var img = Image.FromStream(ms, true);
                MimeType = img.RawFormat.GetImageMIMEType();
                Image = img;
            }
        }
        public void ReduceImageSize()
        {
            double scaleFactor = .9;
            Image _image;
            do
            {
                //Resize Image
                var newWidth = (int)(Image.Width * scaleFactor);
                var newHeight = (int)(Image.Height * scaleFactor);
                Bitmap newBitMap = new Bitmap(newWidth, newHeight);
                Image = newBitMap;
                scaleFactor -= .01;
            }
            while (FileSize > 100000);
        }
    }
}