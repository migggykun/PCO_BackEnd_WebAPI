using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
using System.Linq.Expressions;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class RateRepository : Repository<Rate>, IRateRepository
    {
        public RateRepository(DbContext context)
            : base(context)
        {
              
        }

        public PageResult<Rate> GetPagedRates(int page, int size)
        {
            PageResult<Rate> pageResult = new PageResult<Rate>();
            IQueryable<Rate> queryResult = appDbContext.Rates;

            pageResult = PaginationManager<Rate>.GetPagedResult(queryResult, page, size);
            return pageResult;

        }

        public List<Rate> AddRates(List<Rate> rates)
        {
            return appDbContext.Rates.AddRange(rates).ToList();
        }

        public Rate UpdateRate(int id, Rate rate)
        {
            var rateToUpdate = appDbContext.Rates.Find(id);
            appDbContext.Entry(rateToUpdate).CurrentValues.SetValues(rate);
            return rateToUpdate;
        }

        public void RemoveRates(List<Rate> rates)
        {
            appDbContext.Rates.RemoveRange(rates);
        }

        public Rate GetRate(Expression<Func<Rate, bool>> predicate)
        {
            return appDbContext.Rates.FirstOrDefault(predicate);
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