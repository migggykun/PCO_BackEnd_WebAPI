using Newtonsoft.Json;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class AddConferenceDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        public string Banner { get; set; }

        public int? PromoId { get; set; }

        [Required]
        public virtual ICollection<AddRateWithConferenceDTO> Rates { get; set; }
    }
}