using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class PromoDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public virtual ICollection<PromoMemberDTO> PromoMembers { get; set; }
    }
}