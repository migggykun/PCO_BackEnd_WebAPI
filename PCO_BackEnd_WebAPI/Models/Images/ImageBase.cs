using System;
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
        protected MemoryStream ms{get ;set ;}

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

        private byte[] _Byte;
        public byte[] Byte
        {
            get
            {
                return GetByte();
            }
            set
            {
                _Byte = value;
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
                return _mimeType;
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
                return GetBase64();
            }
            set
            {
                _base64Value = value;
            }
        }
        
        public ImageBase ()
	    {
            ms = new MemoryStream();
    	}

        ~ImageBase()
        {
            ms.Dispose();
        }

        private Image SetImage(Image image)
        {
            Image imageWithFormat;
            image.Save(ms, ImageTypeManager.GetImageFormat(_mimeType));
            imageWithFormat = Image.FromStream(ms);
            return imageWithFormat;
        }

        private string GetBase64()
        {
            if (!string.IsNullOrEmpty(_base64Value))
            {
                return _base64Value;
            }
            else
            {
                return Convert.ToBase64String(_Byte);
            }
        }

        private byte[] GetByte()
        {
            if (_Byte != null)
            {
                return _Byte;
            }
            else
            {
                return Convert.FromBase64String(_base64Value);
            }
        }

        private double GetFileSize()
        {
            return Byte.Length;
        }

        private string GetMIMEType()
        {
            return _image.RawFormat.GetImageMIMEType();
        }

        protected Image CreateNewImage(Image image)
        {
            var rawImage = DrawImage(image);
            var imagewithFormat = GetImageWithFormat(rawImage);
            SetNewImageInfo(imagewithFormat);
            
            return imagewithFormat;
        }

        private Image GetImageWithFormat(Image newImage)
        {
            newImage.Save(ms, ImageTypeManager.GetImageFormat(_mimeType));
            return Image.FromStream(ms);
        }
        private void SetNewImageInfo(Image newImage)
        {
            _Byte = ms.ToArray();
            _base64Value = Convert.ToBase64String(_Byte);
        }

        private Image DrawImage(Image image)
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