﻿using PCO_BackEnd_WebAPI.Models.Conferences.Promos;
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

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ConferenceController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ConferenceController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets all conferences
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseConferenceDTO))]
        public async Task<IHttpActionResult> GetAll(string title = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object result;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var resultDTO = await Task.Run(() => unitOfWork.Conferences
                                                               .GetConferenceByTitle(title));

                result = Mapper.Map<Conference, ResponseConferenceDTO>(resultDTO);
;           }
            else
            {
                result = await Task.Run(() =>unitOfWork.Conferences.GetAll().ToList()
                                                   .Select(Mapper.Map<Conference, ResponseConferenceDTO>));
            }

            return Ok(result);
        }

        /// <summary>
        /// Gets list of upcoming conferences
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(List<ResponseConferenceDTO>))]
        [Route("api/Conference/GetUpcomingConferences")]
        public async Task<IHttpActionResult> GetUpcomingConferences(string date = null)
        {
            try
            {
                DateTime dateParam = DateTime.Now.Date;
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                if (!string.IsNullOrEmpty(date))
                {
                    dateParam = DateTime.Parse(date);
                }
                var result = unitOfWork.Conferences.GetUpcomingConferences(dateParam);
                return Ok(result.Select(Mapper.Map<Conference, ResponseConferenceDTO>));
            }
            catch(FormatException ex)
            {
                return BadRequest("Invalid date format");
            }
        }

        /// <summary>
        /// Gets conference based on specified id
        /// </summary>
        /// <param name="id"></param>
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
        /// <param name="conferenceDTO"></param>
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
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Updates details of conference
        /// </summary>
        /// <param name="id"></param>
        /// <param name="conferenceDTO"></param>
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
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Deletes a conference based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ResponseType(typeof(ResponseConferenceDTO))]
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
                    return Ok(Mapper.Map<Conference, ResponseConferenceDTO>(conference));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
