using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
namespace PCO_BackEnd_WebAPI.Models.Images
{
    public class ImageManager
    {
        private ReferenceImage referenceImage = null;

        public ImageManager(string base64String)
        {
            referenceImage = new ReferenceImage(base64String);
        }

        public byte[] GetByteEquivalent()
        {
            return referenceImage.Byte;
        }

        public void ReduceImageSize()
        {
            referenceImage.ReduceImageSize();
        }

        public string Getbase64Value()
        {
            return referenceImage.Base64Value;
        }


    }
}