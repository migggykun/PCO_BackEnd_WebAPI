using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Registrations
{
    public class RequestRegistrationDTO
    {
        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed!")]
        public int UserId { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Characters are not allowed!")]
        public int ConferenceId { get; set; }

        //[Required]
        public bool IsBundle { get; set; }

        public virtual ICollection<RequestActivitiesToAttendDTO> ActivitiesToAttend { get; set; }

        public int? PromoId { get; set; }

        public double? Amount { get; set; }

        public double? Discount { get; set; }

        
    }
}