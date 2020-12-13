using PCO_BackEnd_WebAPI.DTOs.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponseListMemberRegistrationDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int MemberRegistrationStatusId { get; set; }

        public double? Amount { get; set; }

        [Required]
        public ResponseAccountDTO User { get; set; }
    }
}