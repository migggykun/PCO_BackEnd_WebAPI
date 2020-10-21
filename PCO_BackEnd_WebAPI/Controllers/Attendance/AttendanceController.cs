using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Attendance
{
    public class AttendanceController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        [Route("api/UpdateAttendance/{id:int}")]
        public async Task<IHttpActionResult> UpdateAttendance(int id, ActivityAttendance attendanceDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var activityAttendance = Mapper.Map<ActivityAttendance, ActivityAttendance>(attendanceDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.ActivityAttendances.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.ActivityAttendances.UpdateAttendance(id, activityAttendance));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
        [HttpPost]
        [Route("api/TimeIn/{id:int}")]
        public async Task<IHttpActionResult> TimeIn(int id)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.ActivityAttendances.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.ActivityAttendances.TimeIn(id));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("api/TimeOut/{id:int}")]
        public async Task<IHttpActionResult> TimeOut(int id)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.ActivityAttendances.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.ActivityAttendances.TimeOut(id));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}