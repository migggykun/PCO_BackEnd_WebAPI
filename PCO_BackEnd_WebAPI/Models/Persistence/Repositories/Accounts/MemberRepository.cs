using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Accounts;
using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Accounts
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        public PageResult<Member> GetPageMembers(int page, int size)
        {
            PageResult<Member> pageResult = new PageResult<Member>();
            IQueryable<Member> queryResult = appDbContext.Members;

            pageResult = PaginationManager<Member>.GetPagedResult(queryResult, page, size);
            return pageResult;

        }

        public Member UpdateMember(int id, Member member)
        {
            member.Id = id;
            return appDbContext.UpdateGraph<Member>(member);
        }

        public void RemoveMember(List<Member> members)
        {
            appDbContext.Members.RemoveRange(members);
        }

        private ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }

        public List<Member> AddMembers(List<Member> members)
        {
            return appDbContext.Members.AddRange(members).ToList();
        }

        Member IMemberRepository.GetMemberByUserId(int userId)
        {
            return  appDbContext.Members.ToList().Find(x => x.UserId == userId);           
        }
    }
}