using PCO_BackEnd_WebAPI.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponseListRegistrationDTO
    {
        public int Id { get; set; }

        public int ConferenceId { get; set; }

        public int RegistrationStatusId { get; set; }

        public int?  PromoId { get; set; }

        public ResponseAccountDTO User { get; set; }
    }
}