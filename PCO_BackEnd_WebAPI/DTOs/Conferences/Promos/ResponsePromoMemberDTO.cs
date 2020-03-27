using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences.Promos
{
    public class ResponsePromoMemberDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int MembershipTypeId { get; set; }

        [Required]
        public int PromoId { get; set; }
    }
}