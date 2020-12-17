﻿using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    /// <summary>
    /// Controller Class for User's Personal Information
    /// </summary>
    [RoutePrefix("api/UserInfo")]
    public class UserInfoController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. Initialize Database.
        /// </summary>
        public UserInfoController()
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
        /// <param name="id">id of the user information to be fetched</param>
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
        /// <param name="id">user id</param>
        /// <param name="userInfoDTO">New information about the user to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserInfo/{id:int}")]
        [ResponseType(typeof(ResponseUserInfoDTO))]
        public async Task<IHttpActionResult> UpdateUserInfo(int id, RequestUserInfoDTO userInfoDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
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
                var errorMessage = ex.Message;
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
