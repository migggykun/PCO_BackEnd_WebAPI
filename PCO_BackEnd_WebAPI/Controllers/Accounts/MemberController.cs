﻿using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using RefactorThis.GraphDiff;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    /// <summary>
    /// Controller for PCO Member(paid)
    /// </summary>
    public class MemberController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public MemberController()
        {
            _context = new ApplicationDbContext();
        }
        
        /// <summary>
        /// Get list of members
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseMemberDTO))]
        [Route("api/GetMembers/")]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(()=>unitOfWork.Members.GetPageMembers(page, size));
            var resultDTO = PaginationMapper<Member, ResponseMemberDTO>.MapResult(result);
            return Ok(result);
        }

        /// <summary>
        /// Gets the member based on specified userId
        /// </summary>
        /// <param name="userId">id of the user to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetMember/")]
        [ResponseType(typeof(ResponseMemberDTO))]
        public async Task<IHttpActionResult> Get(int userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Members.GetMemberByUserId(userId));
            if (result == null)
            {
                return Ok(result);
            }
            else
            {
                var memberDTO = Mapper.Map<Member, ResponseMemberDTO>(result);
                return Ok(memberDTO);
            }
        }

        /// <summary>
        /// Adds a member
        /// </summary>
        /// <param name="userId">Id of member to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseMemberDTO))]
        public async Task<IHttpActionResult> AddMember(int userId)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            UnitOfWork unitOfWork = new UnitOfWork(_context);


            var memberExists = await Task.Run(() => unitOfWork.Members.GetMemberByUserId(userId));
            if (memberExists!=null)
            {
                string errorMessages = "Member Already Exists";
                return BadRequest(errorMessages);
            }

            Member member = new Member(userId);
            try
            {
                
                await Task.Run(() => unitOfWork.Members.Add(member));

                ApplicationUser updateMembership = new ApplicationUser();
                ApplicationUser getMember = await Task.Run(() => unitOfWork.Accounts.UserManager.FindByIdAsync(userId));

                var propInfo = getMember.GetType().GetProperties();
                foreach (var item in propInfo)
                {
                    if (item.CanWrite)
                    {
                        updateMembership.GetType().GetProperty(item.Name).SetValue(updateMembership, item.GetValue(getMember, null), null);
                    }
                }
                updateMembership.Id = getMember.Id;
                updateMembership.IsActive = true;
                updateMembership.IsMember = true;
                updateMembership.UserInfo.Address = getMember.UserInfo.Address;
                if(updateMembership.UserInfo.MembershipTypeId!=3) updateMembership.UserInfo.MembershipTypeId = 1; //default value for associate Member (1). Student (3)
                await Task.Run(()=>unitOfWork.Accounts.UpdateAccount(userId, updateMembership));
                
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Member, ResponseMemberDTO>(member);

                return Created(new Uri(Request.RequestUri + "/" + member.Id), resultDTO);

            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates a member
        /// </summary>
        /// <param name="memberDTO">New information about the member to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateMember/")]
        public async Task<IHttpActionResult> UpdateMember(RequestMemberDTO memberDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var member = Mapper.Map<RequestMemberDTO, Member>(memberDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Members.Find(x=>x.UserId == memberDTO.UserId).ToList().FirstOrDefault());
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    var membertoUpdate = await Task.Run(() => unitOfWork.Members.GetMemberByUserId(memberDTO.UserId));
                    member.MemberSince = membertoUpdate.MemberSince;
                    result = await Task.Run(() => unitOfWork.Members.UpdateMember(result.Id, member));

                    ApplicationUser updateMembership = new ApplicationUser();
                    ApplicationUser getMember = await Task.Run(() => unitOfWork.Accounts.UserManager.FindByIdAsync(memberDTO.UserId));

                    var propInfo = getMember.GetType().GetProperties();
                    foreach (var item in propInfo)
                    {
                        if (item.CanWrite)
                        {
                            updateMembership.GetType().GetProperty(item.Name).SetValue(updateMembership, item.GetValue(getMember, null), null);
                        }
                    }
                    updateMembership.Id = getMember.Id;
                    updateMembership.IsMember = getMember != null ? true : false;
                    updateMembership.IsActive = memberDTO.IsActive;
                    updateMembership.UserInfo.Address = getMember.UserInfo.Address;

                    _context.UpdateGraph<ApplicationUser>(updateMembership);
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Member, ResponseMemberDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a member
        /// </summary>
        /// <param name="userId">id of the member to be deleted.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeleteMember/{userId:int}")]
        [ResponseType(typeof(ResponseMemberDTO))]
        public async Task<IHttpActionResult> DeleteMember(int userId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var member = await Task.Run(() => unitOfWork.Members.Find(x=>x.UserId == userId).ToList().FirstOrDefault());
                if (member == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Members.Remove(member));

                    ApplicationUser updateMembership = new ApplicationUser();
                    ApplicationUser getMember = await Task.Run(() => unitOfWork.Accounts.UserManager.FindByIdAsync(userId));

                    var propInfo = getMember.GetType().GetProperties();
                    foreach (var item in propInfo)
                    {
                        if (item.CanWrite)
                        {
                            updateMembership.GetType().GetProperty(item.Name).SetValue(updateMembership, item.GetValue(getMember, null), null);
                        }
                    }
                    updateMembership.Id = getMember.Id;
                    updateMembership.IsMember = false;
                    updateMembership.IsActive = false;
                    updateMembership.UserInfo.Address = getMember.UserInfo.Address;
                    _context.UpdateGraph<ApplicationUser>(updateMembership);
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
