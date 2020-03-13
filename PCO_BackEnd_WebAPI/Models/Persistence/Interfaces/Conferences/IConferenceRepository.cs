﻿using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IConferenceRepository : IRepository<Conference>
    {
        Conference GetConferenceByTitle(string title);
        Conference UpdateConferenceInfo(Conference conference);
    }
}
