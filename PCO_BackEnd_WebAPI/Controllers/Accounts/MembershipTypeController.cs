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

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [RoutePrefix("api/MembershipType")]
    public class MembershipTypeController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of types of membership
        /// </summary>
        /// <param name="membershipType"></param>
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
        /// <param name="id"></param>
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
        /// <param name="membershipTypeDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> AddMembershipType(RequestMembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
            
        }

        /// <summary>
        /// Updates the specified membershipType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="membershipTypeDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> UpdateMembershipType(int id, RequestMembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes the specified membership type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
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
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

    }
}
