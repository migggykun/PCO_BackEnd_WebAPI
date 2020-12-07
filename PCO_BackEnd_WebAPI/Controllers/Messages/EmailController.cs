using Microsoft.AspNet.Identity;
using PCO_BackEnd_WebAPI.App_Start;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using PCO_BackEnd_WebAPI.Models.AccountBindingModels;

namespace PCO_BackEnd_WebAPI.Controllers.Mail
{
    public class EmailController : ApiController
    {
        private readonly ApplicationDbContext _context;

        string[] tags = {"<fname>",
                "<minitial>",
                "<lname>",
                "<memtype>", 
                "<org>"};

        public EmailController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// send email from frontend formatted body
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        /// <remarks>
        /// take note of the following formatting
        /// <fname> - changes to user first name
        /// <lname> - changes to user last name
        /// <minitial> - changes to user middle initial
        /// <org> - changes to user organization/school
        /// <memtype> - changes to user membership type
        /// [link text](any valid URL) - to format hyperlinks
        /// <br> - changes to end of line
        /// limit per line input by 55 char to preserve format
        /// </remarks>
        [HttpPost]
        [Route("SendEmail")]
        public async Task<IHttpActionResult> SendEmail(CustomEmailBindingModel email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UnitOfWork unitOfWork = new UnitOfWork(_context);
            EmailService es = new EmailService();
            var users = await Task.Run(() => unitOfWork.Accounts.UserManager.Users.Where(u => email.UserIds.Any(uid => u.Id == uid)));
            string messageTemplate = string.Empty;


            if (users.Count<ApplicationUser>() == 0)
            {
                string errorMessage = "User Ids do not exist";
                return BadRequest(errorMessage);
            }

            if (email.UserIds.Count != users.Count<ApplicationUser>())
            {
                string errorMessage = "Some user Ids do not exist";
                return BadRequest(errorMessage);
            }

            foreach (var u in users)
            {
                IdentityMessage im = new IdentityMessage();
                im.Subject = email.Header;
                im.Body = FormatBody(u.UserInfo, email.Header, email.Body);
                im.Destination = (u.Email);
                await es.SendAsync(im);
                //await unitOfWork.Accounts.UserManager.SendEmailAsync(u.Id, subject, body);
            }
            return Ok();
        }

        private string FormatBody(UserInfo u, string header, string message)
        {
            string _email= ReadEmailTemplate();
            string _message = message;
            while (_message.Contains("[link text]"))
            {
                string link = _message.Substring(_message.IndexOf("[link text](") + 12, _message.IndexOf(")", _message.IndexOf("[link text]("))-(_message.IndexOf("[link text](") + 12));
                _message = _message.Replace(_message.Substring(_message.IndexOf("[link text]("), link.Length + 13), string.Format("<a href = \"{0}\">{0}</a>", link));
            }

            foreach (string str in tags)
            {
                while(_message.Contains(str))
                {
                    _message = _message.Replace("<fname>", u.FirstName)
                    .Replace("<minitial>", u.MiddleName)
                    .Replace("<lname>", u.LastName)
                    .Replace("<memtype>", u.MembershipType.Name)
                    .Replace("<org>", u.Organization);
                }
            }

            _email = _email.Replace("{header}", header)
                    .Replace("{body}", _message);

            return _email;
        }
        private static string ReadEmailTemplate()
        {
            string binPath = Path.GetDirectoryName((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).LocalPath);
            string emailTemplateFullPath = binPath + @"\Resources\reminderEmailTemplate.html";
            return File.ReadAllText(emailTemplateFullPath);
        }

        
    }
}
