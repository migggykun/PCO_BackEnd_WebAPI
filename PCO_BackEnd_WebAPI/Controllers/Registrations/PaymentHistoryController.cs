using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using PCO_BackEnd_WebAPI.Security.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace PCO_BackEnd_WebAPI.Controllers.Attendance
{
    /// <summary>
    /// Controller for Payment History
    /// </summary>
    public class PaymentHistoryController : ApiController
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// default constructor. initialize database.
        /// </summary>
        public PaymentHistoryController()
        {
            _context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get all payment using user's id
        /// </summary>
        /// <param name="userId">user's id</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthFilter]
        [Route("api/GetPaymentHistory")]
        public async Task<IHttpActionResult> GetPaymentHistory(int userId)
        {
            List<Payment> paymentHistory = new List<Payment>();

            await Task.Run(()=> paymentHistory.AddRange(getPaymentHistory(userId)));

            return Ok(Mapper.Map<List<Payment>, List<Payment>>(paymentHistory));
        }

        private List<Payment> getPaymentHistory(int userId)
        {
            UnitOfWork unitOfWork = new UnitOfWork(_context);

            //Prepare payment report object
            Payment reportItem = new Payment();
            List<Payment> paymentReport = new List<Payment>();

            //get payments and properties, assign to payment report object
            List<Payment> payments = unitOfWork.Payments.GetAll().ToList();
            if (payments != null){
                payments = payments.Where(x => (x.MemberRegistration!=null && x.MemberRegistration.UserId == userId) || (x.Registration!=null && x.Registration.UserId == userId)).ToList();
                if(payments != null)
                {
                    foreach (Payment p in payments)
                    {
                        reportItem = new Payment();
                        var propInfo = p.GetType().GetProperties();
                        foreach (var item in propInfo)
                        {
                            if (item.CanWrite)
                            {
                                reportItem.GetType().GetProperty(item.Name).SetValue(reportItem, item.GetValue(p, null), null);
                            }
                        }

                        paymentReport.Add(reportItem);
                    }
                }
            }
            return paymentReport;
        }
    }
}