using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Images
{
    public class ReceiptManager
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

        public ReceiptManager(string base64Value)
        {
            referenceImage = new ReferenceImage(base64Value);
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
    }
}