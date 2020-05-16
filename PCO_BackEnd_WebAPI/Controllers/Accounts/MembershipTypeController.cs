using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using System.Threading.Tasks;
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Roles;
using System.Web.Http.Cors;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Security.Authorization;
using PCO_BackEnd_WebAPI.Security.OAuth;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    public class MembershipTypeController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeController()
        {
            _context = new ApplicationDbContext();
        }
        
        /// <summary>
        /// Get list of membershiptypes
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="membershipType">keyword for searching</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0, string membershipType = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = unitOfWork.MembershipTypes.GetPagedMembershipTypes(page, size, membershipType);
            var resultDTO = PaginationMapper<MembershipType, ResponseMembershipTypeDTO>.MapResult(result);
            return Ok(result);
        }

        /// <summary>
        /// Gets the membership type based on specified id
        /// </summary>
        /// <param name="id">id of the membershipType to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.MembershipTypes.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var membershipTypeDTO = Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(result);
                return Ok(membershipTypeDTO);
            }   
        }

        /// <summary>
        /// Adds a membershipType
        /// </summary>
        /// <param name="membershipTypeDTO">Details about the type of membershipType to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> AddMembershipType(RequestMembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var membershipType = Mapper.Map<RequestMembershipTypeDTO, MembershipType>(membershipTypeDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.MembershipTypes.Add(membershipType));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(membershipType);
                return Created(new Uri(Request.RequestUri + "/" + membershipType.Id), resultDTO);
            }
            catch(Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
            
        }

        /// <summary>
        /// Updates the specified membershipType
        /// </summary>
        /// <param name="id">id of membershipType</param>
        /// <param name="membershipTypeDTO">New information about membershipType to update</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateMembershipType/{id:int}")]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> UpdateMembershipType(int id, RequestMembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var membershipType = Mapper.Map<RequestMembershipTypeDTO, MembershipType>(membershipTypeDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.MembershipTypes.Get(id));
                if (result == null)
                {
                    return BadRequest();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.MembershipTypes.UpdateMembershipType(id, membershipType));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(result));
                }
               
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes the specified membership type
        /// </summary>
        /// <param name="id">user id of membershipType to delete</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeleteMembershipType/{id:int}")]
        public async Task<IHttpActionResult> DeleteMembershipType(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var membershipType = await Task.Run( () => unitOfWork.MembershipTypes.Get(id));
                if (membershipType == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.MembershipTypes.Remove(membershipType));
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
