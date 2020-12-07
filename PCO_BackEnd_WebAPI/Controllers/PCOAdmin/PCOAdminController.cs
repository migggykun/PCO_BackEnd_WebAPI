using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Conferences;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    public class PCOAdminController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PCOAdminController()
        {
            _context = new ApplicationDbContext();
        }


        [HttpGet]
        [Route("api/GetAnnualFee")]
        public async Task<IHttpActionResult> GetAnnualFee()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.GetAnnualFee());
            return Ok(result);
        }

        [HttpPost]
        [Route("api/SetAnnualFee")]
        public async Task<IHttpActionResult> SetAnnualFee(double newAnnualFee)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.UpdatePCOAdminDetails(newAnnualFee));
            await Task.Run(()=>unitOfWork.Complete());
            return Ok(result);
        }


        [HttpGet]
        [Route("api/GetPassword")]
        public async Task<IHttpActionResult> GetPassword()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.GetPassword());
            return Ok(result);
        }

        [HttpPost]
        [Route("api/SetPassword")]
        public async Task<IHttpActionResult> SetPassword(string newPassword)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.UpdatePCOAdminDetails(null, newPassword));
            return Ok(result);
        }

    }
}
