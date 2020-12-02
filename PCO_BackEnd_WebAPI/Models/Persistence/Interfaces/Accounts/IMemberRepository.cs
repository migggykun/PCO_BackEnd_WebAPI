using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts
{
    public interface IMemberRepository : IRepository<Member>
    {
        PageResult<Member> GetPageMembers(int page, int size);
        List<Member> AddMembers(List<Member> activities);
        Member UpdateMember(int userId, Member member);
        void RemoveMember(List<Member> members);
        Member GetMemberByUserId(int userId);
    }
}
