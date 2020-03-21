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

namespace PCO_BackEnd_WebAPI.Controllers.Registrations
{
    public class PaymentController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PaymentController()
        {
            _context = new ApplicationDbContext();
        }


        [HttpGet]
        [ResponseType(typeof(ResponsePaymentDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Payments.GetAll().ToList());
            var resultDTO = result.Select(Mapper.Map<Payment, ResponsePaymentDTO>);
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
                var paymentDTO = Mapper.Map<Payment, ResponsePaymentDTO>(result);
                return Ok(paymentDTO);
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
        public async Task<IHttpActionResult> UpdatePayment(int registrationId, UpdatePaymentDTO paymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var payment = Mapper.Map<UpdatePaymentDTO, Payment>(paymentDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Payments.Get(registrationId));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Payments.UpdatePayment(registrationId, payment));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Payment, ResponsePaymentDTO>(payment));
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
        [Route("SetConfirmationDate")]
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
