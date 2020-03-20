using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;


namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories
{
    public class ConferenceRegistrationRepository : Repository<Registration>, IConferenceRegistrationRepository
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