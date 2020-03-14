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

        [HttpGet]
        [ResponseType(typeof(PromoMemberDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PromoMembers.GetAll().ToList()
                                                        .Select(Mapper.Map<PromoMember, PromoMemberDTO>));
            return Ok(result);
        }

        [HttpGet]
        [ResponseType(typeof(PromoMemberDTO))]
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
                var rateDTO = Mapper.Map<PromoMember, PromoMemberDTO>(result);
                return Ok(rateDTO);
            }
        }

        [HttpPost]
        [ResponseType(typeof(PromoMemberDTO))]
        public async Task<IHttpActionResult> AddPromoMember(List<PromoMemberDTO> promoMembersDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var promoMembers = promoMembersDTO.Select(Mapper.Map<PromoMemberDTO, PromoMember>).ToList();
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
                var resultDTO = addedPromoMembers.Select(Mapper.Map<PromoMember, PromoMemberDTO>);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpPut]
        [ResponseType(typeof(PromoMemberDTO))]
        public async Task<IHttpActionResult> UpdatePromoMembers(int id, PromoMemberDTO promoMemberDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var promoMembers = Mapper.Map<PromoMemberDTO, PromoMember>(promoMemberDTO);
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
                   result =  await Task.Run(() => unitOfWork.PromoMembers.UpdatePromoMember(promoMembers));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<PromoMember, PromoMemberDTO>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpDelete]
        [ResponseType(typeof(PromoMemberDTO))]
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}

