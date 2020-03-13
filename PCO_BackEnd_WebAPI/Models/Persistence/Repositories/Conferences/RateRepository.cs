using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(DbContext context)
            : base(context)
        {
              
        }

        public List<Rate> AddRates(List<Rate> rates)
        {
            return appDbContext.Rates.AddRange(rates).ToList();
        }

        public Rate UpdateRate(Rate rate)
        {
            return appDbContext.UpdateGraph<Rate>(rate);
        }

        public void RemoveRates(List<Rate> rates)
        {
            appDbContext.Rates.RemoveRange(rates);
        }
        public ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext; 
            }
        }
    }
}