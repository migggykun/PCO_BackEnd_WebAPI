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


        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetPagedPayments(page, size));
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

        [HttpPost]
        [ResponseType(typeof(AddPaymentDTO))]
        public async Task<IHttpActionResult> AddPayment(AddPaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }

        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdatePayment(int id, UpdatePaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

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
