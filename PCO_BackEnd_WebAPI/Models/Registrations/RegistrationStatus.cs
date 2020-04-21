using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public class RegistrationStatus
    {
        [Key]
        public int Id { get; set; }

        public string StatusSlug { get; set; }

        public string StatusLabel { get; set; }
    }
}