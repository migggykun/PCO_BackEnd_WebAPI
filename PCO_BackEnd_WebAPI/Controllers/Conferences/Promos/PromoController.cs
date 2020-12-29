using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.DTOs.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences.Promos
{
    /// <summary>
    /// Controller class for promos
    /// </summary>
    public class PromoController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public PromoController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get list of promos
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="filter">search string filter</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(PageResult<ResponsePromoDTO>))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0, string filter = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(()=>unitOfWork.Promos.GetPagedPromos(page, size, filter));
            var resultDTO = PaginationMapper<Promo, ResponsePromoDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the promo based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponsePromoDTO))]
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
                var promoDTO = Mapper.Map<Promo, ResponsePromoDTO>(result);
                return Ok(promoDTO);
            }
        }

        /// <summary>
        /// Adds a promo for conference
        /// </summary>
        /// <param name="promoDTO">Details about the Promo to be added</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(ResponsePromoDTO))]
        public async Task<IHttpActionResult> AddPromo(RequestPromoDTO promoDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var promo = Mapper.Map<RequestPromoDTO, Promo>(promoDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Promos.Add(promo));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Promo, ResponsePromoDTO>(promo);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates the promo details 
        /// </summary>
        /// <param name="id">id of the promo to be updated</param>
        /// <param name="promoDTO">New information about the promo to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/UpdatePromo/{id:int}")]
        [ResponseType(typeof(ResponsePromoDTO))]
        public async Task<IHttpActionResult> UpdatePromo(int id, RequestPromoDTO promoDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var promo = Mapper.Map<RequestPromoDTO, Promo>(promoDTO);
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
                    result = await Task.Run(() => unitOfWork.Promos.UpdatePromoDetails(id, promo));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Promo, ResponsePromoDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a promo based on specified id
        /// </summary>
        /// <param name="id">id of the promo to be deleted.</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/DeletePromo/{id:int}")]
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}