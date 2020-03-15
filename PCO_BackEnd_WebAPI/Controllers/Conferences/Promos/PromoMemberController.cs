using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
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
    public class PromoMemberController : ApiController
    {
        private readonly ApplicationDbContext _context;
        public PromoMemberController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of promo members
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PromoMembers.GetAll().ToList()
                                                        .Select(Mapper.Map<PromoMember, ResponsePromoMemberDTO>));
            return Ok(result);
        }

        /// <summary>
        /// Gets the promo member information based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PromoMembers.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var rateDTO = Mapper.Map<PromoMember, ResponsePromoMemberDTO>(result);
                return Ok(rateDTO);
            }
        }

        /// <summary>
        /// Adds a promo member
        /// </summary>
        /// <param name="promoMembersDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> AddPromoMember(List<ResponsePromoMemberDTO> promoMembersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promoMembers = promoMembersDTO.Select(Mapper.Map<ResponsePromoMemberDTO, PromoMember>).ToList();
            try
            {
                List<PromoMember> addedPromoMembers = null;
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                if (promoMembers.Count() == 1)
                {
                    await Task.Run(() => unitOfWork.PromoMembers.Add(promoMembers[0]));
                }
                else
                {
                    addedPromoMembers =  await Task.Run(() => unitOfWork.PromoMembers.AddPromoMembers(promoMembers));
                }
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = addedPromoMembers.Select(Mapper.Map<PromoMember, ResponsePromoMemberDTO>);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Updates a promo member
        /// </summary>
        /// <param name="id"></param>
        /// <param name="promoMemberDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> UpdatePromoMembers(int id, ResponsePromoMemberDTO promoMemberDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promoMembers = Mapper.Map<ResponsePromoMemberDTO, PromoMember>(promoMemberDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.PromoMembers.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                   result =  await Task.Run(() => unitOfWork.PromoMembers.UpdatePromoMember(id, promoMembers));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<PromoMember, ResponsePromoMemberDTO>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Deletes a promo member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> DeletePromoMember(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var rate = await Task.Run(() => unitOfWork.PromoMembers.Get(id));
                if (rate == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.PromoMembers.Remove(rate));
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

