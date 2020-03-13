using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class RateDTO
    {

        public int Id { get; set; }

        public int conferenceId { get; set; }

        public int membershipTypeId { get; set; }

        public int regularPrice { get; set; }
    }
}