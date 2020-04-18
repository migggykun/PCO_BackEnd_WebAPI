﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;

namespace PCO_BackEnd_WebAPI.Models.Images
{
    public abstract class ImageBase
    {
        private Image _image;
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = SetImage(value);
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

        private string _mimeType;
        public string MimeType
        {
            get
            {
                return GetMIMEType();
            }
            set
            {
                _mimeType = value;
            }
        }

        private string _base64Value;
        public string Base64Value
        {
            get
            {
                return ConvertToBase64(Byte);
            }
            set
            {
                _base64Value = value;
            }
        }

        private Image SetImage(Image image)
        {
            Image imageWithFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                CreateNewImage(image).Save(ms, ImageTypeManager.GetImageFormat(_mimeType));
                imageWithFormat = Image.FromStream(ms);
            }
            return imageWithFormat;
        }

        private string ConvertToBase64(byte[] arrayValue)
        {
            _base64Value = Convert.ToBase64String(arrayValue);
            return _base64Value;
        }
        private byte[] GetByte()
        {
            byte[] result;
            using (MemoryStream ms = new MemoryStream())
            {
                CreateNewImage(_image).Save(ms, ImageTypeManager.GetImageFormat(_mimeType));
                result = ms.ToArray();
            }
            return result;
        }

        private double GetFileSize()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(Byte, 0, Byte.Length);
                return ms.Length;
            }
        }

        private string GetMIMEType()
        {
            return _image.RawFormat.GetImageMIMEType();
        }

        private Image CreateNewImage(Image image)
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