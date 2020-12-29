﻿using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Models.ViewModels;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    /// <summary>
    /// controller class for event registration
    /// </summary>
    public class RegistrationController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public RegistrationController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get list of registration
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="conferenceId">filter results by conference id</param>
        /// <param name="aStatusId">filter by registration status</param>
        /// <param name="userId">filter by user's id</param>
        /// <param name="akeywordFilter">filter by search keyword</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(List<ResponseRegistrationDTO>))]
        public async Task<IHttpActionResult> GetAll(int page = 1, 
                                                    int size = 0, 
                                                    int? conferenceId = null,
                                                    int? aStatusId = null,
                                                    int? userId = null,
                                                    string akeywordFilter = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var registrationList = await Task.Run(() => unitOfWork.Registrations.GetPagedRegistration(page, size, conferenceId, aStatusId, userId, akeywordFilter));
            var registrationListDTO = PaginationMapper<Registration, ResponseListRegistrationDTO>.MapResult(registrationList);

            return Ok(registrationListDTO);
        }

        /// <summary>
        /// Gets registration based on specified id
        /// </summary>
        /// <param name="id">id of the registration to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
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

        /// <summary>
        /// Add a registration
        /// </summary>
        /// <param name="registrationDTO">Details about the registration to be added</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponseRegistrationDTO))]
        public async Task<IHttpActionResult> Add(RequestRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates registration information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="registrationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [Route("api/UpdateRegistration/{id:int}")]
        [ResponseType(typeof(ResponseRegistrationDTO))]
        public async Task<IHttpActionResult> Update(int id, RequestRegistrationDTO registrationDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
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
                    result = await Task.Run(() => unitOfWork.Registrations.Update(result, conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Registration, ResponseRegistrationDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a registration
        /// </summary>
        /// <param name="id">id of registration to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [Route("api/DeleteRegistration/{id:int}")]
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
                    Payment paymentToDelete = await Task.Run(() => unitOfWork.Payments.Find(x => x.RegistrationId == id).FirstOrDefault());
                    if(paymentToDelete!=null) await Task.Run(() => unitOfWork.Payments.Remove(paymentToDelete));
                    await Task.Run(() => unitOfWork.Registrations.Remove(conferenceRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Change registration status
        /// </summary>
        /// <param name="model">registration id and new registration status</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/SetRegistrationStatus")]
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

        /// <summary>
        /// Get registration status
        /// </summary>
        /// <param name="conferenceId">id of conference Registered to</param>
        /// <param name="userId">id of user registered</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetRegistration/{conferenceId=conferenceId}/{userId=userId}")]
        public async Task<IHttpActionResult> GetRegistrationStatus(int conferenceId, int userId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(()=>unitOfWork.Registrations.GetRegistration(conferenceId, userId));

                var registrationDTO = Mapper.Map<Registration, ResponseRegistrationDTO>(result);
                return Ok(registrationDTO);
            }
            catch (Exception)
            {
                return BadRequest("Error occured. Try Again.");
            }
        }

        /// <summary>
        /// Gets RegistrationFee
        /// </summary>
        /// <param name="conferenceId">id of conference to get registration fee from</param>
        /// <param name="membershipTypeId">id of member type</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetRegistrationFee")]
        public async Task<IHttpActionResult> Get(int conferenceId, int membershipTypeId)
        {
            RegistrationFeeDTO registrationFee;
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            var conference = await Task.Run(()=>unitOfWork.Conferences.Get(conferenceId));
            if(conference == null)
            {
                return NotFound();
            }

            var rate = await Task.Run(()=>unitOfWork.Rates.GetRate(x => x.conferenceId == conferenceId && x.membershipTypeId == membershipTypeId)); 
            var promo = conference.PromoId == null ? null : await Task.Run(()=>unitOfWork.Promos.Get((int)conference.PromoId));
            bool IsPromoMemberExists = promo == null ? false : promo.PromoMembers.Any(x => x.MembershipTypeId == membershipTypeId);

            if (rate == null)
            {
                return NotFound();
            }

            if (promo == null)
            {
                registrationFee = GetRegularPrice(rate);
            }
            else
            {
                var start = promo.Start;
                var end = promo.End;
                var today = PhTime.Now();
                if (today >= start && today <= end && IsPromoMemberExists)
                {
                    registrationFee = new RegistrationFeeDTO
                    {
                        Price = rate.regularPrice - promo.Amount,
                        Discount = promo.Amount,
                        IsPromoApplied = true
                    };
                }
                else
                {
                    registrationFee = GetRegularPrice(rate);
                }
            }

            return Ok(registrationFee);
        }

        private RegistrationFeeDTO GetRegularPrice(Rate rate)
        {
            return new RegistrationFeeDTO
            {
                Price = rate.regularPrice,
                Discount = 0,
                IsPromoApplied = false
            };
        }
    }
}
