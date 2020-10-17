using PCO_BackEnd_WebAPI.Models.Conferences;
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
        List<Activity> AddActivities(List<Activity> activities);
        Activity UpdateActivity(int id, Activity activity);
        void RemoveActivity(List<Activity> activities);
        Activity GetActivity(Expression<Func<Activity, bool>> predicate);
    }
}
