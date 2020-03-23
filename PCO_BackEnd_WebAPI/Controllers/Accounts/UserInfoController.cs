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

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    public class UserInfoController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public UserInfoController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of user information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseUserInfoDTO))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.UserInfos.GetPagedUserInfo(page, size));
            var resultDTO = PaginationMapper<UserInfo, ResponseUserInfoDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the user information based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseUserInfoDTO))]
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
                var userInfoDTO = Mapper.Map<UserInfo, ResponseUserInfoDTO>(result);
                return Ok(userInfoDTO);
            }
        }

        /// <summary>
        /// Updates user info based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userInfoDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(ResponseUserInfoDTO))]
        public async Task<IHttpActionResult> UpdateUserInfo(int id, RequestUserInfoDTO userInfoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userInfo = Mapper.Map<RequestUserInfoDTO, UserInfo>(userInfoDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.UserInfos.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result =  await Task.Run(() => unitOfWork.UserInfos.UpdateUserInfo(id, userInfo));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<UserInfo, ResponseUserInfoDTO>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
