using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences.Promos
{
    public class PromoMemberDTO
    {
        public int Id { get; set; }

        public int MembershipTypeId { get; set; }

        public int PromoId { get; set; }
    }
}