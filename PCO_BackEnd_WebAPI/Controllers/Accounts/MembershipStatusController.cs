using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    public class MembershipStatusController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MembershipStatusController()
        {
            _context = new ApplicationDbContext();
        }
        
        /// <summary>
        /// Update membershipTypeId, membershipStatus, and membership activeness
        /// </summary>
        /// <param name="id"></param>
        /// <param name="membershipTypeId"></param>
        /// <param name="isMember"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateMembershipStatus/{id:int}")]
        public async Task<IHttpActionResult> UpdateMembershipStatus(int id, int? membershipTypeId = null, bool? isMember = null, bool? isActive = null)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.UserInfos.Get(id));

                UserInfo userInfo = new UserInfo();
                userInfo.Address = result.Address;
                userInfo.FirstName = result.FirstName;
                userInfo.Id = result.Id;
                userInfo.LastName = result.LastName;
                userInfo.MiddleName = result.MiddleName;
                userInfo.Organization = result.Organization;

                if (isActive == null)
                {
                    userInfo.IsActive = result.IsActive;
                }
                else
                {
                    userInfo.IsActive = isActive;
                }

                if (isMember == null)
                {
                    userInfo.IsMember = result.IsMember;
                }
                else
                {
                    userInfo.IsMember = isMember;
                }

                if (isMember == false)
                {
                    //Set to default membership type
                    userInfo.MembershipTypeId = 0;
                }
                else
                {
                    if (membershipTypeId == null)
                    {
                        userInfo.MembershipTypeId = result.MembershipTypeId;
                    }
                    else
                    {
                        userInfo.MembershipTypeId = membershipTypeId.GetValueOrDefault();
                    }
                }
          
                if (result == null)
                {
                    return BadRequest();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.UserInfos.UpdateUserInfo(id, userInfo));
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
