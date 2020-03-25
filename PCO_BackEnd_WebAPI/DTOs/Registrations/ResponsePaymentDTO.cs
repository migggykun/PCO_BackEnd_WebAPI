using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class ResponsePaymentDTO
    {
        public int RegistrationId { get; set; }

        public string TransactionNumber { get; set; }

        public DateTime PaymentSubmissionDate { get; set; }

        public int AmountPaid { get; set; }

        public string ProofOfPayment { get; set; }

        public string ConfirmationDate { get; set; }

        public string Remarks { get; set; }

        public ResponseUserInfoDTO UserInfo { get; set; }

        public ResponseConferenceDTO Conference { get; set; }


    }
}