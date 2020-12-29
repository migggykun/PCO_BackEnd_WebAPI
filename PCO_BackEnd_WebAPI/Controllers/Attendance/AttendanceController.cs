using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Attendance
{
    /// <summary>
    /// Controller for Activity / Conference Attendance. Attendance Reports
    /// </summary>
    public class AttendanceController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. initialize database.
        /// </summary>
        public AttendanceController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Time in to activity. FILO (First in last out)
        /// </summary>
        /// <param name="registrationId">registration Id of Conference</param>
        /// <param name="conferenceActivityId">Id of activity to time in to</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
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
                        TimeIn = PhTime.Now() 
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

        /// <summary>
        /// Time Out to activity. FILO (First in last out)
        /// </summary>
        /// <param name="registrationId">registration Id of Conference</param>
        /// <param name="conferenceActivityId">Id of activity to time in to</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
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

        /// <summary>
        /// Get all attendance of specific activity
        /// </summary>
        /// <param name="conferenceId">id of conference</param>
        /// <param name="conferenceActivityId">id of activity of conference</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetActivityAttendanceReport")]
        public async Task<IHttpActionResult> GetActivityAttendanceReport(int conferenceId, int conferenceActivityId)
        {
            List<ActivityAttendanceReport> ActivityAttendanceReport = new List<ActivityAttendanceReport>();

            await Task.Run(()=> ActivityAttendanceReport.AddRange(getActivityAttendanceReport(conferenceId, conferenceActivityId)));

            return Ok(Mapper.Map<List<ActivityAttendanceReport>, List<ActivityAttendanceReport>>(ActivityAttendanceReport));
        }

        /// <summary>
        /// Get all attendance of specific user
        /// </summary>
        /// <param name="userId">id of user</param>
        /// <param name="page">count of pages for filter</param>
        /// <param name="size">size/count per page of search query</param>
        /// <param name="filter">string filter per item in query</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
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

                    var propInfo = report.GetType().GetProperties();
                    foreach (var item in propInfo)
                    {
                        if (item.CanWrite)
                        {
                            conferenceAttendanceHistoryItem.GetType().GetProperty(item.Name).SetValue(conferenceAttendanceHistoryItem, item.GetValue(report, null), null);
                        }
                    }

                    conferenceAttendanceHistory.Add(conferenceAttendanceHistoryItem);
                }
            }

            IQueryable<ConferenceAttendanceHistory> queryResult = conferenceAttendanceHistory.AsQueryable().Where(b => string.IsNullOrEmpty(filter) ? true :
                                                                                b.ConferenceName.Contains(filter) ||
                                                                                b.ActivityName.Contains(filter));

            pageResult = PaginationManager<ConferenceAttendanceHistory>.GetPagedResult(queryResult, page, size);
            return Ok(pageResult);
        }

        /// <summary>
        /// All attendance in conference
        /// </summary>
        /// <param name="conferenceId">id of conference.</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
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
                    List<ActivityAttendanceReport> ActivityAttendanceReports = new List<ActivityAttendanceReport>();
                    ActivityAttendanceReports = getActivityAttendanceReport(conferenceId, ca.Id);
                    if(ActivityAttendanceReports == null || ActivityAttendanceReports.Count<1)
                    {
                        continue;
                    }
                    foreach (var a in ActivityAttendanceReports)
                    {
                        reportItem = new ConferenceAttendanceReport();
                        reportItem.ActivityDate = cd.Date;
                        reportItem.ActivityName = ca.ActivitySchedule.Activity.Name;

                        var propInfo = a.GetType().GetProperties();
                        foreach (var item in propInfo)
                        {
                            if (item.CanWrite)
                            {
                                reportItem.GetType().GetProperty(item.Name).SetValue(reportItem, item.GetValue(a, null), null);
                            }
                        }

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
            List<MembershipType> memTypes = unitOfWork.MembershipTypes.GetAll().ToList();
            
            //get registrations + attendance for conferenceactivity
            List<Registration> registrations = unitOfWork.Registrations.GetAll().ToList();
            if (registrations!=null){   
                registrations = registrations.Where(x => x.ConferenceId == conferenceId).ToList();
                if(registrations!=null)
                {
                    Conference conference = unitOfWork.Conferences.Get(conferenceId);
                    foreach (Registration r in registrations)
                    {
                        ApplicationUser u = unitOfWork.Accounts.FindById(r.UserId);
                        UserInfo ui = u.UserInfo;
                        PRCDetail prc = u.PRCDetail;
                        reportItem = new ActivityAttendanceReport();
                        if (r.IsBundle == false)
                        {
                            if (r.ActivitiesToAttend!=null)
                            {
                                foreach (var att in r.ActivitiesToAttend)
                                {
                                    if (att.ConferenceActivityId == conferenceActivityId)
                                    {
                                        //log
                                        ActivityAttendance aa = unitOfWork.ActivityAttendances.Find(r.UserId, att.ConferenceActivityId);
                                        reportItem.isBundle = r.IsBundle;
                                        
                                        reportItem.Amount = r.Amount;
                                        reportItem.Discount = r.Discount;
                                        reportItem.RegistrationStatus = regStatus[r.RegistrationStatusId];
                                        reportItem.UserId = r.UserId;
                                        reportItem.UserName = ui.FirstName + " " + ui.LastName;
                                        if (prc != null) reportItem.PrcId = prc.IdNumber;
                                        if (prc != null) reportItem.PrcExpiration = prc.ExpirationDate;
                                        if (aa != null) reportItem.TimeIn = aa.TimeIn;
                                        if (aa != null) reportItem.TimeOut = aa.TimeOut;

                                        ConferenceActivity conferenceActivity = null;
                                        ConferenceDay conferenceDay = null;
                                        unitOfWork.Conferences.GetAll().ToList().ForEach(x=>x.ConferenceDays.ToList().ForEach(y=> { conferenceActivity = y.ConferenceActivities.ToList().Find(z => z.Id == att.ConferenceActivityId); conferenceDay = conferenceActivity != null ? y : null; }));
                                        reportItem.CpdUnits = conferenceActivity != null?conferenceActivity.ActivitySchedule.Activity.CpdUnits:null;
                                        reportItem.CpdAccreditationNumber = conferenceActivity != null?conferenceActivity.ActivitySchedule.Activity.CpdAccreditationNumber:null;
                                        reportItem.ActivityName = conferenceActivity!=null?conferenceActivity.ActivitySchedule.Activity.Name:null;
                                        reportItem.ActivityDate = conferenceDay!=null?(DateTime?)conferenceDay.Date:null;
                                        reportItem.Email = u.Email;
                                        reportItem.PhoneNumber = u.PhoneNumber;
                                        reportItem.Address = string.Format("{0}, {1}, {2}, {3}, {4}",ui.Address.StreetAddress, ui.Address.Barangay, ui.Address.City, ui.Address.Province, ui.Address.Zipcode);
                                        reportItem.Organization = ui.Organization;
                                        reportItem.PcoMembershipStatus = u.IsMember!=null&&u.IsMember.Value? u.IsActive != null && u.IsActive.Value?"Active":"Inactive" : "Non-Member";
                                        reportItem.PcoMembershipType = memTypes.Find(x=>x.Id == ui.MembershipTypeId).Name;

                                        ActivityAttendanceReport.Add(reportItem);
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (ConferenceDay conferenceDay in conference.ConferenceDays)
                            {
                                foreach(ConferenceActivity conferenceActivity in conferenceDay.ConferenceActivities)
                                {
                                    if(conferenceActivity.Id == conferenceActivityId)
                                    {
                                        List<ActivityAttendance> aaa = unitOfWork.ActivityAttendances.GetAll().ToList();
                                        ActivityAttendance aa = aaa.Find(x => x.ConferenceActivityId == conferenceActivityId);
                                        reportItem.isBundle = r.IsBundle;

                                        reportItem.Amount = r.Amount;
                                        reportItem.Discount = r.Discount;
                                        reportItem.RegistrationStatus = regStatus[r.RegistrationStatusId];
                                        reportItem.UserId = r.UserId;
                                        reportItem.UserName = ui.FirstName + " " + ui.LastName;
                                        if (prc != null) reportItem.PrcId = prc.IdNumber;
                                        if (prc != null) reportItem.PrcExpiration = prc.ExpirationDate;
                                        if (aa != null) reportItem.TimeIn = aa.TimeIn;
                                        if (aa != null) reportItem.TimeOut = aa.TimeOut;

                                        reportItem.CpdUnits = conferenceActivity != null ? conferenceActivity.ActivitySchedule.Activity.CpdUnits : null;
                                        reportItem.CpdAccreditationNumber = conferenceActivity != null ? conferenceActivity.ActivitySchedule.Activity.CpdAccreditationNumber : null;
                                        reportItem.ActivityName = conferenceActivity != null ? conferenceActivity.ActivitySchedule.Activity.Name : null;
                                        reportItem.ActivityDate = conferenceDay != null ? (DateTime?)conferenceDay.Date : null;
                                        reportItem.Email = u.Email;
                                        reportItem.PhoneNumber = u.PhoneNumber;
                                        reportItem.Address = string.Format("{0}, {1}, {2}, {3}, {4}", ui.Address.StreetAddress, ui.Address.Barangay, ui.Address.City, ui.Address.Province, ui.Address.Zipcode);
                                        reportItem.Organization = ui.Organization;
                                        reportItem.PcoMembershipStatus = u.IsMember != null && u.IsMember.Value ? u.IsActive != null && u.IsActive.Value ? "Active" : "Inactive" : "Non-Member";
                                        reportItem.PcoMembershipType = memTypes.Find(x => x.Id == ui.MembershipTypeId).Name;

                                        ActivityAttendanceReport.Add(reportItem);
                                    }                        
                                }
                            }        
                        }
                    }
                }
            }
            return ActivityAttendanceReport ;
        }
    }
}