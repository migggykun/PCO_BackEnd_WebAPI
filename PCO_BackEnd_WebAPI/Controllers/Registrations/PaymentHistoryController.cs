using AutoMapper;
using PCO_BackEnd_WebAPI.Models.Accounts;
using PCO_BackEnd_WebAPI.Models.Attendances;
using PCO_BackEnd_WebAPI.Models.Conferences;
using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Pagination;
using PCO_BackEnd_WebAPI.Models.Persistence.UnitOfWork;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PCO_BackEnd_WebAPI.Models.Helpers;

namespace PCO_BackEnd_WebAPI.Controllers.Attendance
{
    public class PaymentHistoryController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public PaymentHistoryController()
        {
            _context = new ApplicationDbContext();
        }



        [HttpGet]
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