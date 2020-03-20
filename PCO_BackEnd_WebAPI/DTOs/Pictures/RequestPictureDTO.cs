using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Pictures
{
    public class RequestPictureDTO
    {
        [Required]
        public string image { get; set; }
    }
}