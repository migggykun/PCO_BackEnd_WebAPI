using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Accounts;

namespace PCO_BackEnd_WebAPI.Models.Conferences.Promos
{
    public partial class PromoMember
    {
        public int promoMemberId { get; set; }

        public int membershipTypeId { get; set; }

        public int? promoId { get; set; }
    }
}
