using Newtonsoft.Json;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PCO_BackEnd_WebAPI.DTOs.Conferences
{
    public class AddConferenceDTO : BaseConferenceDTO
    {
        [Required]
        public virtual ICollection<AddRateWithConferenceDTO> Rates { get; set; }
    }
}