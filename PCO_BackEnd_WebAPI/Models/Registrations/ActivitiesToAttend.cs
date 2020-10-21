using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public class ActivitiesToAttend
    {
        public int Id { get; set; }

        public int RegistrationId { get; set; }

        public int ConferenceActivityId { get; set; }

    }
}