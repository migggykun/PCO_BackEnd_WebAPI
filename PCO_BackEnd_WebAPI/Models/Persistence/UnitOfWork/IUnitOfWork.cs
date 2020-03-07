using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
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
        IMembershipTypeRepository MembershipTypes { get; set; }
        IPRCDetailRepository PRCDetails { get; set; }
        IUserInfoRepository UserInfos { get; set; }
        IMembershipAssignmentRepository MembershipAssignments { get; set; }

    }
}
