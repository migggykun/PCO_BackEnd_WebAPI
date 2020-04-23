using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Images.Manager
{
    public class ImageManager
    {
        private ReferenceImage referenceImage = null;
        public string base64Value
        {
            get
            {
                return GetBase64();
            }
        }
        public byte[] Bytes
        { 
            get
            {
                return GetBytes();
            }
        }

        public ImageManager(string base64Value)
        {
            referenceImage = new ReferenceImage(base64Value);
        }

        public ImageManager(byte[] array)
        {
            referenceImage = new ReferenceImage(array);
        }


        private void ReduceImageSize()
        {
            referenceImage.ReduceImageSize();
        }

        private byte[] GetBytes()
        {
            return referenceImage.Byte;
        }

        private string GetBase64()
        {
            return referenceImage.Base64Value;
        }
        
        /// <summary>
        /// Resizes image if size is greater than 100kb
        /// </summary>
        /// <returns></returns>
        public byte[] GetAdjustedSizeInBytes()
        {
            if (referenceImage.FileSize > 100000)
            {
                ReduceImageSize();
                return this.Bytes;
            }
            else
            {
                return this.Bytes;
            }
        }

        public string GetImageFormat()
        {
            return string.Format("data:{0};base64,{1}", referenceImage.MimeType, referenceImage.Base64Value);
        }


    }
}