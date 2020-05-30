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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        ///         /// <remarks>
        /// take note of the following formatting
        /// <fname> - changes to user first name
        /// <lname> - changes to user last name
        /// <minitial> - changes to user middle initial
        /// <org> - changes to user organization/school
        /// <memtype> - changes to user membership type
        /// \n - changes to end of line
        /// limit per line input by 55 char to preserve format
        /// </remarks>
        [HttpPost]
        public async Task<IHttpActionResult> SendSMS(SMSBindingModel sms)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string messageTemplate = string.Empty;
            var users = _context.Users.Where(u => sms.UserIds.Any(uId => uId == u.Id));

            if (users.Count<ApplicationUser>() == 0)
            {
                string errorMessage = "User Ids do not exist";
                return BadRequest(errorMessage);
            }

            if (sms.UserIds.Count != users.Count<ApplicationUser>())
            {
                string errorMessage = "Some user Ids do not exist";
                return BadRequest(errorMessage);  
            }

            string failedSmsMessage = "Failed to send Sms to the following users: ";
            bool isSomeSmsFails = false;

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
                    
                    if(response.IsSuccessStatusCode == false)
                    {
                       failedSmsMessage += u.Email + ",";
                       isSomeSmsFails = true;
                    }
                }
            }

            if (isSomeSmsFails)
            {
                return Ok(failedSmsMessage);
            }
            else
            {
                return Ok();
            }
        }

        private string FormatSMS(string SMS, UserInfo u = null)
        {
           string[] tags = {"<fname>",
                "<minitial>",
                "<lname>",
                "<memtype>", 
                "<org>"};

            foreach(string str in tags)
            {
                SMS = SMS.Replace("<fname>",u.FirstName)
                    .Replace("<lname>", u.LastName)
                    .Replace("<minitial>", u.MiddleName)
                    .Replace("<memtype>", u.MembershipType.Name)
                    .Replace("<org>", u.Organization);

            }
            return SMS;
        }
    }
}