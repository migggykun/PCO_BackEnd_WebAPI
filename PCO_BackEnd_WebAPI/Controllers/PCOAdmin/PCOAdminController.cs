using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Roles;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Conferences
{
    /// <summary>
    /// Controller class for pco admin values (website password, annual fees, reset all members)
    /// </summary>
    public class PCOAdminController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public PCOAdminController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// get annual fee for pco member
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetAnnualFee")]
        public async Task<IHttpActionResult> GetAnnualFee()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.GetAnnualFee());
            return Ok(result);
        }

        /// <summary>
        /// set annual fee for pco member
        /// </summary>
        /// <param name="newAnnualFee"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/SetAnnualFee")]
        public async Task<IHttpActionResult> SetAnnualFee(double newAnnualFee)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.UpdatePCOAdminDetails(newAnnualFee));
            await Task.Run(()=>unitOfWork.Complete());
            return Ok(result);
        }

        /// <summary>
        /// get website password (members only page)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetPassword")]
        public async Task<IHttpActionResult> GetPassword()
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.GetPassword());
            return Ok(result);
        }

        /// <summary>
        /// set password for (members only page)
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthFilter(PCO_Constants.ADMINISTRATOR_ACCESS)]
        [Route("api/SetPassword")]
        public async Task<IHttpActionResult> SetPassword(string newPassword)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);
            var result = await Task.Run(() => unitOfWork.PCOAdminDetail.UpdatePCOAdminDetails(null,newPassword));
            await Task.Run(() => unitOfWork.Complete());
            return Ok(result);
        }

    }
}
