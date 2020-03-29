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


namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
    public class ConferenceRegistrationRepository : Repository<Registration>, IRegistrationRepository
    {
        public ConferenceRegistrationRepository(DbContext context)
            : base(context)
        {
              
        }

        public ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext; 
            }
        }

        public PageResult<Registration> GetPagedRegistration(int page, 
                                                             int size, 
                                                             int? filter = null,
                                                             int? aStatusId = null,
                                                             string akeywordFilter = null)
        {
            PageResult<Registration> pageResult = new PageResult<Registration>();

            int recordCount = appDbContext.Registrations.ToList()
                                                        .Where(c => akeywordFilter == null ? 
                                                                true 
                                                                :
                                                                c.User.Email.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.LastName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.FirstName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.MiddleName.Contains(akeywordFilter)
                                                               )
                                                        .Where(c => filter == null ? true : c.ConferenceId == filter)
                                                        .Where(c => aStatusId == null ? true : c.RegistrationStatusId == aStatusId)
                                                        .Count();
            int mod;
            int totalPageCount;
            int offset;
            int recordToReturn;
            if (size == 0)
            {
                mod = 0;
                totalPageCount = 1;
                offset = 0;
                recordToReturn = recordCount;
            }
            else
            {
                mod = recordCount % size;
                totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);
                offset = size * (page - 1);
                recordToReturn = size;
            }

            pageResult.Results =   appDbContext.Registrations
                                               .Where(c => akeywordFilter == null ?
                                                                true
                                                                :
                                                                c.User.Email.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.LastName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.FirstName.Contains(akeywordFilter) ||
                                                                c.User.UserInfo.MiddleName.Contains(akeywordFilter)
                                                               )
                                               .Where(c => filter == null ? true : c.ConferenceId == filter)
                                               .Where(c => aStatusId == null ? true : c.RegistrationStatusId == aStatusId)
                                               .OrderBy(r => r.Id)
                                               .Skip(offset)
                                               .Take(recordToReturn)
                                               .ToList();
            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
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
    }
}