using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ConferenceRegistrationController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ConferenceRegistrationController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(ConferenceRegistrationDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.ConferenceRegistration.GetAll().ToList()
                                                        .Select(Mapper.Map<Registration, ConferenceRegistrationDTO>));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.ConferenceRegistration.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var registrationDTO = Mapper.Map<Registration, ConferenceRegistrationDTO>(result);
                return Ok(registrationDTO);
            }   
        }

        [HttpPost]
        [ResponseType(typeof(ConferenceRegistrationDTO))]
        public async Task<IHttpActionResult> Add(ConferenceRegistrationDTO conferenceRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conferenceRegistration = Mapper.Map<ConferenceRegistrationDTO, Registration>(conferenceRegistrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.ConferenceRegistration.Add(conferenceRegistration));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Registration, ConferenceRegistrationDTO>(conferenceRegistration);
                return Created(new Uri(Request.RequestUri + "/" + conferenceRegistration.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPut]
        [ResponseType(typeof(ConferenceRegistrationDTO))]
        public async Task<IHttpActionResult> Update(int id, ConferenceRegistrationDTO aConferenceRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conferenceRegistration = Mapper.Map<ConferenceRegistrationDTO, Registration>(aConferenceRegistrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.ConferenceRegistration.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                   result =  await Task.Run(() => unitOfWork.ConferenceRegistration.Update(conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Registration, ConferenceRegistrationDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpDelete]
        [ResponseType(typeof(ConferenceRegistrationDTO))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var conferenceRegistration = await Task.Run(() => unitOfWork.ConferenceRegistration.Get(id));
                if (conferenceRegistration == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.ConferenceRegistration.Remove(conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Registration, ConferenceRegistrationDTO>(conferenceRegistration));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}
