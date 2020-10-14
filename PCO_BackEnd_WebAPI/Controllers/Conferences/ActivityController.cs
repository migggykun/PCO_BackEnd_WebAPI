using AutoMapper;
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
using System.Web.Http.Description;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    public class ActivityController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public ActivityController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Gets list of activities
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> GetAll(int page = 1, int size = 0)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Activities.GetPagedActivities(page, size));
            var resultDTO = PaginationMapper<Activity, Activity>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Gets the activity information based on specified id
        /// </summary>
        /// <param name="id">id of rate to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> Get(int id)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.Activities.Get(id));
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                var rateDTO = Mapper.Map<Activity, Activity>(result);
                return Ok(rateDTO);
            }
        }

        /// <summary>
        /// Get list of activities
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        [HttpPost]
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> AddActivities(List<Activity> activity)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var activities = activity.Select(Mapper.Map<Activity, Activity>).ToList();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);

                if (activities.Count() == 1)
                {
                    await Task.Run(() => unitOfWork.Activities.Add(activities[0]));
                }
                else
                {
                    await Task.Run(() => unitOfWork.Activities.AddRates(activities));
                }
                await Task.Run(() => unitOfWork.Complete());
                var resultDTO = activities.Select(Mapper.Map<Activity, Activity>);
                return Created(string.Empty, resultDTO);
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Updates a activity
        /// </summary>
        /// <param name="id">user id</param>
        /// <param name="activityDTO">New information about the activity to be updated</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UpdateActivity/{id:int}")]
        public async Task<IHttpActionResult> UpdateRate(int id, Activity activityDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var activity = Mapper.Map<Activity, Activity>(activityDTO);
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var result = await Task.Run(() => unitOfWork.Activities.Get(id));
                if (result == null)
                {
                    return NotFound();
                }
                else
                {
                    result = await Task.Run(() => unitOfWork.Activities.UpdateRate(id, activity));
                    await Task.Run(() => unitOfWork.Complete());
                    return Ok(Mapper.Map<Activity, Activity>(result));
                }
            }
            catch (Exception ex)
            {
                string message = ErrorManager.GetInnerExceptionMessage(ex);
                return BadRequest(message);
            }
        }

        /// <summary>
        /// Deletes a activity
        /// </summary>
        /// <param name="id">id of the activity to be deleted.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/DeleteActivity/{id:int}")]
        [ResponseType(typeof(Activity))]
        public async Task<IHttpActionResult> DeleteActivity(int id)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(_context);
                var activity = await Task.Run(() => unitOfWork.Activities.Get(id));
                if (activity == null)
                {
                    return NotFound();
                }
                else
                {
                    await Task.Run(() => unitOfWork.Activities.Remove(activity));
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
