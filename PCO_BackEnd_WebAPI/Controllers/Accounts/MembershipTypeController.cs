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
        [ResponseType(typeof(MembershipTypeDTO))]
        public async Task<IHttpActionResult> GetAll(string membershipType = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object result;
            if (!string.IsNullOrWhiteSpace(membershipType))
            {
                var resultDTO = await Task.Run(() => unitOfWork.MembershipTypes
                                                     .GetMembershipTypeByName(membershipType) as MembershipType);

                result = Mapper.Map<MembershipType, MembershipTypeDTO>(resultDTO);
;           }
            else
            {
                result = await Task.Run(() =>unitOfWork.MembershipTypes.GetAll().ToList()
                                                   .Select(Mapper.Map<MembershipType, MembershipTypeDTO>));
            }
            return Ok(result);
        }

        [HttpGet]
        [ResponseType(typeof(MembershipTypeDTO))]
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
                var membershipTypeDTO = Mapper.Map<MembershipType, MembershipTypeDTO>(result);
                return Ok(membershipTypeDTO);
            }   
        }

        [HttpPost]
        [ResponseType(typeof(MembershipTypeDTO))]
        public async Task<IHttpActionResult> AddMembershipType(MembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var membershipType = Mapper.Map<MembershipTypeDTO, MembershipType>(membershipTypeDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.MembershipTypes.Add(membershipType));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<MembershipType, MembershipTypeDTO>(membershipType);
                return Created(new Uri(Request.RequestUri + "/" + membershipType.Id), resultDTO);
            }
            catch(Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
            
        }

        [HttpPut]
        [ResponseType(typeof(MembershipTypeDTO))]
        public async Task<IHttpActionResult> UpdateMembershipType(int id, MembershipTypeDTO membershipTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var membershipType = Mapper.Map<MembershipTypeDTO, MembershipType>(membershipTypeDTO);
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
                    return Ok(Mapper.Map<MembershipType, MembershipTypeDTO>(result));
                }
               
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        [HttpDelete]
        [ResponseType(typeof(MembershipTypeDTO))]
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
                    return Ok(Mapper.Map<MembershipType, MembershipTypeDTO>(membershipType));
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
