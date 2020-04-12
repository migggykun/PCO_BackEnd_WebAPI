using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences.Promos
{
    public class RequestPromoMemberDTO
    {
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed.")]
        public int MembershipTypeId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed.")]
        public int PromoId { get; set; }
    }
}