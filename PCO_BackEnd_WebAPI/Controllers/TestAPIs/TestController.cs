
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Images.Manager;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System.Data.Entity.Validation;
using PCO_BackEnd_WebAPI.Security.Authorization;
using PCO_BackEnd_WebAPI.Models.Roles;

namespace PCO_BackEnd_WebAPI.Controllers.TestAPIs
{
    public class TestController : ApiController
    {
        private readonly ApplicationDbContext _context;
        
        public TestController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            //Activities
            //var result =  await Task.Run(() => _context.Activities.ToList());
            //return Ok(result);

            //ActivitySchedule
            var result = await Task.Run(() => _context.ActivitySchedules.ToList());
            return Ok(result);

            //ConferenceActivity
            //var result = await Task.Run(() => _context.ConferenceActivities.ToList());
            //return Ok(result);


            //ConferenceDays
            //var result = await Task.Run(() => _context.ConferenceDays.ToList());
            //return Ok(result);

            //Conference
            //var result =  await Task.Run(() => _context.Conferences.ToList());
            //return Ok(result);


        }

        [HttpPost]
        [Route("api/AddActivitySchedule")]
        [ResponseType(typeof(ResponseActivityScheduleDTO))]
        public async Task<IHttpActionResult> AddActivitySchedule(RequestActivityScheduleDTO activityScheduleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var activitySchedule = Mapper.Map<RequestActivityScheduleDTO, ActivitySchedule>(activityScheduleDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.ActivitySchedules.Add(activitySchedule));
                await Task.Run(() => unitOfWork.Complete());
                //var resultDTO = Mapper.Map<ActivitySchedule, ResponseActivityScheduleDTO>(activitySchedule);
                return Ok(Mapper.Map<ActivitySchedule, ResponseActivityScheduleDTO>(activitySchedule));

            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("api/AddConferenceActivity")]
        [ResponseType(typeof(ResponseConferenceActivityDTO))]
        public async Task<IHttpActionResult> AddConferenceActivity(RequestConferenceActivityDTO conferenceActivityDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var activitySchedule = Mapper.Map<RequestActivityScheduleDTO, ActivitySchedule>(conferenceActivityDTO);
            try
            {
                var conferenceActivity = new ConferenceActivity
                {
                    Id = 0,
                    ConferenceDayId = 1,
                    ActivityScheduleId = 0,
                    ActivitySchedule = new ActivitySchedule
                    {
                        Id = 0,
                        ActivityId = 1,
                        Start = PhTime.Now().TimeOfDay,
                        End = PhTime.Now().TimeOfDay
                    }
                };
                _context.ConferenceActivities.Add(conferenceActivity);
                _context.SaveChanges();
                return Ok(conferenceActivity);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("api/AddConferenceDay")]
        [ResponseType(typeof(ResponseConferenceDayDTO))]
        public async Task<IHttpActionResult> AddConferenceDay(RequestConferenceDayDTO conferenceDayDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var conferenceDay = Mapper.Map<RequestConferenceDayDTO, ConferenceDay>(conferenceDayDTO);
            try
            {
                var conferenceDay = new ConferenceDay
                {
                    Id = 0,
                    ConferenceId = 4,
                    Date = PhTime.Now(),
                    Start = PhTime.Now().TimeOfDay,
                    End = PhTime.Now().TimeOfDay
                };
                _context.ConferenceDays.Add(conferenceDay);
                _context.SaveChanges();
                return Ok(conferenceDay);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("api/AddPayment")]
        public async Task<IHttpActionResult> AddPayment(AddPaymentDTO addpaymentDTO)
        {
            try
            {
                Payment temp = new Payment
                {
                    AmountPaid = 1200,
                    TransactionNumber = "ABCD",
                    PaymentSubmissionDate = PhTime.Now(),
                    //refRegistrationId = 10088,
                    paymentType = "membership"
                };

                _context.Payments.Add(temp);
                _context.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            return Ok();
        }

        [HttpGet]
        [CustomAuthFilter]
        [Route("api/TestGet")]
        public async Task<IHttpActionResult> Get()
        {
            var result = new
            {
                Name = "Miguel",
                Age = "26"
            };
            return Ok(result);
        }
    }
}
