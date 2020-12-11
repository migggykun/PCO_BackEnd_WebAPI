using AutoMapper;
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
using PCO_BackEnd_WebAPI.Models.Images.Manager;
using PCO_BackEnd_WebAPI.Models.Images;

namespace PCO_BackEnd_WebAPI.Controllers.Registrations
{
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
        public async Task<IHttpActionResult> GetAll(string filter = null,
                                                    int page = 1, 
                                                    int size = 0, 
                                                    DateTime? aPaymentSubmissionDateFrom = null, 
                                                    DateTime? aPaymentSubmissionDateTo = null,
                                                    DateTime? aConfirmationDateFrom = null,
                                                    DateTime? aConfirmationDateTo = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetPagedPayments(filter,
                                                                                   page, 
                                                                                   size,
                                                                                   aPaymentSubmissionDateFrom,
                                                                                   aPaymentSubmissionDateTo,
                                                                                   aConfirmationDateFrom,
                                                                                   aConfirmationDateTo));

            var users = result.Results.Select(x => unitOfWork.UserInfos.Get(String.Compare(x.paymentType.Trim().ToLower(),"membership",true) == 0? (int)x.UserId: x.Registration.UserId));
            var conferences = result.Results.Select(x => String.Compare(x.paymentType.Trim().ToLower(), "registration",true) == 0? unitOfWork.Conferences.Get(x.Registration.ConferenceId) : null);

            var resultDTO =  PaymentMapper.MapToPagedResponsePaymentDTO(result, conferences, users);

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
            UserInfo user = null;
            Conference conference = null;

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(result);
                if (string.Compare(result.paymentType, "registration", true) == 0)
                {
                    user = unitOfWork.UserInfos.Get(result.Registration.UserId);
                    conference = unitOfWork.Conferences.Get(result.Registration.ConferenceId);
                    resultDTO = PaymentMapper.MapToResponsePaymentDTO(result, conference, user, result.Registration == null ? (int?)result.Registration.RegistrationStatusId : null);
                }
               
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        [Route("api/GetRegistrationPayment/{registrationId}")]
        public async Task<IHttpActionResult> GetRegistrationPayment(int registrationId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.Find(x=>x.RegistrationId==registrationId).FirstOrDefault());
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var activity = Mapper.Map<Payment, ResponsePaymentDTO>(result);
                return Ok(activity);
            }
        }

        /// <summary>
        /// Gets list of Registration
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        [Route("api/GetMemberPayments/{userId}")]
        public async Task<IHttpActionResult> GetMemberPayments(int userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetPagedPayments(userId));

            var resultDTO = PaginationMapper<Payment, ResponsePaymentDTO>.MapResult(result);

            return Ok(resultDTO);
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

            UserInfo user = null;
            Conference conference = null;
            Registration registration = null;
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var payment = Mapper.Map<AddPaymentDTO, Payment>(paymentDTO);
            try
            {
                if (!string.IsNullOrEmpty(paymentDTO.ProofOfPayment))
                {
                    //Convert receipt image to bytes
                    ImageManager receiptManager = new ImageManager(paymentDTO.ProofOfPayment);
                    payment.Receipt = new Receipt();
                    payment.Receipt.Id = payment.Id;
                    payment.Receipt.Image = receiptManager.GetAdjustedSizeInBytes();
                }

                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Payments.Add(payment));
                await Task.Run(() => unitOfWork.Complete());

                var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(payment);

                //Convert to DTO
                if(string.Compare(paymentDTO.paymentType, "registration",true) == 0 && payment.RegistrationId != null)
                {             
                    registration = unitOfWork.Registrations.Get((int)payment.RegistrationId);
                    user = unitOfWork.UserInfos.Get(registration.UserId);
                    conference = registration.Conference;
                    resultDTO = PaymentMapper.MapToResponsePaymentDTO(payment, conference, user, registration == null ? (int?)registration.RegistrationStatusId : null);
                }
               
                return Created(new Uri(Request.RequestUri + "/" + payment.Id), resultDTO);
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
        [HttpPost]
        [Route("api/UpdatePayment/{id:int}")]
        public async Task<IHttpActionResult> UpdatePayment(int id, UpdatePaymentDTO paymentDTO)
        {
            Registration registration = null;
            UserInfo user = null;
            Conference conference = null;

            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }
            var newPayment = Mapper.Map<UpdatePaymentDTO, Payment>(paymentDTO);
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
                    await Task.Run(() => unitOfWork.Payments.UpdatePayment(result, newPayment, paymentDTO.ProofOfPayment));
                    await Task.Run(() => unitOfWork.Complete());

                    var resultDTO = Mapper.Map<Payment, ResponsePaymentDTO>(newPayment);
                    //Convert to DTO
                    if (string.Compare(paymentDTO.paymentType, "registration", true) == 0 && newPayment.RegistrationId != null)
                    {
                        registration = unitOfWork.Registrations.Get((int)result.RegistrationId);
                        user = unitOfWork.UserInfos.Get(registration.UserId);
                        conference = registration.Conference;
                        resultDTO = PaymentMapper.MapToResponsePaymentDTO(result, conference, user, registration == null ? (int?)registration.RegistrationStatusId : null);
                    }
                   
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
        [HttpPost]
        [Route("api/DeletePayment/{id:int}")]
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
        [HttpPost]
        [Route("api/payment/SetConfirmationDate/{id}")]
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
