using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Images.Manager;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Registrations
{
    /// <summary>
    /// Controller Class for Payments
    /// </summary>
    public class PaymentController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public PaymentController()
        {
            _context = new ApplicationDbContext();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="aPaymentSubmissionDateFrom"></param>
        /// <param name="aPaymentSubmissionDateTo"></param>
        /// <param name="aConfirmationDateFrom"></param>
        /// <param name="aConfirmationDateTo"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> GetAll(string filter = null,
                                                    int page = 1, 
                                                    int size = 0, 
                                                    DateTime? aPaymentSubmissionDateFrom = null, 
                                                    DateTime? aPaymentSubmissionDateTo = null,
                                                    DateTime? aConfirmationDateFrom = null,
                                                    DateTime? aConfirmationDateTo = null,
                                                    string registrationStatus=null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetPagedPayments(filter,
                                                                                   page, 
                                                                                   size,
                                                                                   aPaymentSubmissionDateFrom,
                                                                                   aPaymentSubmissionDateTo,
                                                                                   aConfirmationDateFrom,
                                                                                   aConfirmationDateTo,
                                                                                   registrationStatus));

            var users = result.Results.Select(x => unitOfWork.UserInfos.Get(String.Compare(x.paymentType.Trim().ToLower(),"registration",true) == 0? x.Registration.UserId : (int)x.MemberRegistration.UserId));
            var conferences = result.Results.Select(x => String.Compare(x.paymentType.Trim().ToLower(), "registration",true) == 0? unitOfWork.Conferences.Get(x.Registration.ConferenceId) : null);
            var membershipRegistrations = unitOfWork.MemberRegistrations.GetAll().Where(m => users.ToList().Find(u => u.Id == m.UserId) != null);
            var resultDTO =  PaymentMapper.MapToPagedResponsePaymentDTO(result, conferences, users, membershipRegistrations);

            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets user based on specified id
        /// </summary>
        /// <param name="id">id of payment to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            int maxValidStatus = 3;
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
                    resultDTO = PaymentMapper.MapToResponsePaymentDTO(result, conference, user, result.Registration != null ? (int?)result.Registration.RegistrationStatusId : null);
                }
                else if(string.Compare(result.paymentType, "membership",true) ==0)
                {
                    MemberRegistration membershipRegistration;
                    if (result.MemberRegistration.MemberRegistrationStatusId > maxValidStatus)
                    {
                        membershipRegistration = unitOfWork.MemberRegistrations.Find(x => x.Id == result.MemberRegistrationId).FirstOrDefault();
                        
                    }
                    else
                    {
                        membershipRegistration = unitOfWork.MemberRegistrations.Find(x => x.Id == result.MemberRegistrationId && result.MemberRegistration.MemberRegistrationStatusId!=6).FirstOrDefault();
                    }
                    if (membershipRegistration != null) user = unitOfWork.UserInfos.Get(membershipRegistration.UserId);
                    resultDTO = PaymentMapper.MapToResponsePaymentDTO(result, null, user, membershipRegistration != null ? (int?)result.MemberRegistration.MemberRegistrationStatusId : null, membershipRegistration != null ? (int?)membershipRegistration.Id : null);
                }
                else
                {
                    //do nothing
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
        [CustomAuthFilter]
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
        /// Gets payment from member Registration Id
        /// </summary>
        /// <param name="memberRegistrationId">id of member Registration</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponsePaymentDTO))]
        [Route("api/GetMemberRegistrationPayment/{memberRegistrationId}")]
        public async Task<IHttpActionResult> GetMemberRegistrationPayment(int memberRegistrationId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.Find(x => x.MemberRegistrationId == memberRegistrationId).FirstOrDefault());
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
        /// Add a payment for registration
        /// </summary>
        /// <param name="paymentDTO">Details about the payment to be added</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
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
        [CustomAuthFilter]
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
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
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
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/payment/SetConfirmationDate/{id}")]
        public async Task<IHttpActionResult> ConfirmPaymentDate(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            if (await Task.Run(()=>unitOfWork.Payments.Get(id)) == null)
            {
                return NotFound();
            }

            unitOfWork.Payments.SetPaymentConfirmationDate(id);
            unitOfWork.Complete();
            return Ok();
        }
    }
}
