using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    /// <summary>
    /// Controller for PRC Details
    /// </summary>
    [RoutePrefix("api/PRCDetail")]
    public class PRCDetailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. Initialize Database
        /// </summary>
        public PRCDetailController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of prc details
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="prcId">search PRC Details with the input prcId</param>
        /// <param name="aExpirationDateFrom">Search ExpirationDate From filter</param>
        /// <param name="aExpirationDateTo">Search ExpirationDate To Filter</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, 
                                                    int size = 0, 
                                                    string prcId = null,
                                                    DateTime? aExpirationDateFrom = null,
                                                    DateTime? aExpirationDateTo = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PRCDetails.GetPagedPRCDetail(page, 
                                                                                      size, 
                                                                                      prcId, 
                                                                                      aExpirationDateFrom, 
                                                                                      aExpirationDateTo));
            var resultDTO = PaginationMapper<PRCDetail, ResponsePRCDetailDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the PRC details based on specified id
        /// </summary>
        /// <param name="id">id of the prc detail to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var membershipTypeDTO = Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result);
                return Ok(membershipTypeDTO);
            }
        }

        /// <summary>
        /// Adds PRC details
        /// </summary>
        /// <param name="PRCDetailDTO">Details about PRC Details to be added</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> AddPRCDetail(RequestPRCDetailDTO PRCDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var prcDetail = Mapper.Map<RequestPRCDetailDTO, PRCDetail>(PRCDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.PRCDetails.Add(prcDetail));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(prcDetail);
                return Created(new Uri(Request.RequestUri + "/" + prcDetail.Id), resultDTO);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return BadRequest("Error Occured, try again");
            }

        }

        /// <summary>
        /// Updates PRC detail based on specified id
        /// </summary>
        /// <param name="id">user Id</param>
        /// <param name="prcDetailDTO">New information about the PRCDetails to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [Route("UpdatePRCDetail/{id:int}")]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> UpdatePRCDetail(int id, RequestPRCDetailDTO prcDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var PRCDetail = Mapper.Map<RequestPRCDetailDTO, PRCDetail>(prcDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.PRCDetails.Update(id, PRCDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result));
                }

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Deletes PRC detail based on specified id
        /// </summary>
        /// <param name="id">user id to delete</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [Route("api/DeletePRCDetail/{id:int}")]
        public async Task<IHttpActionResult> DeletePRCDetail(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var PRCDetail = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
                if (PRCDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.PRCDetails.Remove(PRCDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
