using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    public class ConferencesController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ConferencesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult AddConference(Conference conference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            _context.Conferences.Add(conference);
            unitOfWork.Complete();
            return Ok();
        }
    }
}
