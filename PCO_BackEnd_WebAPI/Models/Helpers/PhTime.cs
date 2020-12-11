using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Images.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Helpers
{
    public static class PhTime
    {
        public static DateTime Now()
        {
            string SST = "Singapore Standard Time";
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(SST);

            return TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);

        }
    }
}