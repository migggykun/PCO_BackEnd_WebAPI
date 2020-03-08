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

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    [Authorize(Roles = RoleNames.ROLE_ADMINISTRATOR)]
    public class MembershipTypeAssignmentController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public MembershipTypeAssignmentController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(MembershipAssignmentDTO))]
        public async Task<IHttpActionResult> GetAll()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var resultDTO = await Task.Run(() => unitOfWork.MembershipAssignments.GetAll().ToList()
                                                   .Select(Mapper.Map<MembershipAssignment, MembershipAssignmentDTO>));
            return Ok(resultDTO);
        }

        [HttpGet]
        [ResponseType(typeof(MembershipAssignmentDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.MembershipAssignments.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var membershipTypeDTO = Mapper.Map<MembershipAssignment, MembershipAssignmentDTO>(result);
                return Ok(membershipTypeDTO);
            }
        }

        [HttpPut]
        [ResponseType(typeof(MembershipAssignmentDTO))]
        public async Task<IHttpActionResult> UpdateMembershipType(MembershipAssignmentDTO membershipAssignmentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var membershipAssignment = Mapper.Map<MembershipAssignmentDTO, MembershipAssignment>(membershipAssignmentDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.MembershipAssignments.Get(membershipAssignment.Id));
                if (result == null)
                {
                    return BadRequest();
                }
                else
                {
                    await Task.Run(() => unitOfWork.MembershipAssignments.Update(membershipAssignment));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<MembershipAssignment, MembershipAssignmentDTO>(membershipAssignment));
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
