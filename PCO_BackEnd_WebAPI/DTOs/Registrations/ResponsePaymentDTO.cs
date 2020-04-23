using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponsePaymentDTO
    {
        [Required]
        public int RegistrationId { get; set; }

        [Required]
        public string TransactionNumber { get; set; }

        [Required]
        public DateTime PaymentSubmissionDate { get; set; }

        [Required]
        public int AmountPaid { get; set; }

        public string ConfirmationDate { get; set; }

        public string Remarks { get; set; }

        [Required]
        public ResponseUserInfoDTO UserInfo { get; set; }
       
        [Required]
        public ResponseConferenceDTO Conference { get; set; }


    }
}