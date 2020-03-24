using PCO_BackEnd_WebAPI.Models.Entities;
using PCO_BackEnd_WebAPI.Models.Persistence.Interfaces.Registrations;
using PCO_BackEnd_WebAPI.Models.Registrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RefactorThis.GraphDiff;
using PCO_BackEnd_WebAPI.Models.Pagination;
namespace PCO_BackEnd_WebAPI.Models.Persistence.Repositories.Registrations
{
	public class PaymentRepository : Repository<Payment> , IPaymentRepository
	{
		public PaymentRepository (ApplicationDbContext context) : base(context)
		{

		}

		public PageResult<Payment> GetPagedPayments(int page, int size)
		{
			PageResult<Payment> pageResult = new PageResult<Payment>();
            int recordCount = appDbContext.Payments.Count();
            int mod;
            int totalPageCount;
            int offset;
            int recordToReturn;
            if (size == 0)
            {
                mod = 0;
                totalPageCount = 1;
                offset = 0;
                recordToReturn = recordCount;
            }
            else
            {
                mod = recordCount % size;
                totalPageCount = (recordCount / size) + (mod == 0 ? 0 : 1);
                offset = size * (page - 1);
                recordToReturn = size;
            }
			pageResult.Results =  appDbContext.Payments.OrderBy(p => p.RegistrationId)
											  .Skip(offset)
											  .Take(recordToReturn)
											  .ToList();

            pageResult.PageCount = totalPageCount;
            pageResult.RecordCount = recordCount;
			return pageResult;
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
			payment.RegistrationId = id;
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