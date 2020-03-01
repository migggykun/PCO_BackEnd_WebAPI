using PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork
{
    interface IUnitOfWork : IDisposable
    {
        ConferenceRepository Conferences { get; set; }
    }
}
