using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PCO_BackEnd_WebAPI.Models.Conferences;
using System.Data.Entity;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ConferenceRepository : Repository<Conference>
    {
        public ConferenceRepository(DbContext context) : base(context)
        {
              
        }
    }
}