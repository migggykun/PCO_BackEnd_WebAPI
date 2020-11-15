using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
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

        //[HttpPost]
        //[Route("api/UpdateAttendance/{id:int}")]
        //public async Task<IHttpActionResult> UpdateAttendance(int id, ActivityAttendance attendanceDTO)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
        //        return BadRequest(errorMessages);
        //    }

        //    var activityAttendance = Mapper.Map<ActivityAttendance, ActivityAttendance>(attendanceDTO);
        //    try
        //    {
        //        UnitOfWork unitOfWork = new UnitOfWork(_context);
        //        var result = await Task.Run(() => unitOfWork.ActivityAttendances.Get(id));
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            result = await Task.Run(() => unitOfWork.ActivityAttendances.UpdateAttendance(id, activityAttendance));
        //            await Task.Run(() => unitOfWork.Complete());
        //            return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ErrorManager.GetInnerExceptionMessage(ex);
        //        return BadRequest(message);
        //    }
        //}

        [HttpPost]
        [Route("api/TimeIn")]
        public async Task<IHttpActionResult> TimeIn(int registrationId, int conferenceActivityId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                Registration registration = await Task.Run(() => unitOfWork.Registrations.Get(registrationId));

                // check if user is registered          
                if(registration == null)
                {
                    return NotFound();
                }

                //check if user is registered as bundle
                bool isBundle = registration.IsBundle;
                //check if user is registered to activity
                if (!isBundle)
                {
                    bool registeredToActivity = registration.ActivitiesToAttend.ToList().Exists(x => x.ConferenceActivityId == conferenceActivityId);
                    if(!registeredToActivity)
                    {
                        return NotFound();
                    }
                }

                //check if attendance record is existing, then time in
                ActivityAttendance attendance = await Task.Run(() => (unitOfWork.ActivityAttendances.Find(registration.UserId, conferenceActivityId)));             
                if(attendance == null)
                {
                    ActivityAttendance newAttendance = new ActivityAttendance()
                    {
                        ConferenceActivityId = conferenceActivityId,
                        UserId = registration.UserId,
                        TimeIn = DateTime.Now
                    };
                    var result = newAttendance;
                    await Task.Run(() => unitOfWork.ActivityAttendances.Add(newAttendance));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
                }
                else
                {
                    var result = await Task.Run(() => unitOfWork.ActivityAttendances.TimeIn(attendance.Id));
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
        [Route("api/TimeOut")]
        public async Task<IHttpActionResult> TimeOut(int registrationId, int conferenceActivityId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                Registration registration = await Task.Run(() => unitOfWork.Registrations.Get(registrationId));

                // check if user is registered          
                if (registration == null)
                {
                    return NotFound();
                }

                //check if user is registered as bundle
                bool isBundle = registration.IsBundle;
                //check if user is registered to activity
                if (!isBundle)
                {
                    bool registeredToActivity = registration.ActivitiesToAttend.ToList().Exists(x => x.ConferenceActivityId == conferenceActivityId);
                    if (!registeredToActivity)
                    {
                        return NotFound();
                    }
                }

                //check if attendance record is existing, then time out
                ActivityAttendance attendance = await Task.Run(() => (unitOfWork.ActivityAttendances.Find(registration.UserId, conferenceActivityId)));
                if (attendance == null)
                {
                    ActivityAttendance newAttendance = new ActivityAttendance()
                    {
                        ConferenceActivityId = conferenceActivityId,
                        UserId = registration.UserId,
                        TimeOut = DateTime.Now
                    };
                    var result = newAttendance;
                    await Task.Run(() => unitOfWork.ActivityAttendances.Add(newAttendance));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<ActivityAttendance, ActivityAttendance>(result));
                }
                else
                {
                    var result = await Task.Run(() => unitOfWork.ActivityAttendances.TimeOut(attendance.Id));
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