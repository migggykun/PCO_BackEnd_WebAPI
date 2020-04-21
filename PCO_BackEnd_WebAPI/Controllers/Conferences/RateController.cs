using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    [RoutePrefix("api/Rate")]
    public class RateController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RateController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of rates
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseRateDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Rates.GetPagedRates(page, size));
            var resultDTO = PaginationMapper<Rate, ResponseRateDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the rate information based on specified id
        /// </summary>
        /// <param name="id">id of rate to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseRateDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Rates.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var rateDTO = Mapper.Map<Rate, ResponseRateDTO>(result);
                return Ok(rateDTO);
            }
        }

        /// <summary>
        /// Add list of Rates 
        /// </summary>
        /// <param name="rateDTO">>Details about the rate to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseRateDTO))]
        public async Task<IHttpActionResult> AddRates(List<RequestRateDTO> rateDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var rates = rateDTO.Select(Mapper.Map<RequestRateDTO, Rate>).ToList();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                if(rates.Count() == 1)
                {
                    await Task.Run(() => unitOfWork.Rates.Add(rates[0]));
                }
                else
                {
                    await Task.Run(() => unitOfWork.Rates.AddRates(rates));
                }
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = rates.Select(Mapper.Map<Rate, ResponseRateDTO>);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates a rate
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="rateDTO">New information about the rate to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateRate/{id:int}")]
        public async Task<IHttpActionResult> UpdateRate(int id, RequestRateDTO rateDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var rate = Mapper.Map<RequestRateDTO, Rate>(rateDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Rates.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.Rates.UpdateRate(id, rate));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Rate, ResponseRateDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a rate
        /// </summary>
        /// <param name="id">id of the rate to be deleted.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteRate/{id:int}")]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> DeleteRates(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var rate = await Task.Run(() => unitOfWork.Rates.Get(id));
                if (rate == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Rates.Remove(rate));
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
