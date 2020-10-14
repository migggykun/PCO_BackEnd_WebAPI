using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Conferences.Activities
{
    [Table("pc0_Database_Staging.[dbo.Activities]")]
    public partial class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int  Id { get;  set;  }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}