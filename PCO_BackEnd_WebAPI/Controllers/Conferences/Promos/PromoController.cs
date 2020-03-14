using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
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

namespace PCO_BackEnd_WebAPI.Controllers.Conferences.Promos
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class PromoController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PromoController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(PromoDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Promos.GetAll().ToList()
                                                        .Select(Mapper.Map<Promo, PromoDTO>));
            return Ok(result);
        }

        [HttpGet]
        [ResponseType(typeof(PromoDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Promos.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var promoDTO = Mapper.Map<Promo, PromoDTO>(result);
                return Ok(promoDTO);
            }
        }

        [HttpPost]
        [ResponseType(typeof(PromoDTO))]
        public async Task<IHttpActionResult> AddPromo(PromoDTO promoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promo = Mapper.Map<PromoDTO, Promo>(promoDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Promos.Add(promo));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Promo, PromoDTO>(promo);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPut]
        [ResponseType(typeof(PromoDTO))]
        public async Task<IHttpActionResult> UpdatePromo(int id, PromoDTO promoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promo = Mapper.Map<PromoDTO, Promo>(promoDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Promos.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.Promos.UpdatePromoDetails(promo));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Promo, PromoDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpDelete]
        [ResponseType(typeof(ConferenceDTO))]
        public async Task<IHttpActionResult> DeletePromo(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var rate = await Task.Run(() => unitOfWork.Promos.Get(id));
                if (rate == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Promos.Remove(rate));
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