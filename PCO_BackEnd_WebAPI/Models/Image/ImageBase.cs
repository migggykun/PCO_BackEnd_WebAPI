using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Images
{
    public abstract class ImageBase
    {
        private Image _image;
        public Image Image
        {
            get
            {
                return this._image;
            }
            set
            {
                this._image = value;
            }
        }

        public byte[] Byte
        {
            get
            {
                return GetByte();
            }
        }

        public double FileSize
        {
            get
            {
                return GetFileSize();
            }
        }

        private string _base64Value;
        public string Base64Value 
        {
            get 
            {
                return ConvertToBase64(this.Byte);
            }
            set 
            {
                _base64Value = value;
            }
        }

        private string ConvertToBase64(byte[] arrayValue)
        {
            this._base64Value = Convert.ToBase64String(arrayValue);
            return this._base64Value;
        }
        private byte[] GetByte()
        {
            using (var ms = new MemoryStream())
            {
                this._image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private double GetFileSize()
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                mStream.Write(this.Byte, 0, this.Byte.Length);
                return mStream.Length;
            }
        }

        protected Image CreateNewImage(Image image)
        {
            var oldImg = this._image == null ? image : this._image;
            Bitmap bitmapNew = new Bitmap(image.Width, image.Height);
            var graphics = Graphics.FromImage(bitmapNew);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(oldImg, new Rectangle(0, 0, bitmapNew.Width, bitmapNew.Height), 0, 0,oldImg.Width, oldImg.Height, GraphicsUnit.Pixel);
            return bitmapNew;
        }
    }
}