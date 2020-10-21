using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Attendance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Attendance
{
    public class AttendanceRepository : Repository<ActivityAttendance>, IAttendaceRepository
    {
        public AttendanceRepository(DbContext context)
            : base(context)
        {
        }
        public PageResult<ActivityAttendance> GetPagedAttendance(int page, int size)
        {
            PageResult<ActivityAttendance> pageResult = new PageResult<ActivityAttendance>();
            IQueryable<ActivityAttendance> queryResult = appDbContext.ActivityAttendances;

            pageResult = PaginationManager<ActivityAttendance>.GetPagedResult(queryResult, page, size);
            return pageResult;
        }

        public ActivityAttendance UpdateAttendance(int id, ActivityAttendance entityToUpdate)
        {
            var attendanceToUpdate = appDbContext.ActivityAttendances.Find(id);
            appDbContext.Entry(attendanceToUpdate).CurrentValues.SetValues(entityToUpdate);
            return attendanceToUpdate;
        }

        public ActivityAttendance TimeIn(int id)
        {
            var attendanceToUpdate = appDbContext.ActivityAttendances.Find(id);
            ActivityAttendance timeIn = attendanceToUpdate;
            timeIn.TimeIn = DateTime.Now;
            appDbContext.Entry(attendanceToUpdate).CurrentValues.SetValues(timeIn);
            return attendanceToUpdate;
        }

        public ActivityAttendance TimeOut(int id)
        {
            var attendanceToUpdate = appDbContext.ActivityAttendances.Find(id);
            ActivityAttendance timeOut = attendanceToUpdate;
            timeOut.TimeOut = DateTime.Now;
            appDbContext.Entry(attendanceToUpdate).CurrentValues.SetValues(timeOut);
            return attendanceToUpdate;
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
