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
    public class PRCDetailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PRCDetailController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of PRCDetails
        /// </summary>
        /// <param name="prcId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponsePRCDetailDTO))]
        public async Task<IHttpActionResult> GetAll(string prcId = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            object resultDTO;
            if (!string.IsNullOrWhiteSpace(prcId))
            {
                resultDTO = await Task.Run(() => unitOfWork.PRCDetails.GetAll().Where(m => m.IdNumber.Contains(prcId)).ToList()
                                                         .Select(Mapper.Map<PRCDetail,ResponsePRCDetailDTO>));
            }
            else
            {
                resultDTO = await Task.Run(() => unitOfWork.PRCDetails.GetAll().ToList()
                                                   .Select(Mapper.Map<PRCDetail, ResponsePRCDetailDTO>));
            }
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the PRC details based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds PRC details
        /// </summary>
        /// <param name="PRCDetailDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates PRC detail based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="prcDetailDTO"></param>
        /// <returns></returns>
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
                    result = await Task.Run(() => unitOfWork.PRCDetails.Update(id, PRCDetail));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<PRCDetail, ResponsePRCDetailDTO>(result));
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Error Occured, try again");
            }
        }

        /// <summary>
        /// Deletes PRC detail based on specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
