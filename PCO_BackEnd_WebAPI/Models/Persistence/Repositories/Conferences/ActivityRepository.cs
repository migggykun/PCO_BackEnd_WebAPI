using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ActivityRepository : Repository<Activity>, IActivityRepository
    {
        public ActivityRepository(DbContext context)
            : base(context)
        {
              
        }

        public PageResult<Activity> GetPagedActivity(int page, int size)
        {
            PageResult<Activity> pageResult = new PageResult<Activity>();
            IQueryable<Activity> queryResult = appDbContext.Activities;

            pageResult = PaginationManager<Activity>.GetPagedResult(queryResult, page, size);
            return pageResult;

        }

        public List<Activity> AddRates(List<Activity> activity)
        {
            return appDbContext.Activities.AddRange(activity).ToList();
        }

        public Activity UpdateRate(int id, Activity activity)
        {
            var activityToUpdate = appDbContext.Activities.Find(id);
            appDbContext.Entry(activityToUpdate).CurrentValues.SetValues(activity);
            return activityToUpdate;
        }

        public void RemoveRates(List<Activity> rates)
        {
            appDbContext.Activities.RemoveRange(rates);
        }

        public Activity GetRate(Expression<Func<Activity, bool>> predicate)
        {
            return appDbContext.Activities.FirstOrDefault(predicate);
        }

        private ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext; 
            }
        }

        public PageResult<Activity> GetPagedActivities(int page, int size)
        {
            PageResult<Activity> pageResult = new PageResult<Activity>();
            IQueryable<Activity> queryResult = appDbContext.Activities;

            pageResult = PaginationManager<Activity>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }
    }
}