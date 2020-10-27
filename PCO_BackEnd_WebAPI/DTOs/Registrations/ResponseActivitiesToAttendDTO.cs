using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponseActivitiesToAttendDTO
    {
        public int Id { get; set; }

        public int RegistrationId { get; set; }

        public int ConferenceActivityId { get; set; }
    }
}