using Newtonsoft.Json;
using PCO_BackEnd_WebAPI.Models.AccountBindingModels;
using PCO_BackEnd_WebAPI.Models.Accounts;
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
        public async Task<IHttpActionResult> SendSMS(SMSBindingModel sms)
        {

            string messageTemplate = string.Empty;
            var users = _context.Users.Where(u => sms.UserIds.Any(uId => uId == u.Id));

            foreach (var u in users)
            {
                var param = new
                {
                    apikey = "dfb7ef7a3607feef3d9373d84a55da0e",
                    number = u.PhoneNumber,
                    message = FormatSMS(sms.Message, u.UserInfo)
                };

                var json = JsonConvert.SerializeObject(param);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = "https://api.semaphore.co/api/v4/messages";
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;
                }
            }
            return Ok();
        }

        private string FormatSMS(string SMS, UserInfo u = null)
        {
            SMS = SMS.Replace("<fname>", u.FirstName)
                    .Replace("<lname>", u.LastName)
                    .Replace("<minitial>", u.MiddleName)
                    .Replace("<memtype>", u.MembershipType.Name)
                    .Replace("<org>", u.Organization);
            return SMS;
        }
    }
}