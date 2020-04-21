using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Bank
{
    public class BankDetail
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountNumber { get; set; }

        public string Branch { get; set; }
    }
}