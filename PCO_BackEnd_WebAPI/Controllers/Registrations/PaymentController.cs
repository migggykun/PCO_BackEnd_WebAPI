﻿using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
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
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Helpers;

namespace PCO_BackEnd_WebAPI.Controllers.Registrations
{
    [RoutePrefix("api/Payment")]
    public class PaymentController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PaymentController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of Registration
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, 
                                                    int size = 0, 
                                                    DateTime? aPaymentSubmissionDateFrom = null, 
                                                    DateTime? aPaymentSubmissionDateTo = null,
                                                    DateTime? aConfirmationDateFrom = null,
                                                    DateTime? aConfirmationDateTo = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetPagedPayments(page, 
                                                                                   size,
                                                                                   aPaymentSubmissionDateFrom,
                                                                                   aPaymentSubmissionDateTo,
                                                                                   aConfirmationDateFrom,
                                                                                   aConfirmationDateTo));

            var resultDTO = PaginationMapper<Payment, ResponsePaymentDTO>.MapResult(result);
            var users = result.Results.Select(x => unitOfWork.UserInfos.Get(x.Registration.UserId));
            var conferences = result.Results.Select(x => unitOfWork.Conferences.Get(x.Registration.ConferenceId));

            foreach (var r in result.Results)
            {
                int index = resultDTO.Results.ToList().FindIndex(x => x.RegistrationId == r.RegistrationId);
                var userInfo = users.First(x => x.Id == r.Registration.UserId);
                var conf = conferences.First(x => x.Id ==r.Registration.ConferenceId);
                PaymentMapper.MapToResponsePaymentDTO(resultDTO.Results[index], userInfo, conf);
            }
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets user based on specified id
        /// </summary>
        /// <param name="id">id of payment to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(result);
                var user = unitOfWork.UserInfos.Get(result.Registration.UserId);
                var conference = unitOfWork.Conferences.Get(result.Registration.ConferenceId);
                PaymentMapper.MapToResponsePaymentDTO(resultDTO, user, conference);
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// Add a payment for registration
        /// </summary>
        /// <param name="paymentDTO">Details about the payment to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> AddPayment(AddPaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var payment = Mapper.Map<AddPaymentDTO, Payment>(paymentDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Payments.Add(payment));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);
                return Created(new Uri(Request.RequestUri + "/" + payment.RegistrationId), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }

        }

        /// <summary>
        /// Updates payment details
        /// </summary>
        /// <param name="id">id of payment to be updated</param>
        /// <param name="paymentDTO">New information of payment to be updated</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePayment(int id, UpdatePaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }
            var payment = Mapper.Map<UpdatePaymentDTO, Payment>(paymentDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Payments.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Payments.UpdatePayment(id, payment));
                    await Task.Run(() => unitOfWork.Complete());
                    var user = unitOfWork.UserInfos.Get(result.Registration.UserId);
                    var conference = unitOfWork.Conferences.Get(result.Registration.ConferenceId);
                    var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);
                    PaymentMapper.MapToResponsePaymentDTO(resultDTO, user, conference);
                    return Ok(resultDTO);
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a payment
        /// </summary>
        /// <param name="id">id of payment to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeletePayment(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var payment = await Task.Run(() => unitOfWork.Payments.Get(id));
                if (payment == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Payments.Remove(payment));
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
        /// Set the date when the payment is confirmed
        /// </summary>
        /// <param name="id">id of payment to be confirmed</param>
        /// <returns></returns>
        [HttpPut]
        [Route("SetConfirmationDate/{id}")]
        public async Task<IHttpActionResult> ConfirmPaymentDate(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            if (unitOfWork.Payments.Get(id) == null)
            {
                return NotFound();
            }

            unitOfWork.Payments.SetPaymentConfirmationDate(id);
            unitOfWork.Complete();
            return Ok();
        }
    }
}
