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
using PCO_BackEnd_WebAPI.Models.Images;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Images.Manager;
using PCO_BackEnd_WebAPI.Security.Authorization;
using PCO_BackEnd_WebAPI.Models.Roles;
namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    /// <summary>
    /// Controller for Conference
    /// </summary>
    public class ConferenceController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Default Constructor. Initialize Database.
        /// </summary>
        public ConferenceController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all list of Reference
        /// </summary>
        /// <param name="model">details of query</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> GetAll([FromUri] ConferenceParameterBindingModel model)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() =>unitOfWork.Conferences.GetPagedConferences((int)model.Page, (int)model.Size, model.Title,
                                                                                        model.Day, model.Month, model.Year,
                                                                                        model.FromDate, model.ToDate));
            //Convert to DTO
            var resultDTO = ConferenceMapper.MapToPagedResponseConferenceDTO(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets conference based on specified id
        /// </summary>
        /// <param name="id">id of the conference to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
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
                var conferenceDTO = ConferenceMapper.MapToResponseConferenceDTO(result);
                return Ok(conferenceDTO);
            }   
        }

        /// <summary>
        /// Adds a conference
        /// </summary>
        /// <param name="conferenceDTO">Details about the conference to be added.</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> AddConference(AddConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conference = Mapper.Map<AddConferenceDTO, Conference>(conferenceDTO);
            string errorMessage = string.Empty;
            bool isValidSchedule = ConferenceValidator.IsValidSchedule(conference, out errorMessage);

            if (isValidSchedule == false)
            {
                return BadRequest(errorMessage);
            }

            try
            {
                ImageManager imageManager;
                if (!string.IsNullOrEmpty(conferenceDTO.Banner))
                {
                    imageManager = new ImageManager(conferenceDTO.Banner);
                    conference.Banner = new Banner();
                    conference.Banner.Image = imageManager.Bytes;
                }

                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Conferences.Add(conference));
                await Task.Run(() => unitOfWork.Complete());
                await Task.Run(() => unitOfWork.Conferences.FillInConferenceActivities(conference));

                
                var resultDTO = ConferenceMapper.MapToResponseConferenceDTO(conference);
                return Created(new Uri(Request.RequestUri + "/" + conference.Id), resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates details of conference
        /// </summary>
        /// <param name="id">id of the conference to be updated</param>
        /// <param name="conferenceDTO">New information about the conference to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/UpdateConference/{id:int}")]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> UpdateConference(int id, UpdateConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conference = Mapper.Map<UpdateConferenceDTO, Conference>(conferenceDTO);
            string errorMessage = string.Empty;
            bool isValidSchedule = ConferenceValidator.IsValidSchedule(conference, out errorMessage,id);

            if(isValidSchedule == false)
            {
                return BadRequest(errorMessage);
            }

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
                   result =  await Task.Run(() => unitOfWork.Conferences.UpdateConferenceInfo(id, conference, conferenceDTO.Banner));
                   await Task.Run(() => unitOfWork.Complete());
                   var resultDTO = ConferenceMapper.MapToResponseConferenceDTO(result);
                   return Ok(resultDTO);
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a conference based on specified id
        /// </summary>
        /// <param name="id">id of the conference to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/DeleteConference/{id:int}")]
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
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }
    }
}
