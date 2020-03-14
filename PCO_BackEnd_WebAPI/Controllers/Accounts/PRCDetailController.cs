using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Accounts;
using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Roles;
using System.Web.Http.Cors;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class PRCDetailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PRCDetailController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> GetAll(string prcId = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object resultDTO;
            if (!string.IsNullOrWhiteSpace(prcId))
            {
                var result = await Task.Run(() => unitOfWork.PRCDetails
                                                     .GetPRCDetailById(prcId) as PRCDetail);
                resultDTO = Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result);
            }
            else
            {
                resultDTO = await Task.Run(() => unitOfWork.PRCDetails.GetAll().ToList()
                                                   .Select(Mapper.Map<PRCDetail, ResponsePRCDetailDTO>));
            }
            return Ok(resultDTO);
        }

        [HttpGet]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var membershipTypeDTO = Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result);
                return Ok(membershipTypeDTO);
            }
        }

        [HttpPost]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> AddPRCDetail(RequestPRCDetailDTO PRCDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prcDetail = Mapper.Map<RequestPRCDetailDTO, PRCDetail>(PRCDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                await Task.Run(() => unitOfWork.PRCDetails.Add(prcDetail));
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(prcDetail);
                return Created(new Uri(Request.RequestUri + "/" + prcDetail.Id), resultDTO);
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }

        }

        [HttpPut]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> UpdatePRCDetail(int id, RequestPRCDetailDTO prcDetailDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var PRCDetail = Mapper.Map<RequestPRCDetailDTO, PRCDetail>(prcDetailDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.PRCDetails.Update(PRCDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result));
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePRCDetail(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var PRCDetail = await Task.Run(() => unitOfWork.PRCDetails.Get(id));
                if (PRCDetail == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.PRCDetails.Remove(PRCDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }
    }
}
