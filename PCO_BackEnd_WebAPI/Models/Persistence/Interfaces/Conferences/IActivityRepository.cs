using PCO_BackEnd_WebAPI.Models.Conferences.Activities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IActivityRepository : IRepository<Activity>
    {
        PageResult<Activity> GetPagedActivities(int page, int size);
        List<Activity> AddRates(List<Activity> activities);
        Activity UpdateRate(int id, Activity activity);
        void RemoveRates(List<Activity> rates);
        Activity GetRate(Expression<Func<Activity, bool>> predicate);
    }
}
