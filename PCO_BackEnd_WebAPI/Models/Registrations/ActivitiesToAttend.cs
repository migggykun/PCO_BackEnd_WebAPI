using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PCO_BackEnd_WebAPI.Models.Registrations
{
    [Table("pc0_Database_Staging.[dbo.ActivitiesToAttend]")]
    public class ActivitiesToAttend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int RegistrationId { get; set; }

        public int ConferenceActivityId { get; set; }

    }
}