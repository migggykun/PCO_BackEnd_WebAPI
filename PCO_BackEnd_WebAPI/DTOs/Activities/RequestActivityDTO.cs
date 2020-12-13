using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.DTOs.Activities
{
    public class RequestActivityDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int? CPDUnits { get; set; }

        public string CPDAccreditationNumber { get; set; }
    }
}