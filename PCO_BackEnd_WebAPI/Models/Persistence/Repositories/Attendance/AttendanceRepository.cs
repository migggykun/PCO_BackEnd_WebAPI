using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Attendance;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Attendance
{
    public class AttendanceRepository : Repository<ActivityAttendance>, IAttendanceRepository
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

        public ActivityAttendance Find(int userId, int conferenceActivityId)
        {
            ActivityAttendance attendanceToUpdate = appDbContext.ActivityAttendances.ToList().Find(x => x.UserId == userId && x.ConferenceActivityId == conferenceActivityId);
            return attendanceToUpdate;
        }

        public ActivityAttendance TimeIn(int id)
        {
            var attendanceToUpdate = appDbContext.ActivityAttendances.Find(id);
            ActivityAttendance timeIn = new ActivityAttendance();
            timeIn.Id = attendanceToUpdate.Id;
            timeIn.UserId = attendanceToUpdate.UserId;
            timeIn.ConferenceActivityId = attendanceToUpdate.ConferenceActivityId;
            timeIn.TimeOut = attendanceToUpdate.TimeOut;
            if (attendanceToUpdate != null)
            {
                timeIn.TimeIn = attendanceToUpdate.TimeIn==null?PhTime.Now():attendanceToUpdate.TimeIn;
            }
            else
            {
                timeIn.TimeIn = attendanceToUpdate.TimeIn;
            }
                
            appDbContext.Entry(attendanceToUpdate).CurrentValues.SetValues(timeIn);
            return attendanceToUpdate;
        }

        public ActivityAttendance TimeOut(int id)
        {
            var attendanceToUpdate = appDbContext.ActivityAttendances.Find(id);
            ActivityAttendance timeOut = new ActivityAttendance();
            timeOut.Id = attendanceToUpdate.Id;
            timeOut.UserId = attendanceToUpdate.UserId;
            timeOut.ConferenceActivityId = attendanceToUpdate.ConferenceActivityId;
            timeOut.TimeIn = attendanceToUpdate.TimeIn;
            if (attendanceToUpdate != null)
            {
                timeOut.TimeOut = PhTime.Now();
            }
            else
            {
                timeOut.TimeOut = attendanceToUpdate.TimeOut;
            }
            
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
