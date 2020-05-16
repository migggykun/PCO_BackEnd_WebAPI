using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Roles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Security.OAuth;
using PCO_BackEnd_WebAPI.Roles;
using Microsoft.AspNet.Identity;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    public class AddressController : ApiController
    {
        private readonly ApplicationDbContext _context;

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
        [CustomAuthorize(Roles = UserRoles.ROLE_ADMIN)]
        [HttpGet]
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
        [CustomAuthorize]
        [HttpGet]
        [ResponseType(typeof(ResponseAddressDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            int userId = Convert.ToInt32(User.Identity.GetUserId());
            if (User.IsInRole(UserRoles.ROLE_MEMBER) && userId != id)
            {
                return StatusCode(HttpStatusCode.Forbidden);
            }

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
        [CustomAuthorize]
        [HttpPost]
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
                return BadRequest("Error Occured, try again");
            }
        }
    }
}