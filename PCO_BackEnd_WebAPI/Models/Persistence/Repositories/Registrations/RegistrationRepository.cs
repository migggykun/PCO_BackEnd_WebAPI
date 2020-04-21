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
    public class ConferenceRegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public ConferenceRegistrationRepository(DbContext context)
            : base(context)
        {

        }

        public PageResult<Registration> GetPagedRegistration(int page, 
                                                             int size, 
                                                             int? filter = null,
                                                             int? aStatusId = null,
                                                             int? userId = null,
                                                             string akeywordFilter = null)
        {
            PageResult<Registration> pageResult;

            IQueryable<Registration> queryResult = appDbContext.Registrations.Where(c => akeywordFilter == null ?
                                                                true
                                                                :
                                                                c.User.Email.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.LastName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.FirstName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.MiddleName.Contains(akeywordFilter)
                                                               )
                                                        .Where(c => filter == null ? true : c.ConferenceId == filter)
                                                        .Where(c => aStatusId == null ? true : c.RegistrationStatusId == aStatusId);

            pageResult = PaginationManager<Registration>.GetPagedResult(queryResult, page, size, appDbContext.Registrations.Count());
            return pageResult;
        }

        public List<Registration> Add(List<Registration> aRegistrationList)
        {
            return appDbContext.Registrations.AddRange(aRegistrationList).ToList();
        }

        public Registration Update(Registration oldRegistration, Registration newRegistration)
        {
            newRegistration.Id = oldRegistration.Id;
            newRegistration.RegistrationStatusId = oldRegistration.RegistrationStatusId;
            return appDbContext.UpdateGraph<Registration>(newRegistration);
        }

        public void Remove(List<Registration> aRegistrationList)
        {
            appDbContext.Registrations.RemoveRange(aRegistrationList);
        }

        public void SetRegistrationStatus(int id, int status)
        {
            var reg = appDbContext.Registrations.Find(id);
            reg.RegistrationStatusId = status;

        }

        public int GetRegistrationStatus(int conferenceId, int userId)
        {
            Expression<Func<Registration, bool>> predicate;
            predicate = (x) => x.ConferenceId == conferenceId && x.UserId == userId;
            IQueryable<Registration> query = appDbContext.Registrations;

            if (query.Any(x => x.ConferenceId == conferenceId && x.UserId == userId))
            {
                return query.First(predicate).RegistrationStatusId;
            }
            else
            {
                return 0;
            }
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