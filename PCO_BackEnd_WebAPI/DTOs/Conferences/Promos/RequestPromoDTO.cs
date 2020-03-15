using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences.Promos
{
    public class RequestPromoDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int Price { get; set; }

        public List<int> MembershipTypeIds { get; set; }
    }
}