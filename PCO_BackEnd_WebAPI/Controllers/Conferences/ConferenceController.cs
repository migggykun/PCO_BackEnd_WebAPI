using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using System.Web.Http.Description;
using System.Threading.Tasks;
using AutoMapper;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.ParameterBindingModels;
namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    [RoutePrefix("api/Conference")]
    public class ConferenceController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ConferenceController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all list of Reference
        /// </summary>
        /// <param name="title">filter return by conference title</param>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> GetAll([FromUri] ConferenceParameterBindingModel model)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() =>unitOfWork.Conferences.GetPagedConferences((int)model.Page, (int)model.Size, model.Title,
                                                                                        model.Day, model.Month, model.Year,
                                                                                        model.FromDate, model.ToDate));
            var resultDTO = PaginationMapper<Conference, ResponseConferenceDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets conference based on specified id
        /// </summary>
        /// <param name="id">id of the conference to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Conferences.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var membershipTypeDTO = Mapper.Map<Conference, ResponseConferenceDTO>(result);
                return Ok(membershipTypeDTO);
            }   
        }

        /// <summary>
        /// Adds a conference
        /// </summary>
        /// <param name="conferenceDTO">Details about the conference to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> AddConference(AddConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conference = Mapper.Map<AddConferenceDTO, Conference>(conferenceDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Conferences.Add(conference));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Conference, ResponseConferenceDTO>(conference);
                return Created(new Uri(Request.RequestUri + "/" + conference.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates details of conference
        /// </summary>
        /// <param name="id">id of the conference to be updated</param>
        /// <param name="conferenceDTO">New information about the conference to be updated</param>
        /// <returns></returns>
        [HttpPut]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> UpdateConference(int id, UpdateConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conference = Mapper.Map<UpdateConferenceDTO, Conference>(conferenceDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Conferences.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                   result =  await Task.Run(() => unitOfWork.Conferences.UpdateConferenceInfo(id, conference));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Conference, ResponseConferenceDTO>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ExceptionManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a conference based on specified id
        /// </summary>
        /// <param name="id">id of the conference to be deleted</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteConference(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var conference = await Task.Run(() => unitOfWork.Conferences.Get(id));
                if (conference == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Conferences.Remove(conference));
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
