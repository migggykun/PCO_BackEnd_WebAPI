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

        public PageResult<Registration> GetPagedRegistration(int page, int size, int? filter = null)
        {
            PageResult<Registration> pageResult = new PageResult<Registration>();

            int offset = size * (page - 1);
            var recordCount = appDbContext.Conferences.Count();
            var mod = recordCount % size;
            var totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);

            pageResult.RecordCount = recordCount;
            pageResult.PageCount = totalPageCount;
            pageResult.Results =   appDbContext.Registrations.Where(c => filter == null ? true : c.ConferenceId == filter)
                                               .OrderBy(r => r.Id)
                                               .Skip(offset)
                                               .Take(size)
                                               .ToList();
            return pageResult;

        }

        public List<Registration> Add(List<Registration> aRegistrationList)
        {
            return appDbContext.Registrations.AddRange(aRegistrationList).ToList();
        }

        public Registration Update(int id, Registration aRegistration)
        {
            aRegistration.Id = id;
            return appDbContext.UpdateGraph<Registration>(aRegistration);
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