using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Images.Helpers
{
    public static class ImageFormatter
    {
        public static string GetImageStringFormat(byte[] array)
        {
            return new ImageManager(array).GetImageFormat();
        }
    }
}