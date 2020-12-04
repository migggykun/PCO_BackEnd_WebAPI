using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using PCO_BackEnd_WebAPI.Models.AccountBindingModels;
using PCO_BackEnd_WebAPI.Models.AccountViewModels;
using PCO_BackEnd_WebAPI.Providers;
using PCO_BackEnd_WebAPI.Results;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Entities;
using System.Linq;
using AutoMapper;
using PCO_BackEnd_WebAPI.DTOs.Accounts;
using PCO_BackEnd_WebAPI.Models.Roles;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Helpers;
using PCO_BackEnd_WebAPI.Security;
using PCO_BackEnd_WebAPI.Security.DTO;
using Newtonsoft.Json;
using System.Text;
using PCO_BackEnd_WebAPI.Models.Email;
using PCO_BackEnd_WebAPI.Models.ViewModels;
using System.Text.RegularExpressions;

namespace PCO_BackEnd_WebAPI.Controllers.Accounts
{
    [AllowAnonymous]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        

        #region Use WebAPI Methods

        /// <summary>
        /// Sends email to user to confirm registered email address.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendEmailConfirmation/{id:int}")]
        public async Task<IHttpActionResult> SendEmailConfirmation(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await Task.Run(() => SendEmail(user.Id, (int)EmailClassification.CONFIRM_EMAIL));
            }

            return Ok();
        }

        /// <summary>
        /// Sends email to user to confirm registered email address.
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendPhoneConfirmation/{id:int}")]
        public async Task<IHttpActionResult> SendPhoneConfirmation(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await Task.Run(() => SendSMS(id, (int)SMSClassification.CONFIRM_PHONE));
            }

            return Ok();
        }

        /// <summary>
        /// Send Email to use using SMTP
        /// </summary>
        /// <param name="aEmail">email address</param>
        /// <param name="emailClassification">Checker if email template for reset password or confirm email will be passed.</param>
        /// <returns>Void</returns>
        private async Task SendEmail(int id, int emailClassification)
        {
            BaseEmailService emailService;
            if (emailClassification == (int)EmailClassification.CONFIRM_EMAIL)
            {
                emailService = new ConfirmEmailService(id);
                await emailService.SendEmail(UserManager);
            }

            else if (emailClassification == (int)EmailClassification.RESET_PASSWORD)
            {
                emailService = new ResetPasswordService(id);
                await emailService.SendEmail(UserManager);
            }
            else
            {
                //Do Nothing
            }
        }

        private async Task SendSMSAsync(int id, string phoneNumber, string message)
        {
            var u = await UserManager.FindByIdAsync(id);
            var param = new
            {
                apikey = "dfb7ef7a3607feef3d9373d84a55da0e",
                number = u.PhoneNumber,
                message = FormatSMS(message, u.UserInfo),
                sendername = "PCOAdvisory"
            };

            var json = JsonConvert.SerializeObject(param);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var url = "https://api.semaphore.co/api/v4/messages";
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result;

                //if (response.IsSuccessStatusCode == false)
                //{
                //    failedSmsMessage += u.Email + ",";
                //    isSomeSmsFails = true;
                //}
            }
        }

        private string FormatSMS(string SMS, UserInfo u = null)
        {
            string[] tags = {"<fname>",
                "<minitial>",
                "<lname>",
                "<memtype>",
                "<org>"};


            foreach (string str in tags)
            {
                if (u.FirstName != null) SMS = SMS.Replace("<fname>", u.FirstName);
                if (u.MiddleName != null) SMS = SMS.Replace("<minitial>", u.MiddleName);
                if (u.LastName != null) SMS = SMS.Replace("<lname>", u.LastName);
                if (u.MembershipType != null) SMS = SMS.Replace("<memtype>", u.MembershipType.Name);
                if (u.Organization != null) SMS = SMS.Replace("<org>", u.Organization);

            }
            return SMS;
        }

        /// <summary>
        /// Send Email to use using SMTP
        /// </summary>
        /// <param name="aEmail">email address</param>
        /// <param name="emailClassification">Checker if email template for reset password or confirm email will be passed.</param>
        /// <returns>Void</returns>
        private async Task SendSMS(int id, int emailClassification)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (emailClassification == (int)SMSClassification.CONFIRM_PHONE)
            {
                string code = await UserManager.GenerateChangePhoneNumberTokenAsync(id, user.PhoneNumber);
                string idToken = StringManipulationHelper.SetParameterPhone(user.PhoneNumber, code, user.IsAdmin);
                string callbackURL = StringManipulationHelper.SetConfirmPhoneURL(idToken, user.IsAdmin);

                var message = "Congratulations <fname> <lname>! To Complete your registration, please confirm your Account by clicking the link: " + callbackURL;
                await SendSMSAsync(id, user.PhoneNumber, message);
  
            }

            if (emailClassification == (int)SMSClassification.RESET_PASSWORD)
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(id);
                string idToken = StringManipulationHelper.SetParameter(user.Email, code, user.IsAdmin);
                string callbackURL = StringManipulationHelper.SetResetPasswordURL(idToken, user.IsAdmin);

                var message = "To Reset your password, please click the link: " + callbackURL;
                await SendSMSAsync(id, user.PhoneNumber, message);
            }
        }

        /// <summary>
        /// Sends link to the specified email on how to reset password.
        /// </summary>
        /// <param name="email">Email of user to reset</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendResetPasswordEmail")]
        public async Task<IHttpActionResult> SendResetPasswordEmail(string email)
        {
            bool isEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
                 
            if (isEmail)
            {
                var user = await UserManager.FindByEmailAsync(email);
                await Task.Run(() => SendEmail(user.Id, (int)EmailClassification.RESET_PASSWORD));
            }
            else
            {
                UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
                var user = await Task.Run(() => unitOfWork.Accounts.GetUserByPhoneNumber(email));
                await Task.Run(() => SendSMS(user.Id, (int)SMSClassification.RESET_PASSWORD));
            }        

            return Ok();
        }

        /// <summary>
        /// Reset users password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ApplicationUser user = null;
            bool isEmail = Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
                user = await Task.Run(() => unitOfWork.Accounts.GetUserByPhoneNumber(model.Email));
            }
            else
            {
                user = await UserManager.FindByEmailAsync(model.Email);
            }
                  
            if (user == null)
            {
                return NotFound();
            }
            IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user.Id);
                return Ok();
            }
            else
            {

                return BadRequest(result.Errors.ToList()[0].ToString());
            }
        }

        /// <summary>
        /// Sets the user account as confirmed.
        /// </summary>
        /// <param name="model">userId: id of user<br/>token: Generate token to confirm email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            IdentityResult result = result = await UserManager.ConfirmEmailAsync(user.Id, model.Token);
            if (result.Succeeded)
            {
                await UserManager.UpdateSecurityStampAsync(user.Id);
                return Ok();
            }
            else
            {
                return BadRequest(result.Errors.ToList()[0].ToString());
            }
        }

        /// <summary>
        /// Sets the user account as confirmed.
        /// </summary>
        /// <param name="model">userId: id of user<br/>token: Generate token to confirm email</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ConfirmPhone")]
        public async Task<IHttpActionResult> ConfirmPhone(ConfirmSmsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
            var user = await Task.Run(() => unitOfWork.Accounts.GetUserByPhoneNumber(model.PhoneNumber));

            bool result = result = await UserManager.VerifyChangePhoneNumberTokenAsync(user.Id, model.Token, model.PhoneNumber);
            if (result)
            {
                user.PhoneNumberConfirmed = true;
                await Task.Run(() => unitOfWork.Accounts.UpdateAccount(user.Id, user));
                await UserManager.UpdateSecurityStampAsync(user.Id);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // POST api/Account/Register
        /// <summary>
        /// Creates a user account
        /// </summary>
        /// <param name="model">If no PRC Details to be specified, set values to empty string(""). otherwise, set its value</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        [ResponseType(typeof(ResponseAccountDTO))]
        public async Task<IHttpActionResult> Register(UserAccountBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
            var user = new ApplicationUser()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PRCDetail = Mapper.Map<RequestPRCDetailDTO, PRCDetail>(model.PrcDetail),
                UserInfo = Mapper.Map<RequestUserInfoDTO, UserInfo>(model.UserInfo),
                IsAdmin = model.IsAdmin,
                MemberSince = DateTime.Now.Date,
                IsActive = false,
                IsMember = false
            };
            try
            {
                if (unitOfWork.Accounts.GetUserByPhoneNumber(user.PhoneNumber) != null)
                {
                    IdentityResult existingPhone = new IdentityResult();
                    ModelState.AddModelError("", string.Format("User with {0} Phone Number already exists!",user.PhoneNumber));
                    string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                    return BadRequest(errorMessages);
                }

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                await Task.Run(() => SendEmail(user.Id, (int)EmailClassification.CONFIRM_EMAIL));
                await Task.Run(() => SendSMS(user.Id, (int)SMSClassification.CONFIRM_PHONE));

                return Ok(Mapper.Map<ApplicationUser, ResponseAccountDTO>(user));
            }
            catch (Exception ex)
            {
               string message = ErrorManager.GetAllExceptionMessages(ex);
               return BadRequest(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByPhoneOrEmail")]
        public async Task<IHttpActionResult> GetUserByPhoneOrEmail(string phoneOrEmail)
        {
            bool isEmail = Regex.IsMatch(phoneOrEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            if(isEmail)
            {
                var user = await UserManager.FindByEmailAsync(phoneOrEmail);
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    var resultDTO = Mapper.Map<ApplicationUser, ResponseAccountDTO>(user);
                    return Ok(resultDTO);
                }
            }
            else
            {
                UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
                var user = await Task.Run(()=> unitOfWork.Accounts.GetUserByPhoneNumber(phoneOrEmail));
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    var resultDTO = Mapper.Map<ApplicationUser, ResponseAccountDTO>(user);
                    return Ok(resultDTO);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByPhone")]
        public async Task<IHttpActionResult> GetUserByPhone(string phoneNumber)
        {
            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
            var user = await Task.Run(() => unitOfWork.Accounts.GetUserByPhoneNumber(phoneNumber));
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                var resultDTO = Mapper.Map<ApplicationUser, ResponseAccountDTO>(user);
                return Ok(resultDTO);
            }
        }

        // POST api/Account/ChangePassword
        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }


        /// <summary>
        /// Returns list of accounts
        /// </summary>
        /// <param name="page">nth page of list. Default value: 1</param>
        /// <param name="size">count of item to return in a page. Returns all record if not specified</param>
        /// <param name="keywordFilter">filter of returned items</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllUsers")]
        [ResponseType(typeof(List<ResponseAccountDTO>))]
        public async Task<IHttpActionResult> GetUsers(string keywordFilter = null, 
                                                      int page = 1, 
                                                      int size = 0,
                                                      string organization = null,
                                                      string province = null,
                                                      string membershipType = null,
                                                      bool? isMember = null,
                                                      bool? isActive = null)
        {
            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
            var result = await Task.Run(() => unitOfWork.Accounts.GetPagedAccounts(page,
                                                                                   size,
                                                                                   keywordFilter,
                                                                                   organization,
                                                                                   province,
                                                                                   membershipType,
                                                                                   isMember,
                                                                                   isActive));

            var resultDTO = PaginationMapper<ApplicationUser, ResponseAccountDTO>.MapResult(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// Get user details by email
        /// </summary>
        /// <param name="email">Registered email of user</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                var resultDTO = Mapper.Map<ApplicationUser, ResponseAccountDTO>(user);
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// Gets user based on specified id
        /// </summary>
        /// <param name="id">id of account to be fetched</param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(ResponseAccountDTO))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(Mapper.Map<ApplicationUser, ResponseAccountDTO>(user));
            }
        }

        /// <summary>
        /// Updates user Account
        /// </summary>
        /// <param name="id">user Id</param>
        /// <param name="accountDTO">Request body of user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUser/{id:int}")]
        [ResponseType(typeof(ResponseAccountDTO))]
        public async Task<IHttpActionResult> UpdateUser(int id, RequestAccountDTO accountDTO)
        {
            if (!ModelState.IsValid)
            {
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            var user = Mapper.Map<RequestAccountDTO, ApplicationUser>(accountDTO);
            UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());

            var userToUpdate = await unitOfWork.Accounts.UserManager.FindByIdAsync(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            else if (unitOfWork.Accounts.GetUserByPhoneNumber(user.PhoneNumber) != null && (userToUpdate.PhoneNumber != user.PhoneNumber))
            {
                ModelState.AddModelError("", string.Format("User with {0} Phone Number already exists!",user.PhoneNumber));
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }
            else if (unitOfWork.Accounts.UserManager.FindByEmail(user.Email) != null && (userToUpdate.Email != user.Email))
            {
                ModelState.AddModelError("", string.Format("User with {0} Email already exists!", user.Email));
                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }
            else
            {
                var result = unitOfWork.Accounts.UpdateAccount(id, user);
                var resultDTO = Mapper.Map<ApplicationUser, ResponseAccountDTO>(result);
                unitOfWork.Complete();
                return Ok(resultDTO);
            }
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="id">user of id to be deleted</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteUser/{id:int}")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            var appuserToDelete = await UserManager.FindByIdAsync(id);
            if (appuserToDelete == null)
            {
                return NotFound();
            }
            else
            {
                await UserManager.DeleteAsync(appuserToDelete);
            }
            return Ok();
        }

        [HttpGet]
        [Route("Login")]
        public async Task<IHttpActionResult> Login(string email, string password)
        {
            bool isEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            if(!isEmail)
            {
                UnitOfWork unitOfWork = new UnitOfWork(new ApplicationDbContext());
                email = await Task.Run(() => unitOfWork.Accounts.GetUserByPhoneNumber(email).Email);
            }

            var user = await Task.Run(() => UserManager.FindAsync(email, password));
            if (user == null)
            {
                return BadRequest("Username or Password is invalid.");
            }
            else
            {
                string token = TokenManager.GenerateToken(user.UserName);
                UserInformationDTO resultDTO = new UserInformationDTO()
                {
                    Token = token,
                    UserId = user.Id,
                    IsAdmin = user.IsAdmin
                };
                return Ok(resultDTO);
            }
        }

        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                string errorMessages = ErrorManager.GetModelStateErrors(ModelState);
                return BadRequest(errorMessages);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
