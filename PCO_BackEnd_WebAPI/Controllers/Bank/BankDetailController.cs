using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Bank;
using PCO_BackEnd_WebAPI.DTOs.Bank;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Bank
{
    public class BankDetailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public BankDetailController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Returns all bank details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<ResponseBankDetailDTO>))]
        public async Task <IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.BankDetails.GetAll());
            var resultDTO = result.Select(Mapper.Map<BankDetail, ResponseBankDetailDTO>).ToList();
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets bank detail based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.BankDetails.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var resultDTO = Mapper.Map<BankDetail, ResponseBankDetailDTO>(result);
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// Adds a Bank Detail
        /// </summary>
        /// <param name="bankDetailDTO">Bank details to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseBankDetailDTO))]
        public async Task<IHttpActionResult> AddConference(RequestBankDetailDTO bankDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bankDetail = Mapper.Map<RequestBankDetailDTO, BankDetail>(bankDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.BankDetails.Add(bankDetail));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<BankDetail, ResponseBankDetailDTO>(bankDetail);
                return Created(new Uri(Request.RequestUri + "/" + bankDetail.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates BankDetails
        /// </summary>
        /// <param name="id">id of the bank detail to be updated</param>
        /// <param name="conferenceDTO">New information about the bank to be updated</param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(ResponseBankDetailDTO))]
        public async Task<IHttpActionResult> UpdateBankDetail(int id, RequestBankDetailDTO bankDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bankDetail = Mapper.Map<RequestBankDetailDTO, BankDetail>(bankDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.BankDetails.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.BankDetails.UpdateBankDetails(id, bankDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<BankDetail, ResponseBankDetailDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a bank details based on specified id
        /// </summary>
        /// <param name="id">id of the bank details to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteBankDetails(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var bankDetail = await Task.Run(() => unitOfWork.BankDetails.Get(id));
                if (bankDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.BankDetails.Remove(bankDetail));
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
    }
}