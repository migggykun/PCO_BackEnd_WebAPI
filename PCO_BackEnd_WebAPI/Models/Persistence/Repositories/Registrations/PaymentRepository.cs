using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
    public class PaymentRepository : Repository<Payment> , IPaymentRepository
    {
        public PaymentRepository (ApplicationDbContext context) : base(context)
	    {

    	}

        public override void Add(Payment payment)
        {
            //Set details
            string transactionNumber = Guid.NewGuid().ToString();
            DateTime submissionDate = DateTime.Now;
            payment.PaymentSubmissionDate = submissionDate;
            payment.TransactionNumber = transactionNumber;
            appDbContext.Payments.Add(payment);
        }

        public void UpdatePayment(int id, Payment payment)
        {
            payment.Id = id;
            payment.TransactionNumber = Guid.NewGuid().ToString();
            payment.PaymentSubmissionDate = DateTime.Now;
            appDbContext.UpdateGraph<Payment>(payment);
        }

        public void SetPaymentConfirmationDate(int id)
        {
            var obj = appDbContext.Payments.Find(id);
            obj.ConfirmationDate = DateTime.Now;
            string x = obj.ToString();
        }

        private ApplicationDbContext appDbContext
        {
            get
            {
                return _context as ApplicationDbContext;
            }
        }
    }
}