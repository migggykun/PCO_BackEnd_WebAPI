using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces
{
    public interface IRegistrationRepository : IRepository<Registration>
    {
        List<Registration> Add(List<Registration> aRegistrationList);
        Registration Update(Registration oldRegistration, Registration aRegistration);
        void Remove(List<Registration> aRegistrationList);
        void SetRegistrationStatus(int id, int status);
        PageResult<Registration> GetPagedRegistration(int page, int size, int? filter, int? aStatusId, int? userId, string akeywordFilter);
        Registration GetRegistration(int conferenceId, int userId);
        void UpdateRegistrationFee(int id, double amount, double discount);
    }
}
