﻿using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Bank;
using PCO_BackEnd_WebAPI.Models.Bank;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Bank
{
    /// <summary>
    /// Controller Class for Bank Details
    /// </summary>
    public class BankDetailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. Initialize Database.
        /// </summary>
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
        public async Task <IHttpActionResult> GetAll(int page = 0, int size = 0, string filter = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.BankDetails.GetPagedBankDetails(page, size, filter));
            return Ok(result);
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
        public async Task<IHttpActionResult> AddBankDetails(RequestAddBankDetailDTO bankDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bankDetail = Mapper.Map<RequestAddBankDetailDTO, BankDetail>(bankDetailDTO);
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates BankDetails
        /// </summary>
        /// <param name="id">id of the bank detail to be updated</param>
        /// <param name="bankDetailDTO">New information about the bank to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateBankDetail/{id:int}")]
        [ResponseType(typeof(ResponseBankDetailDTO))]
        public async Task<IHttpActionResult> UpdateBankDetails(int id, RequestUpdateBankDetailDTO bankDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var bankDetail = Mapper.Map<RequestUpdateBankDetailDTO, BankDetail>(bankDetailDTO);
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a bank details based on specified id
        /// </summary>
        /// <param name="id">id of the bank details to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeleteBankDetail/{id:int}")]
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}