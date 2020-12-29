using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences.Promos
{
    /// <summary>
    /// Controller for Members applicable for promo
    /// </summary>
    public class PromoMemberController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default constructor. iniitalize database.
        /// </summary>
        public PromoMemberController()
        {
            _context = new ApplicationDbContext();
        }


        /// <summary>
        /// Gets list of promo members
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PromoMembers.GetPagedPromoMember(page, size));
            var resultDTO = PaginationMapper<PromoMember, ResponsePromoMemberDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the promo member information based on id
        /// </summary>
        /// <param name="id">id of the promo member to be get</param>
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
        /// <param name="promoMembersDTO">Details about the promo member to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> AddPromoMember(List<ResponsePromoMemberDTO> promoMembersDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates a promo member
        /// </summary>
        /// <param name="id">id of the promo member to be updated</param>
        /// <param name="promoMemberDTO">New information about the promo member to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdatePromoMember/{id:int}")]
        [ResponseType(typeof(ResponsePromoMemberDTO))]
        public async Task<IHttpActionResult> UpdatePromoMembers(int id, ResponsePromoMemberDTO promoMemberDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
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
                var errorMessage = ex.Message;
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Deletes a promo member
        /// </summary>
        /// <param name="id">id of promo member to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeletePromoMember/{id:int}")]
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}

