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
    public interface IMemberRegistrationRepository : IRepository<MemberRegistration>
    {
        List<MemberRegistration> Add(List<MemberRegistration> aMemberRegistrationList);
        MemberRegistration Update(MemberRegistration oldMemberRegistration, MemberRegistration aMemberRegistration);
        void Remove(List<MemberRegistration> aMemberRegistrationList);
        MemberRegistration SetRegistrationStatus(int id, int status);
        PageResult<MemberRegistration> GetPagedMemberRegistration(int page, int size, int? aStatusId, int? userId, string akeywordFilter);
        MemberRegistration GetMemberRegistration(int userId);
    }
}
