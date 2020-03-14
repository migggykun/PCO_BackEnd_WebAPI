﻿using PCO_BackEnd_WebAPI.Models.Accounts;
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

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class MembershipTypeController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(ResponseMembershipTypeDTO))]
        public async Task<IHttpActionResult> GetAll(string membershipType = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object result;
            if (!string.IsNullOrWhiteSpace(membershipType))
            {
                var resultDTO = await Task.Run(() => unitOfWork.MembershipTypes
                                                     .GetMembershipTypeByName(membershipType) as MembershipType);

                result = Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(resultDTO);
;           }
            else
            {
                result = await Task.Run(() =>unitOfWork.MembershipTypes.GetAll().ToList()
                                                   .Select(Mapper.Map<MembershipType, ResponseMembershipTypeDTO>));
            }
            return Ok(result);
        }

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
                return BadRequest("Error Occured, try again");
            }
            
        }

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
                    result = await Task.Run(() => unitOfWork.MembershipTypes.Update(membershipType));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(result));
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

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
                    return Ok(Mapper.Map<MembershipType, ResponseMembershipTypeDTO>(membershipType));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

    }
}
