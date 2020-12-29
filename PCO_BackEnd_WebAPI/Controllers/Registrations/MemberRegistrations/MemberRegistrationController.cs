using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Registrations;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Models.ViewModels;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.MemberRegistrations
{
    /// <summary>
    /// Controller for membership registrations (be a member)
    /// </summary>
    public class MemberRegistrationController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public MemberRegistrationController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get list of registration
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="aStatusId">filter results by registration status</param>
        /// <param name="userId">filter results by user Id</param>
        /// <param name="akeywordFilter">filter results by search keyword</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(List<ResponseMemberRegistrationDTO>))]
        public async Task<IHttpActionResult> GetAll(int page = 1, 
                                                    int size = 0, 
                                                    int? aStatusId = null,
                                                    int? userId = null,
                                                    string akeywordFilter = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var memberRegistrationList = await Task.Run(() => unitOfWork.MemberRegistrations.GetPagedMemberRegistration(page, size, aStatusId, userId, akeywordFilter));
            var memberRegistrationListDTO = PaginationMapper<MemberRegistration, ResponseListMemberRegistrationDTO>.MapResult(memberRegistrationList);

            return Ok(memberRegistrationListDTO);
        }

        /// <summary>
        /// Gets registration based on specified id
        /// </summary>
        /// <param name="id">id of the registration to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.MemberRegistrations.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var memberRegistrationDTO = Mapper.Map<MemberRegistration, ResponseMemberRegistrationDTO>(result);
                return Ok(memberRegistrationDTO);
            }   
        }

        /// <summary>
        /// Add a registration
        /// </summary>
        /// <param name="memberRegistrationDTO">Details about the registration to be added</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseMemberRegistrationDTO))]
        public async Task<IHttpActionResult> Add(RequestMemberRegistrationDTO memberRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var memberRegistration = Mapper.Map<RequestMemberRegistrationDTO, MemberRegistration>(memberRegistrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.MemberRegistrations.Add(memberRegistration));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<MemberRegistration, ResponseMemberRegistrationDTO>(memberRegistration);
                return Created(new Uri(Request.RequestUri + "/" + memberRegistration.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates registration information
        /// </summary>
        /// <param name="id"></param>
        /// <param name="memberRegistrationDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateMemberRegistration/{id:int}")]
        [ResponseType(typeof(ResponseMemberRegistrationDTO))]
        public async Task<IHttpActionResult> Update(int id, RequestMemberRegistrationDTO memberRegistrationDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }
            var memberRegistration = Mapper.Map<RequestMemberRegistrationDTO, MemberRegistration>(memberRegistrationDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.MemberRegistrations.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.MemberRegistrations.Update(result, memberRegistration));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<MemberRegistration, ResponseMemberRegistrationDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a registration
        /// </summary>
        /// <param name="id">id of registration to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeleteMemberRegistration/{id:int}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var memberRegistration = await Task.Run(() => unitOfWork.MemberRegistrations.Get(id));
                if (memberRegistration == null)
                {
                    return NotFound();
                }
                else
                {
                    Payment paymentToDelete = await Task.Run(() => unitOfWork.Payments.Find(x => x.MemberRegistrationId == id).FirstOrDefault());
                    if (paymentToDelete != null) await Task.Run(() => unitOfWork.Payments.Remove(paymentToDelete));
                    await Task.Run(() => unitOfWork.MemberRegistrations.Remove(memberRegistration));
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

        /// <summary>
        /// Change registration status
        /// </summary>
        /// <param name="model">registration id and new registration status</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SetMemberRegistrationStatus")]
        public async Task<IHttpActionResult> SetMemberRegistrationStatus(SetRegistrationViewModel model)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                MemberRegistration result = await Task.Run(() => unitOfWork.MemberRegistrations.SetRegistrationStatus(model.RegistrationId, model.Status));
                unitOfWork.Complete();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest("Error occured. Try Again.");
            }
        }

        /// <summary>
        /// Get Member registration with the user's id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns></returns>
        [HttpGet]      
        [Route("api/GetMemberRegistration/{userId=userId}")]
        [ResponseType(typeof(ResponseMemberRegistrationDTO))]
        public async Task<IHttpActionResult> GetMemberRegistration(int userId)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run (()=>unitOfWork.MemberRegistrations.GetMemberRegistration(userId));

                var memberRegistrationDTO = Mapper.Map<MemberRegistration, ResponseMemberRegistrationDTO>(result);
                return Ok(memberRegistrationDTO);
            }
            catch (Exception)
            {
                return BadRequest("Error occured. Try Again.");
            }
        }


        /// <summary>
        /// Gets RegistrationFee
        /// </summary>
        ///
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetMemberRegistrationFee")]
        public async Task<IHttpActionResult> GetMemberRegistrationFee(bool isMember)
        {
            double registrationFee;
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            registrationFee = await Task.Run(()=>unitOfWork.PCOAdminDetail.GetAnnualFee(isMember));

            return Ok(registrationFee);
        }

        /// <summary>
        /// Reset all members to isactive false
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ResetMemberRegistrations")]
        public async Task<IHttpActionResult> ResetMemberRegistrations()
        {
            int archivedValue = 6;
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var memberRegistrations = await Task.Run(()=>unitOfWork.MemberRegistrations.GetAll().ToList());
            var members = await Task.Run(()=>unitOfWork.Members.GetAll().ToList());
            try
            {
                foreach(var memberRegistration in memberRegistrations)
                {
                    MemberRegistration newMemberRegistration = new MemberRegistration();
                    newMemberRegistration.MemberRegistrationStatusId = archivedValue;
                    unitOfWork.MemberRegistrations.UpdateStatus(memberRegistration, newMemberRegistration);
                   
                }
                foreach(var member in members)
                {
                    Member newMember = new Member();
                    newMember.Id = member.Id;
                    newMember.IsActive = false;
                    newMember.MemberSince = member.MemberSince;
                    newMember.UserId = member.UserId;
                    unitOfWork.Members.UpdateMember(newMember.Id, newMember);
                }
                unitOfWork.Complete();

            }
            catch(Exception e)
            {
                var errorMessage = e.Message;
                return BadRequest();
            }
            return Ok();
        }
    }
}
