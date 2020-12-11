using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Accounts;
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
using PCO_BackEnd_WebAPI.Models.Helpers;

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
                        TimeIn = attendance.TimeIn == null ? PhTime.Now() : attendance.TimeIn
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
                        TimeOut = PhTime.Now()
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

        [HttpGet]
        [Route("api/GetActivityAttendanceReport")]
        public async Task<IHttpActionResult> GetActivityAttendanceReport(int conferenceId, int conferenceActivityId)
        {
            List<ActivityAttendanceReport> ActivityAttendanceReport = new List<ActivityAttendanceReport>();

            await Task.Run(()=> ActivityAttendanceReport.AddRange(getActivityAttendanceReport(conferenceId, conferenceActivityId)));

            return Ok(Mapper.Map<List<ActivityAttendanceReport>, List<ActivityAttendanceReport>>(ActivityAttendanceReport));
        }

        [HttpGet]
        [Route("api/GetConferenceAttendanceHistory")]
        public async Task<IHttpActionResult> GetConferenceAttendanceHistory(int userId, int page = 0, int size = 0, string filter = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            PageResult<ConferenceAttendanceHistory> pageResult;
            List<ConferenceAttendanceHistory> conferenceAttendanceHistory = new List<ConferenceAttendanceHistory>();
            List<Conference> conferences = await Task.Run(() => unitOfWork.Conferences.GetAll().ToList());
            
            foreach(Conference c in conferences)
            {
                List<ConferenceAttendanceReport> conferenceAttendanceReports = getConferenceAttendanceReport(c.Id).Where(x => x.UserId == userId).ToList();
                foreach(var report in conferenceAttendanceReports)
                {
                    ConferenceAttendanceHistory conferenceAttendanceHistoryItem = new ConferenceAttendanceHistory();
                    conferenceAttendanceHistoryItem.ConferenceId = c.Id;
                    conferenceAttendanceHistoryItem.ConferenceName = c.Title;
                    conferenceAttendanceHistoryItem.ActivityDate = report.ActivityDate;
                    conferenceAttendanceHistoryItem.ActivityName = report.ActivityName;
                    conferenceAttendanceHistoryItem.TimeIn = report.TimeIn;
                    conferenceAttendanceHistoryItem.TimeOut = report.TimeOut;
                    conferenceAttendanceHistoryItem.RegistrationStatus = report.RegistrationStatus;
                    conferenceAttendanceHistory.Add(conferenceAttendanceHistoryItem);
                }
            }

            IQueryable<ConferenceAttendanceHistory> queryResult = conferenceAttendanceHistory.AsQueryable().Where(b => string.IsNullOrEmpty(filter) ? true :
                                                                                b.ConferenceName.Contains(filter) ||
                                                                                b.ActivityName.Contains(filter));

            pageResult = PaginationManager<ConferenceAttendanceHistory>.GetPagedResult(queryResult, page, size);
            return Ok(pageResult);
        }

        [HttpGet]
        [Route("api/GetConferenceAttendanceReport")]
        public async Task<IHttpActionResult> GetConferenceAttendanceReport(int conferenceId)
        {
            var result = await Task.Run(() => getConferenceAttendanceReport(conferenceId));
            return Ok(Mapper.Map<List<ConferenceAttendanceReport>, List<ConferenceAttendanceReport>>(result));
        }
        private List<ConferenceAttendanceReport> getConferenceAttendanceReport(int conferenceId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            //Prepare activity attendance report object
            ConferenceAttendanceReport reportItem = new ConferenceAttendanceReport();
            List<ConferenceAttendanceReport> ConferenceAttendanceReports = new List<ConferenceAttendanceReport>();

            //get conference
            Conference conference = unitOfWork.Conferences.Get(conferenceId);
            if(conference == null)
            {
                return null;
            }
            foreach (var cd in conference.ConferenceDays)
            {
                foreach (var ca in cd.ConferenceActivities)
                {
                    reportItem = new ConferenceAttendanceReport();
                    List<ActivityAttendanceReport> ActivityAttendanceReports = getActivityAttendanceReport(conferenceId, ca.Id);
                    foreach (var a in ActivityAttendanceReports)
                    {
                        reportItem.ActivityDate = cd.Date;
                        reportItem.ActivityName = ca.ActivitySchedule.Activity.Name;

                        reportItem.Amount = a.Amount;
                        reportItem.Discount = a.Discount;
                        reportItem.isBundle = a.isBundle;
                        reportItem.PRCExpiration = a.PRCExpiration;
                        reportItem.PRCId = a.PRCId;
                        reportItem.RegistrationStatus = a.RegistrationStatus;
                        reportItem.TimeIn = a.TimeIn;
                        reportItem.TimeOut = a.TimeOut;
                        reportItem.UserId = a.UserId;
                        reportItem.UserName = a.UserName;

                        ConferenceAttendanceReports.Add(reportItem);
                    }
                }

            }
            return ConferenceAttendanceReports;
        }
        private List<ActivityAttendanceReport> getActivityAttendanceReport(int conferenceId, int conferenceActivityId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            //Prepare activity attendance report object
            ActivityAttendanceReport reportItem = new ActivityAttendanceReport();
            List<ActivityAttendanceReport> ActivityAttendanceReport = new List<ActivityAttendanceReport>();

            string[] regStatus = { null, "Unpaid", "Pending", "Paid", "Declined", "Cancelled" };

            //get registrations + attendance for conferenceactivity
            IEnumerable<Registration> regs = unitOfWork.Registrations.GetAll();
            List<Registration> registrations = new List<Registration>();
            if (regs.Count() > 0){ 
                registrations = regs.Where(x => x.ConferenceId == conferenceId).ToList();

                foreach (Registration r in registrations)
                {
                    reportItem = new ActivityAttendanceReport();
                    if (r.IsBundle == false)
                    {
                        foreach (var att in r.ActivitiesToAttend)
                        {
                            if (att.ConferenceActivityId == conferenceActivityId)
                            {
                                //log
                                ActivityAttendance aa = unitOfWork.ActivityAttendances.Find(r.UserId, att.ConferenceActivityId);
                                reportItem.isBundle = r.IsBundle;
                                UserInfo ui = unitOfWork.UserInfos.Get(r.UserId);
                                PRCDetail prc = unitOfWork.PRCDetails.GetPRCDetailById(r.UserId.ToString());
                                reportItem.Amount = r.Amount;
                                reportItem.Discount = r.Discount;
                                reportItem.RegistrationStatus = regStatus[r.RegistrationStatusId];
                                reportItem.UserId = r.UserId;
                                reportItem.UserName = ui.FirstName + " " + ui.LastName;
                                if (prc != null) reportItem.PRCId = prc.IdNumber;
                                if (prc != null) reportItem.PRCExpiration = prc.ExpirationDate;
                                if (aa != null) reportItem.TimeIn = aa.TimeIn;
                                if (aa != null) reportItem.TimeOut = aa.TimeOut;
                                ActivityAttendanceReport.Add(reportItem);
                            }
                        }
                    }
                    else
                    {
                        List<ActivityAttendance> aaa = unitOfWork.ActivityAttendances.GetAll().ToList();
                        ActivityAttendance aa = aaa.Find(x => x.ConferenceActivityId == conferenceActivityId);
                        reportItem.isBundle = r.IsBundle;
                        UserInfo ui = unitOfWork.UserInfos.Get(r.UserId);
                        PRCDetail prc = unitOfWork.PRCDetails.Get(r.UserId);
                        reportItem.Amount = r.Amount;
                        reportItem.Discount = r.Discount;
                        reportItem.RegistrationStatus = regStatus[r.RegistrationStatusId];
                        reportItem.UserId = r.UserId;
                        reportItem.UserName = ui.FirstName + " " + ui.LastName;
                        if (prc != null) reportItem.PRCId = prc.IdNumber;
                        if (prc != null) reportItem.PRCExpiration = prc.ExpirationDate;
                        if (aa != null) reportItem.TimeIn = aa.TimeIn;
                        if (aa != null) reportItem.TimeOut = aa.TimeOut;
                        ActivityAttendanceReport.Add(reportItem);
                    }
                } 
            }
            return ActivityAttendanceReport;
        }
    }
}