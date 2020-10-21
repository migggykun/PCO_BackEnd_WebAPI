using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.Repositories;

namespace PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Attendance
{
    interface IAttendaceRepository: IRepository<ActivityAttendance>
    {
        PageResult<ActivityAttendance> GetPagedAttendance(int page, int size);
        ActivityAttendance UpdateAttendance(int id, ActivityAttendance entityToUpdate);
    }
}
