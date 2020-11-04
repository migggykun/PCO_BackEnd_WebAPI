using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using PCO_BackEnd_WebAPI.Models.Accounts;
namespace PCO_BackEnd_WebAPI.Models.Conferences
{
    public partial class Rate
    {
        [Key]
        public int Id { get; set; }

        public int conferenceId { get; set; }

        public int membershipTypeId { get; set; }

        public double regularPrice { get; set; }

        public int? conferenceActivityId { get; set; }
    
        public int? ActivityId { get; set; }
    }
}
