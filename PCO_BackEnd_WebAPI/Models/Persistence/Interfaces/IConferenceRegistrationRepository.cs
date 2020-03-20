using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Registrations;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces
{
    public interface IConferenceRegistrationRepository : IRepository<Registration>
    {
        List<Registration> Add(List<Registration> aRegistrationList);
        Registration Update(int id, Registration aRegistration);
        void Remove(List<Registration> aRegistrationList);
        void SetRegistrationStatus(int id, int status);
    }
}
