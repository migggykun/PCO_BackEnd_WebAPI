using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    public partial class DailyAttendanceRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int RegistrationId { get; set; }

        [Column(TypeName = "date")]
        public DateTime AttendanceDate { get; set; }

        public int PeriodId { get; set; }

        public virtual Registration Registration { get; set; }
    }
}
