using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
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
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class RateController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RateController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(RateDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Rates.GetAll().ToList()
                                                   .Select(Mapper.Map<Rate,RateDTO>));
            return Ok(result);
        }

        [HttpGet]
        [ResponseType(typeof(RateDTO))]
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
                var rateDTO = Mapper.Map<Rate, RateDTO>(result);
                return Ok(rateDTO);
            }
        }

        [HttpPost]
        [ResponseType(typeof(RateDTO))]
        public async Task<IHttpActionResult> AddRates(List<RateDTO> rateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rates = rateDTO.Select(Mapper.Map<RateDTO, Rate>).ToList();
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
                var resultDTO = rates.Select(Mapper.Map<Rate, RateDTO>);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpPut]
        [ResponseType(typeof(ConferenceDTO))]
        public async Task<IHttpActionResult> UpdateRate(int id, RateDTO rateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var rate = Mapper.Map<RateDTO, Rate>(rateDTO);
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
                    result = await Task.Run(() => unitOfWork.Rates.UpdateRate(rate));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Rate, RateDTO>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpDelete]
        [ResponseType(typeof(ConferenceDTO))]
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
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
