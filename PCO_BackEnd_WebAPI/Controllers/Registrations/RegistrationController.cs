using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Pagination;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    public class RegistrationController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RegistrationController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get list of registration
        /// </summary>
        /// <param name="page">nth page of the list</param>
        /// <param name="size">number of item to return per page</param>
        /// <param name="conferenceId">filter results by conference id</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<ResponseRegistrationDTO>))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 5, int conferenceId = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var registrationList = await Task.Run( () => unitOfWork.Registrations.GetPagedRegistration(page,size,conferenceId));
            var registrationListDTO = PaginationMapper<Registration, ResponseRegistrationDTO>.MapResult(registrationList);

            return Ok(registrationListDTO);
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Registrations.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var registrationDTO = Mapper.Map<Registration, ResponseRegistrationDTO>(result);
                return Ok(registrationDTO);
            }   
        }

        [HttpPost]
        [ResponseType(typeof(ResponseRegistrationDTO))]
        public async Task<IHttpActionResult> Add(RequestRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registration = Mapper.Map<RequestRegistrationDTO, Registration>(registrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Registrations.Add(registration));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Registration, ResponseRegistrationDTO>(registration);
                return Created(new Uri(Request.RequestUri + "/" + registration.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPut]
        [ResponseType(typeof(ResponseRegistrationDTO))]
        public async Task<IHttpActionResult> Update(int id, RequestRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conferenceRegistration = Mapper.Map<RequestRegistrationDTO, Registration>(registrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Registrations.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.Registrations.Update(id, conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Registration, ResponseRegistrationDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var conferenceRegistration = await Task.Run(() => unitOfWork.Registrations.Get(id));
                if (conferenceRegistration == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Registrations.Remove(conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("SetRegistrationStatus")]
        public async Task<IHttpActionResult> SetRegistrationStatus(SetRegistrationViewModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Registrations.SetRegistrationStatus(model.RegistrationId, model.Status));
                unitOfWork.Complete();

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Error occured. Try Again.");
            }
        }
    }
}
