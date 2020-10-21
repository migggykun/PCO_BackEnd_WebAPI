using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class ResponseRateDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int conferenceId { get; set; }

        [Required]
        public int membershipTypeId { get; set; }

        [Required]
        public double regularPrice { get; set; }
        [Required]
        public int? conferenceActivityId { get; set; }
    }
}