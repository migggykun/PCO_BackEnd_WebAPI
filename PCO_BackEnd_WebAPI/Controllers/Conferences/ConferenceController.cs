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


        [HttpGet]
        [ResponseType(typeof(ConferenceDTO))]
        public async Task<IHttpActionResult> GetAll(string title = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object result;
            if (!string.IsNullOrWhiteSpace(title))
            {
                var resultDTO = await Task.Run(() => unitOfWork.Conferences
                                                               .GetConferenceByTitle(title));

                result = Mapper.Map<Conference, ConferenceDTO>(resultDTO);
;           }
            else
            {
                result = await Task.Run(() =>unitOfWork.Conferences.GetAll().ToList()
                                                   .Select(Mapper.Map<Conference, ConferenceDTO>));
            }

            return Ok(result);
        }

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
                var membershipTypeDTO = Mapper.Map<Conference, ConferenceDTO>(result);
                return Ok(membershipTypeDTO);
            }   
        }

        [HttpPost]
        [ResponseType(typeof(ConferenceDTO))]
        public async Task<IHttpActionResult> AddConference(ConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var conference = Mapper.Map<ConferenceDTO, Conference>(conferenceDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.Conferences.Add(conference));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<Conference, ConferenceDTO>(conference);
                return Created(new Uri(Request.RequestUri + "/" + conference.Id), resultDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpPut]
        [ResponseType(typeof(ConferenceDTO))]
        public async Task<IHttpActionResult> UpdateConference(int id, ConferenceDTO conferenceDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var conference = Mapper.Map<ConferenceDTO, Conference>(conferenceDTO);
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
                   result =  await Task.Run(() => unitOfWork.Conferences.UpdateConferenceInfo(conference));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Conference, ConferenceDTO>(result));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpDelete]
        [ResponseType(typeof(ConferenceDTO))]
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
                    return Ok(Mapper.Map<Conference, ConferenceDTO>(conference));
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
