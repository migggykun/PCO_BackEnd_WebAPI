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
    /// Controller Class for User Address
    /// </summary>
    public class AddressController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor, initialize database
        /// </summary>
        public AddressController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of user information
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(ResponseAddressDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Addresses.GetPagedAddress(page, size));
            var resultDTO = PaginationMapper<Address, ResponseAddressDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the user information based on specified id
        /// </summary>
        /// <param name="id">id of the user information to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponseAddressDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Addresses.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var addressDTO = Mapper.Map<Address, ResponseAddressDTO>(result);
                return Ok(addressDTO);
            }
        }

        /// <summary>
        /// Updates user info based on specified id
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="addressDTO">New information about the user to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter]
        [Route("api/UpdateAddress/{id:int}")]
        [ResponseType(typeof(ResponseAddressDTO))]
        public async Task<IHttpActionResult> UpdateAddress(int id, RequestAddressDTO addressDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var address = Mapper.Map<RequestAddressDTO, Address>(addressDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Addresses.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result =  await Task.Run(() => unitOfWork.Addresses.UpdateAddress(id, address));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Address, ResponseAddressDTO>(result));
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