using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Security.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Security.Controller
{
    public class SecurityController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public SecurityController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetToken(string username, string password)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            //Validate account
            var user = await Task.Run( () => unitOfWork.Accounts.UserManager.FindByNameAsync(username));
            if (user == null)
            {
                return NotFound();
            }

            if (await Task.Run(() => unitOfWork.Accounts.UserManager.CheckPasswordAsync(user, password)))
            {
                string token = TokenManager.GenerateToken(user.UserName, user.IsAdmin);
                UserInformationDTO resultDTO = new UserInformationDTO()
                {
                    Token = token,
                    IsAdmin = user.IsAdmin
                };
                return Ok(resultDTO);
            }
            else
            {
                return BadRequest("Username or Password is invalid.");
            }
        }

        [HttpGet]
        [Route("GetUsernameInToken")]
        public async Task<IHttpActionResult> GetUsernameInToken(string token)
        {
            string result = TokenManager.ValidateToken(token);
            return Ok(result);
        }
    }
}