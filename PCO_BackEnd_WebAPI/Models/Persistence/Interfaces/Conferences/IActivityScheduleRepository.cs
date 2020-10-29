using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Conferences
{
    public interface IActivityScheduleRepository:IRepository<ActivitySchedule>
    {
        PageResult<ActivitySchedule> GetPagedActivitySchedules(int page, int size);
        ActivitySchedule UpdateActivitySchedule(int id, ActivitySchedule activitySchedule);
    }
}
