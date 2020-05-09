using Newtonsoft.Json;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Messages
{
    public class SMSController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public SMSController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> SendSMS(List<int> userIds)
        {

            string messageTemplate = "Hi {0}! \n You may pay your registration fee through our BDO Account. \n. Thanks";
            var users = _context.Users.Where(u => userIds.Any(uId => uId == u.Id));
            int orgIdCode = 10131;

            foreach (var u in users)
            {
                var param = new
                {
                    orgId = orgIdCode,
                    to = u.PhoneNumber,
                    message = string.Format(messageTemplate, u.UserInfo.FirstName)
                };

                var json = JsonConvert.SerializeObject(param);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = "https://api.engagespark.com/v1/sms/contact";
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Token", "3312456fe33d545e52cc2a467fee6f663f59b629");
                    var response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
                }
            }
            return Ok();
        }
    }
}
