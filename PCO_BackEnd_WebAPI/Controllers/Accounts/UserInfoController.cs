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
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [Authorize(Roles = RoleNames.ROLE_ADMINISTRATOR)]
    public class UserInfoController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UserInfoController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(UserInfoDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var resultDTO = await Task.Run(() => unitOfWork.UserInfos.GetAll());
            if (resultDTO == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(resultDTO.Select(Mapper.Map<UserInfo, UserInfoDTO>));
            }
        }

        [HttpGet]
        [ResponseType(typeof(UserInfoDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.UserInfos.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var userInfoDTO = Mapper.Map<UserInfo, UserInfoDTO>(result);
                return Ok(userInfoDTO);
            }
        }

        [HttpPut]
        [ResponseType(typeof(UserInfoDTO))]
        public async Task<IHttpActionResult> UpdateUserInfo(UserInfoDTO userInfoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userInfo = Mapper.Map<UserInfoDTO, UserInfo>(userInfoDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.UserInfos.Get(userInfo.Id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.UserInfos.Update(userInfo));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<UserInfo, UserInfoDTO>(userInfo));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
