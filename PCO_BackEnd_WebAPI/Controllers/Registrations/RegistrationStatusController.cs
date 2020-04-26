using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Registrations
{
    [RoutePrefix("api/RegistrationStatus")]
    public class RegistrationStatusController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public RegistrationStatusController()
        {
            _context = new ApplicationDbContext();
        }

        [CustomAuthorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetStatusRegistration()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.RegStatus.GetAll().ToList());
            return Ok(unitOfWork.RegStatus.GetAll().ToList());
        }
    }
}