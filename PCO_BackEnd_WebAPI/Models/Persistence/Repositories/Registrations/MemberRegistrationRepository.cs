using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System.Linq.Expressions;


namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
    public class MemberRegistrationRepository : Repository<MemberRegistration>, IMemberRegistrationRepository
    {
        public MemberRegistrationRepository(DbContext context)
            : base(context)
        {

        }

        public PageResult<MemberRegistration> GetPagedMemberRegistration(int page, 
                                                             int size, 
                                                             int? aStatusId = null,
                                                             int? userId = null,
                                                             string akeywordFilter = null)
        {
            PageResult<MemberRegistration> pageResult;

            IQueryable<MemberRegistration> queryResult = appDbContext.MemberRegistrations.Where(c => akeywordFilter == null ?
                                                                true
                                                                :
                                                                c.User.Email.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.LastName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.FirstName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.MiddleName.Contains(akeywordFilter)
                                                               )
                                                        .Where(c => aStatusId == null ? true : c.MemberRegistrationStatusId == aStatusId);

            pageResult = PaginationManager<MemberRegistration>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public List<MemberRegistration> Add(List<MemberRegistration> aMemberRegistrationList)
        {
            return appDbContext.MemberRegistrations.AddRange(aMemberRegistrationList).ToList();
        }

        public MemberRegistration Update(MemberRegistration oldMemberRegistration, MemberRegistration newMemberRegistration)
        {
            newMemberRegistration.Id = oldMemberRegistration.Id;
            newMemberRegistration.MemberRegistrationStatusId = oldMemberRegistration.MemberRegistrationStatusId;
            return appDbContext.UpdateGraph<MemberRegistration>(newMemberRegistration);
        }

        public MemberRegistration UpdateStatus(MemberRegistration oldMemberRegistration, MemberRegistration newMemberRegistration)
        {
            newMemberRegistration.Id = oldMemberRegistration.Id;
            newMemberRegistration.Amount = oldMemberRegistration.Amount;
            newMemberRegistration.User = oldMemberRegistration.User;
            newMemberRegistration.UserId = oldMemberRegistration.UserId;
            return appDbContext.UpdateGraph<MemberRegistration>(newMemberRegistration);
        }

        public void Remove(List<MemberRegistration> aMemberRegistrationList)
        {
            appDbContext.MemberRegistrations.RemoveRange(aMemberRegistrationList);
        }

        public MemberRegistration SetRegistrationStatus(int id, int status)
        {
            var reg = appDbContext.MemberRegistrations.Find(id);
            reg.MemberRegistrationStatusId = status;
            return reg;
        }

        public MemberRegistration GetMemberRegistration(int userId)
        {
            Expression<Func<MemberRegistration, bool>> predicate;
            int maxValidStatus = 3;
            predicate = (x) => x.UserId == userId && x.MemberRegistrationStatusId <= maxValidStatus;
            IQueryable<MemberRegistration> query = appDbContext.MemberRegistrations;

            return query.FirstOrDefault(predicate);
        }

        private ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}