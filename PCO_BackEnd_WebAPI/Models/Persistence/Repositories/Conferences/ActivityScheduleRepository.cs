using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Conferences
{
    public class ActivityScheduleRepository : Repository<ActivitySchedule>, IActivityScheduleRepository
    {
        public ActivityScheduleRepository(DbContext context)
           : base(context)
        {

        }
        public ActivitySchedule GetActivitySchedule(string activityScheduleId)
        {
            throw new NotImplementedException();
        }

        public PageResult<ActivitySchedule> GetPagedActivitySchedules(int page, int size, string filter = null, string day = null, string month = null, string year = null, string fromDate = null, string toDate = null)
        {
            PageResult<ActivitySchedule> pageResult = new PageResult<ActivitySchedule>();
            IQueryable<ActivitySchedule> queryResult = appDbContext.ActivitySchedules;

            pageResult = PaginationManager<ActivitySchedule>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public ActivitySchedule UpdateActivitySchedule(int id, ActivitySchedule activitySchedule)
        {
            var activityScheduleToUpdate = appDbContext.ActivitySchedules.Find(id);
            appDbContext.Entry(activityScheduleToUpdate).CurrentValues.SetValues(activitySchedule);
            return activityScheduleToUpdate;
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