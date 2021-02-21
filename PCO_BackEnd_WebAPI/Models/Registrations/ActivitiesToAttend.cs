﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    [Table("ActivitiesToAttend")]
    public class ActivitiesToAttend
    {
        [Key]
        public int Id { get; set; }

        public int RegistrationId { get; set; }

        public int ConferenceActivityId { get; set; }

    }
}