using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Drawing;

namespace PCO_BackEnd_WebAPI.Models.Images.Helpers
{
    public static class ImageTypeManager
    {
        public static ImageFormat GetImageFormat(string imageType)
        {
            if (imageType.Equals(ImageFormat.Jpeg.GetImageMIMEType()))
                return ImageFormat.Jpeg;
            if (imageType.Equals(ImageFormat.Bmp.GetImageMIMEType()))
                return ImageFormat.Bmp;
            if (imageType.Equals(ImageFormat.Png.GetImageMIMEType()))
                return ImageFormat.Png;
            if (imageType.Equals(ImageFormat.Emf.GetImageMIMEType()))
                return ImageFormat.Emf;
            if (imageType.Equals(ImageFormat.Exif.GetImageMIMEType()))
                return ImageFormat.Exif;
            if (imageType.Equals(ImageFormat.Gif.GetImageMIMEType()))
                return ImageFormat.Gif;
            if (imageType.Equals(ImageFormat.Icon.GetImageMIMEType()))
                return ImageFormat.Icon;
            if (imageType.Equals(ImageFormat.MemoryBmp.GetImageMIMEType()))
                return ImageFormat.MemoryBmp;
            if (imageType.Equals(ImageFormat.Tiff.GetImageMIMEType()))
                return ImageFormat.Tiff;
            if (imageType.Equals(ImageFormat.Wmf.GetImageMIMEType()))
                return ImageFormat.Wmf;
            else
            {
                return null;
            }
        }

        public static string GetImageMIMEType(this ImageFormat imgFormat)
        {
            var imageDecorders = ImageCodecInfo.GetImageDecoders();
            var codecInfo = imageDecorders.FirstOrDefault(i => i.FormatID == imgFormat.Guid);
            if (codecInfo != null)
            {
                return codecInfo.MimeType;
            }
            else
            {
                return "image/unknown";
            }
        }
    }
}