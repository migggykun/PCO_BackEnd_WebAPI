
using PCO_BackEnd_WebAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.TestAPIs
{
    public class TestController : ApiController
    {
        private readonly ApplicationDbContext _context;
        
        public TestController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            //Activities
            //var result =  await Task.Run(() => _context.Activities.ToList());
            //return Ok(result);

            //ActivitySchedule
            //var result =  await Task.Run(() => _context.ActivitySchedules.ToList());
            //return Ok(result);

            //ActivitySchedule
            //var result = await Task.Run(() => _context.ConferenceActivities.ToList());
            //return Ok(result);


            //ConferenceDays
            //var result =  await Task.Run(() => _context.ConferenceDays.ToList());
            //return Ok(result);

            //Conference
            var result =  await Task.Run(() => _context.Conferences.ToList());
            return Ok(result);


        } 
    }
}
