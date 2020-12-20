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
using PCO_BackEnd_WebAPI.Models.Comparer;

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
            rate.Id = id;
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

        public ICollection<Rate> UpdateRates(ICollection<Rate> rates, int cId)
        {
            //Update existing data
            foreach (var r in rates)
            {
                if(r.Id==0)
                {
                    //new rate
                    appDbContext.Rates.Add(r);
                }
                else
                {
                    var tempRate = appDbContext.Rates.Find(r.Id);
                    if (tempRate != null)
                    {
                        //Entity already exists in db
                        var attachedEntry = appDbContext.Entry(tempRate);
                        attachedEntry.CurrentValues.SetValues(r);
                    }
                    else
                    {
                        //Entity not yet exists in db
                        appDbContext.Rates.Add(r);
                    }
                }
            }

            //Delete data not in collection
            List<Rate> ratesIdFromDbList = new List<Rate>();
            foreach (var rate in appDbContext.Rates)
            {
                if(rate.conferenceId == cId && rate.conferenceActivityId == null)
                {
                    ratesIdFromDbList.Add(rate);
                }
            }

            List<Rate> ratesToDelete = ratesIdFromDbList.Except(rates, new GenericEqualityComparer<Rate>()).ToList();
            appDbContext.Rates.RemoveRange(ratesToDelete);

            return appDbContext.Rates.Where(x => x.conferenceId == cId) .Select(y => y).ToList();
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